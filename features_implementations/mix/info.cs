public class info {
    public double term_frequency{get;set;}
    public int stemed {get;set;}
    public double tf_idf{get;set;}
    public List<Tuple<int,int>> positions{get; set;}
    public string linked{get; set;}
    public info(string word)
    {
        term_frequency = 0;
        positions = new List<Tuple<int, int>>();
        linked = word;
        stemed = 1;
    }

    // the goal of this method is mix the keys that point to the same word, like ayudo-ayudar.

    // so we create an object instance, we compute each word, after that apply stemmer, and after that update the dict.
    public static Dictionary<string,info> mix_keys(Dictionary<string, info> the_dict, Dictionary<string,string> link_dict)
    {
        // create a new dict.
        Dictionary<string, info> result = new Dictionary<string, info>();
        // loop through keys in link_dict.
        foreach( KeyValuePair<string,string> k in link_dict )
        {
            if(!result.ContainsKey(k.Value))
            {
                result[k.Value] = new info(k.Value);
            }
            // update the key
            result[k.Value].term_frequency += the_dict[k.Key].term_frequency;
            // stemed is the number of words that get stemmed to this word.
            result[k.Value].stemed +=1;
            foreach (Tuple<int,int> position in the_dict[k.Key].positions)
            {
                result[k.Value].positions.Add(position);
            } 
        }
        return result;
    } 
}