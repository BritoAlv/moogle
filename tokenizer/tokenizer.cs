namespace tokenizer;
using docc;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

public partial class token
{
    List<string> hashes;

    public token()
    {
        hashes = System.IO.File.ReadAllLines("../cache/hashes.txt").ToList();
    }

public static string QuickHash(string secret)
{
    var sha256 = SHA256.Create(); // creates an instance og a sha256.
    var secretBytes = Encoding.UTF8.GetBytes(secret); // convert to bytes the string.
    var secretHash = sha256.ComputeHash(secretBytes); // compute its hash.
    return Convert.ToHexString(secretHash); // return the hash.
}

public void process(string text, int id, Dictionary<string, info_word_doc> bdd)
{    
    // compute hash of the text,
    string hash = QuickHash(text);
    // un parche 
    text = text + " ";
    // un parche
    if (!this.hashes.Contains(hash))
    {

        // we have to split the texxt and while.
        // update the dict bdd.
        // update a temp dict to generate the two jsons.
        // generate json and write this hash to the list of common hashes.
        Dictionary<string , double> tf_to_json = new Dictionary<string, double>();
        Dictionary<string, List<int>> pos_to_json = new Dictionary<string, List<int>>();
        for (int i = 0; i < text.Length; i++)
        {
            // reach an index that is alphanumeric
            if(char.IsLetterOrDigit(text[i]))
            {
                int start = i;
                string word = (text[i]).ToString();
                // move i to where the word ends
                i = i+1;
                while(char.IsLetterOrDigit(text[i]))
                {
                    word = word + text[i];
                    i = i+1;
                }

                if (!bdd.ContainsKey(word))
                {
                    bdd[word] = new info_word_doc(word);
                }
                if (!bdd[word].docs.ContainsKey(id))
                {
                    bdd[word].docs[id] = new info_word();
                }

                if(!tf_to_json.ContainsKey(word))
                {
                    tf_to_json[word] = 0;
                }
                if(!pos_to_json.ContainsKey(word))
                {
                    pos_to_json[word] = new List<int>();
                }                

                tf_to_json[word]++;
                bdd[word].docs[id].pos.Add(i);
                pos_to_json[word].Add(i);        
            }
        }
        int len = tf_to_json.Count();
        foreach (var item in tf_to_json)
        {
            bdd[item.Key].idf++;
            tf_to_json[item.Key] = tf_to_json[item.Key] / (double)(len);
            bdd[item.Key].docs[id].weight = tf_to_json[item.Key];
        }
        // serialize the dicts and save to the cache folder but how id it with the name of the hash.
        string jsonString1 = JsonSerializer.Serialize(tf_to_json);
        string jsonString2 = JsonSerializer.Serialize(pos_to_json);
        File.WriteAllText("../cache/"+hash+"1.json", jsonString1);
        File.WriteAllText("../cache/"+hash+"2.json", jsonString2);
        hashes.Add(hash);
        System.IO.File.WriteAllLines("../cache/hashes.txt", hashes);
    }
    else
    {
        // load the json of the doc and update its words in the databse.
        Dictionary<string , double> tf_to_json = JsonSerializer.Deserialize<Dictionary<string, double>>(File.ReadAllText("../cache/"+hash+"1.json"));
        Dictionary<string , List<int>> pos_to_json = JsonSerializer.Deserialize<Dictionary<string, List<int>>>(File.ReadAllText("../cache/"+hash+"2.json"));

        foreach (var item in tf_to_json)
        {
            if (!bdd.ContainsKey(item.Key))
            {
                bdd[item.Key] = new info_word_doc(item.Key);
            }
            if (!bdd[item.Key].docs.ContainsKey(id))
            {
                bdd[item.Key].docs[id] = new info_word(pos_to_json[item.Key]);
            }
            bdd[item.Key].idf++;
            bdd[item.Key].docs[id].weight = tf_to_json[item.Key];
        }
    }

}

}  


