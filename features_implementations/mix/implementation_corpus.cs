public class corpus
{
    public Dictionary<string, int> idf {get;set;}
    public corpus()
    {
        this.idf = new Dictionary<string, int>();
    }
    public void update_corpus(string[] words)
    {
        foreach (string word in words)
        {
            if (!this.idf.ContainsKey(word))
            {
                this.idf[word] = 0;
            }
            this.idf[word] += 1;
        }
    }
}