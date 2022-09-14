namespace corpuss;
using docc;
using tokenizer;

public partial class corpus
{
    public string[] words; // all the words in the corpus.
    public int[] index; // need this to do levensthein algorithm.
    public trie the_trie; // well a trie to autocomplete.
    public doc[] the_docs; // information about each doc.
    public int cant_docs; // total of docs.
    public Dictionary<string, info_word_doc> bd; // the info is here.
    public corpus()
    {
        bool use_cache_stemmer = true; // by default it will use the stemmer cache
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
            k.process(the_docs[i].name, the_docs[i].text, i, bd);
        }
        
        ////////////////////////////////////////////////////////////////////////////// 
        //  logic for updating hashes.txt and decide if use stemmer
        //////////////////////////////////////////////////////////////////////////////

        if (k.new_hashes.Count > 0 || !(k.hashes_on_txt.Count == k.old_hashes.Count))
        {
            use_cache_stemmer = false; // if this conditions are hold this mean that there is a change on the content folder so we need to re-run the stemmer
        }
        List<string> hashes_now = new List<string>();
        foreach (var item in k.new_hashes)
        {
            hashes_now.Add(item);
        }
        foreach (var item in k.old_hashes)
        {
            hashes_now.Add(item);
        }

        System.IO.File.WriteAllLines("../cache/hashes.txt", hashes_now);        

        ////////////////////////////////////////////////////////////////////////////// 
        // Levensthein Algorithm Part;
        ////////////////////////////////////////////////////////////////////////////// 
        words = bd.Keys.ToArray();
        Array.Sort(words, (x, y) => x.Length.CompareTo(y.Length));

        // this index will store in the position k the first position in the words array
        // of a word with length k.
        index = new int[31];
        for (int i = 1; i < 31; i++)
        {
            for (int j = 0; j < words.Length; j++)
            {
                if(words[j].Length == i)
                {
                    index[i] = j; 
                    break;
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////// 
        //  Trie Part
        //////////////////////////////////////////////////////////////////////////////

        the_trie = new trie();
        foreach (var word in words)
        {
            the_trie.insert(word);
        }
        
        // end of stuff.

        /*
        at this point we have to update tf-idf.
        */
        k.update_tf_idf(bd, this.cant_docs);

        /*
        at this point we can set the norm of each document.
        */

        for (int i = 0; i<cant_docs; i++) 
        {
            double norm = 0;
            foreach (var item in this.bd) // loop through the words 
            {
                if (item.Value.docs.ContainsKey(i)) 
                {
                    norm = norm + (item.Value.docs[i].weight * item.Value.docs[i].weight);
                }
            }
            the_docs[i].norm = Math.Sqrt(norm);
        }
        

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

