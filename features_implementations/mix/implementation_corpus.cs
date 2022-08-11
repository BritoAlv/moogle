public class corpus
{
    public Dictionary<string, int> idf {get;set;}
    
    public static void update_corpus(Dictionary<string, int> target, string[] words)
    {
        foreach (string word in words)
        {
            if (!target.ContainsKey(word))
            {
                target[word] = 0;
            }
            target[word] += 1;
        }
    }
}