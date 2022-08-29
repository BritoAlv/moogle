// in the corpus will be all the words found in the documents, will be its idf, and its similar words and sinonimos. Also a trie to do autocomplete
public class corpus
{
    // for each word we need to store its idf, this is initially, after all documents have been created, we pass this information to each document, and we don't need it anymore, but now we need to for each word store its linked one, and also for some words, store a list which will be the ones that are linked to it.
    public doc[] the_docs;
    public Dictionary<string, int> idf; // store words with its idf
    public Dictionary<string, string> words_linked; // for each words we assign its similar one
    public Dictionary<string, string[]> linked_list; // for each root word we assign its similars.
    public trie triee;
    public int number_of_docs;
    // the trie to do autocomplete.
    public corpus()
    {
        idf = new Dictionary<string, int>(); 
        number_of_docs = 0;
        DirectoryInfo folder = new DirectoryInfo("./Content");
        FileInfo[] books = folder.GetFiles(); // names of files in  list
        this.the_docs = new doc[books.Length];
        for (int i = 0; i < books.Length; i++)
        {
            the_docs[i] = new doc(books[i].Name, this);
        }
        // recreate the idf dict so that its keys are added in length order, this is used for the levensthein alg.

        idf = alg.sort_dict_length(idf);

        // en este punto ya podemos calcular el tf-idf de cada documento.    
        for (int i = 0; i < books.Length; i++)
        {
                the_docs[i].compute_tf_idf(this);
        }
        triee = new trie();
        foreach(string word in this.idf.Keys)
        {
            triee.Insert(word);
        }
        // en este punto usamos el stemmer
        words_linked = stemmer.stem(this.idf.Keys.ToArray());
        // the stemmer linked the words, but now how do i apply sinonymus
        words_linked = syn.work(words_linked); 
        linked_list = alg.link_words(words_linked); // this is an abstraction.

        // at this point execute the algorithm to reduce the keys of a document given the linked_list.
        for (int i = 0; i < books.Length; i++)
        {     
            /* 
            Now for each document we have to re-build its data, we loop through the linked_lists and we take the first element of the document in each list, if there is one, we mix the keys of all the elements in that list that are in that document, and we build a new dict like the preview. the reamining keys will have null objects, but exist this allow us to check if a key is in some dict.

            why this works?, if we want to find a word in some document, we go through its linked list in the corpus, if there there is some word that there is in the document, we accept the word in the document, and get its info by the dict we build, now the problem is how to check between those words which one are in the document.
            */
            the_docs[i].mix_keys(this);
            // at this point we can compute the norm of the document. (only consider non-null values of the keys.)
            the_docs[i].set_norm();
        }



        // having the trie done, the documents ready to compute stuff, what is missing.
        // goal is make a frontend, backend, so .... 
        

    }

    public void update_corpus(doc a)
    {
        string[] words = a.dic.Keys.ToArray();
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

/* 

What's left

- ya el corpus tiene los documentos con las keys mixed, con sus tf-idf fine y su norma, mi punto es que la query, necesita cierta información de los documentos, para ejecutarse, mi objetivo por hoy es lograr tener implementado los metodos que le darán esa información a la query, para otro día ponerme en funcióin de ella. El algoritmo de levenshtein necesita para funcionar digamos que es escrito ospital, ya tengo como acceder a las llaves del dict para lo de levesnthein, what else, vamos a simular le proceso, el usuario intriduce una query, para cada palabra de la query se halla si está en el documento o no, esto es hecho buscando en la lista de palabras asociadas a ella en el corpus si alguna de ellas está en el documento, si no hay ninguna no lo está si hay alguna la primera es la llave por la que la podemos buscar en el documento, teniendo eso si la palabra hay q buscarla explicita aplicamos el algoritmo de calcular su info sin las otras palabras, recall that the info objects need to be sorted always, si no pasamos el info object que encontramos, para el operador de cercanía lo que hace falta son estos info_objects sorted, el snippet necesita el texto y necesita los info_objects de las palabras que va a buscar, así que nada pongamonos en marcha de impleentar todo lo que falta, para poder tener el corpus pinchando. el tf-idf funciona de la siguiente manera dadas las palabras de la query, las definimos como vectores con tf-idf, respecto a la query, calculando su norma también ahora para el documento se calcula multiplicando los weight de cada info object correspondiente, y dividiendolo entre la norma del documento, esto no parece so hard. Remember i have three scoring funcitons. let's worksss.s 

 */