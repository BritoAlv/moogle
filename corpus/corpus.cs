namespace corpuss;
using docc;
using tokenizer;

public partial class corpus
{
    public string[] words;
    public doc[] the_docs;
    public int cant_docs;
    Dictionary<string, info_word_doc> bd;
    public corpus(bool use_cache_stemmer = false)
    {
        this.bd = new Dictionary<string, info_word_doc>();
        DirectoryInfo location = new DirectoryInfo("../Content");
        FileInfo[] files = location.GetFiles();
        this.cant_docs = files.Count();
        this.the_docs = new doc[cant_docs];
        token k = new token();
        for (int i = 0; i < cant_docs; i++)
        {
            the_docs[i] = new doc(files[i].Name, i);
            /*
            now based on the .text field of this doc i need to get its relevant information,
            and mix it in the .bd dictionary of the corpus. the object token I just created
            will take care of that, 
            */
            k.process(the_docs[i].text, i, bd);
        }

        // i need this to do the levensthein algorithm 
        words = bd.Keys.ToArray();
        Array.Sort(words, (x, y) => x.Length.CompareTo(y.Length));
        // end of stuff.

        /*
        at this point we have to update tf-idf.
        */
        k.update_tf_idf(bd, this.cant_docs);

        /*
        at this point apply the stemmer to update linked words.
        */        
        k.stem(bd, use_cache_stemmer);

        /*
        at this point apply the synonimus to update linked words.
        */
        k.sinonyms(bd);

        k.group_similar_words(bd);         
    }
    
/*
at this point the corpus is ready to use, only is left to implement the methods for using it,
to get a specific word in a document we do corpus.bd[word].docs[#id of document], 
if this is null we return an empty object, our methods to deal with score will work even if
the info passed to it is empty. 

I will write methods to retrieval information from the corpus in a diferent file.
*/



}

