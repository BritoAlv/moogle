// this class will save the info of each word in a document,
public class info
{
    public double weight;
    public List<pos> places;
    
    public info(double weight = 0)
    {
        this.weight = weight;
        this.places = new List<pos>();
        
    }
    public info(List<pos> some_pos , double weight = 0)
    {
        this.weight = weight;
        places = pos.sort(some_pos); // always sorted
    }

    // function to be used when doing an explicit search of the word.
    public static info one_word(info a, string word, doc A)
    {
        // "word" is an word that there is in "a", our goal is only use the "a" word.
        // this take an info object and delete from it all the word that are no "word", 
        // its weight is the tf-idf, in the document, can be obtained from the "a" object, and the idf comes from the corpus, recall that the "word" to do this have to be in the doc.
        List<pos> aa = new List<pos>();
        foreach (pos item in a.places)
        {
            if(item.word == word)
            {
                aa.Add(item);
            }
        }
        return new info(aa, A.dic[word].weight);
    }
}
