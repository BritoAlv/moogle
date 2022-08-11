public class document
{
    public string name {get; set;}
    public string text {get; set;}
    public Dictionary<string, info>  initial_words {get; set;}
    public Dictionary<string, string> link_dict{get; set;}

    public document(string title, corpus x)
    {
        name = title;
        text = tokenization.read_txt(name);
        initial_words = tokenization.words_in_document(text);
        link_dict = stemmer.stem(initial_words.Keys.ToArray());
        // set words to the corpus.
        corpus.update_corpus(x.idf, initial_words.Keys.ToArray() );
    }

    public static void update_idf(Dictionary<string, info> target, corpus x, Dictionary<string,string> link)
    {
        // take the words from link, find its idf value in corpus, and puts in the target.

        // count will be the number of words that there are in the document, its link keys length.
        int count = 0; 
        foreach (KeyValuePair<string, string> k in link )
        {
            target[k.Value].tf_idf = x.idf[k.Key];
            count +=1;
        }

        // update tf, update idf.
        foreach (KeyValuePair<string,info> k in target)
        {
            target[k.Key].tf_idf = target[k.Key].tf_idf / ((double)target[k.Key].stemed);
            target[k.Key].term_frequency = target[k.Key].tf_idf / ((double)count);
        } 
    }
}

// when a document it's created it will tokenize its words, and after that we will stem them, so at the end we have a dict which contain the links, and other that contain the words with its term frequency, at this point we take all the words in the link_dict and send them to the corpus, now we have to wait until all document objects are created, after that we define the idf, to make this we need another function that updates idf, but the thing is how set idf of updated words. It has to be the mean of the words. 
