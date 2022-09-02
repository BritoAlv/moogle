namespace corpuss;
using docc;
using tokenizer;
public class corpus
{
    public doc[] the_docs;
    public int cant_docs;
    Dictionary<string, info_word_doc> bd;
    public corpus()
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
        /*
        at this point for each we have to update tf-idf, and apply the stemmer to update the links.
        */

    /*
    at this point the corpus is ready to use, only is left to implement the methods for using it, to get a specific word in a document we do corpus.bd[word].docs[#id of document], if this is null we return an empty object, to obtain similar words of a word we 
    */

    }


}

