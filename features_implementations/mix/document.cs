public class document
{
    public string name {get; set;}
    public string text {get; set;}
    public Dictionary<string, info>  inital_words {get; set;}
    public Dictionary<string, string> link_dict{get; set;}

    public document(string title)
    {
        name = title;
        text = tokenization.read_txt(name);
        inital_words = tokenization.words_in_document(text);
        link_dict = stemmer.stem(inital_words.Keys.ToArray());
        
    }
}

// when a document it's created it will tokenize its words, and after that we will stem them, so at the end we have a dict which contain the links, and other that contain the words with its term frequency, at this point we take all the words in the link_dict and send them to the corpus, now we have to wait until all document objects are created, after that we define the idf 
