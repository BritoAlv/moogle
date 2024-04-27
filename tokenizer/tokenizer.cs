namespace tokenizer;
using docc;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

public partial class token
{
    public List<string> hashes_on_txt; // load the hashes in disk.
    public List<string> new_hashes; // new hashes
    public List<string> old_hashes; // the hashes that are in the disk

    public token()
    {
        hashes_on_txt = System.IO.File.ReadAllLines("../cache/hashes.txt").ToList();
        new_hashes = new List<string>();
        old_hashes = new List<string>();
    }

public static string QuickHash(string secret)
{
    var sha256 = SHA256.Create(); // creates an instance og a sha256.
    var secretBytes = Encoding.UTF8.GetBytes(secret); // convert to bytes the string.
    var secretHash = sha256.ComputeHash(secretBytes); // compute its hash.
    return Convert.ToHexString(secretHash); // return the hash.
}

public void process(string doc_name,string text, int id, Dictionary<string, info_word_doc> bdd)
{   

    // compute hash of the text,
    string hash = QuickHash(text);
    // un parch 
    text = text + " ";
    // un parch
    if (!this.hashes_on_txt.Contains(hash))
    {
     
        // we have to split the text and while.
        // update the dict bdd.
        // update a temp dict to generate the two json.
        // generate json and write this hash to the list of common hashes.
        Dictionary<string , double> tf_to_json = new Dictionary<string, double>();
        Dictionary<string, List<int>> pos_to_json = new Dictionary<string, List<int>>();

        ///////////////////////////////////////////////////////////////////
        // DEFINING A PROCEDURE TO ADD WORDS TO THE DICTS, .
        ///////////////////////////////////////////////////////////////////

        void add_word(string item, int i)
        {
            if (!bdd.ContainsKey(item))
            {
                bdd[item] = new info_word_doc(item);
            }
            if (!bdd[item].docs.ContainsKey(id))
            {
                bdd[item].docs[id] = new info_word();
            }
            if(!tf_to_json.ContainsKey(item))
            {
                tf_to_json[item] = 0;
            }
            if(!pos_to_json.ContainsKey(item))
            {
                pos_to_json[item] = new List<int>();
            }                
            tf_to_json[item]++;
            bdd[item].docs[id].pos.Add(i);
            pos_to_json[item].Add(i); 
        }

        ///////////////////////////////////////////////////////////////////
        // START LOOPING THE TEXT.
        ///////////////////////////////////////////////////////////////////

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
                ///////////////////////////////////////////////////////////////////
                // Will add the word in lower and if it is the case the upper also.
                ///////////////////////////////////////////////////////////////////
                add_word(word,i);
                if (char.IsUpper(word[0]))
                {
                    add_word(word.ToLower(),i);
                }
            }
        }
        double len = tf_to_json.Values.Sum(); // cant of words in the document. allowing repeated ones.
        foreach (var item in tf_to_json)
        {
            bdd[item.Key].idf++;
            tf_to_json[item.Key] = tf_to_json[item.Key] / (double)(len);
            bdd[item.Key].docs[id].weight = tf_to_json[item.Key];
        }
        // serialize the dicts and save to the cache folder but how id it with the name of the hash.
        string jsonString1 = JsonSerializer.Serialize(tf_to_json);
        string jsonString2 = JsonSerializer.Serialize(pos_to_json);
        File.WriteAllText("../cache/"+doc_name+"1.json", jsonString1);
        File.WriteAllText("../cache/"+doc_name+"2.json", jsonString2);
        new_hashes.Add(hash);
    }
    else
    {
        // load the json of the doc and update its words in the database.
        Dictionary<string , double> tf_to_json = JsonSerializer.Deserialize<Dictionary<string, double>>(File.ReadAllText("../cache/"+doc_name+"1.json"));
        Dictionary<string , List<int>> pos_to_json = JsonSerializer.Deserialize<Dictionary<string, List<int>>>(File.ReadAllText("../cache/"+doc_name+"2.json"));

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
        old_hashes.Add(hash);
    }

}

}  



