public class corpus
{
    public Dictionary<string, int> idf {get;set;}
    public int number_of_docs {get;set;}
    public corpus()
    {
        this.idf = new Dictionary<string, int>();
        this.number_of_docs = 0;
    }
    public void update_corpus(string[] words)
    {
        number_of_docs +=1;
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