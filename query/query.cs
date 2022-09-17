/* 

*/
namespace qquery;
using corpuss;
using d_t_h;
public partial class query
{
    string q; // the query
    public Dictionary<string, int> words; // words in the query with its id.
    List<string[]> op_cerc; // words that are related by ~.
    HashSet<string> only_words; // words that have to be forced.
    HashSet<string> forbidden_words; // words that can't be.
    Dictionary<string, int> boosted_words; // words that have more priority.
    HashSet<string> similar_words; // words that we can find its similar and sinonyms.
    HashSet<string> common_words; // words that will be ignored becuase are so common.
    Dictionary<string, double> tfidf; // tf-idf words in the query.
    public HashSet<string> closest_words; // ignore word that are affected by at least one operator of cercania
    public HashSet<string> words_to_request; // words that are not ignored and allowed. 
    public double[] score_by_tfidf;
    public double[] score_by_cercania;
    public double[] score_by_min_interval;
    public Dictionary<int, List<id_element<int>>> relevant_info;

    public Dictionary<int, double[]> medallas;
    public snippet[] the_snippets;
    public List<string> words_to_suggest;
    public Tuple<int, string,string,string>[] best_docs;
    double norm;

    public query(string s, corpus x)
    {
        ////////////////////////////////////////////////////////////////////////////// 
        //save all the relevant info of the query. Why use A HashSet and not something else;
        //////////////////////////////////////////////////////////////////////////////
        this.q = s + " ";
        this.words = new Dictionary<string, int>();
        this.only_words = new HashSet<string>();
        this.forbidden_words = new HashSet<string>();
        this.similar_words = new HashSet<string>();
        this.common_words = new HashSet<string>();
        this.tfidf = new Dictionary<string, double>();
        this.boosted_words = new Dictionary<string, int>();
        this.closest_words = new HashSet<string>();
        int found_words = 0;
        this.op_cerc = new List<string[]>();
        this.words_to_request = new HashSet<string>();
        this.words_to_suggest = new List<string>();
        this.medallas = new Dictionary<int, double[]>();
        this.the_snippets = new snippet[x.cant_docs];
        ////////////////////////////////////////////////////////////////////////////// 
        // start looping the query to get its word;
        //////////////////////////////////////////////////////////////////////////////

        for (int i = 0; i < q.Length; i++)
        {
            if (char.IsLetterOrDigit(q[i]))
            {
                int start = i;
                i++;
                while (char.IsLetterOrDigit(q[i]))
                {
                    i++;
                }
                int end = i-1;
                string word = q.Substring(start, end-start+1);

                ////////////////////////////////////////////////////////////////////////////// 
                // giving an id to each word query and keep track of its tf;
                //////////////////////////////////////////////////////////////////////////////

                if (!words.ContainsKey(word)) // first time that we found this word.
                {
                    // id is found_words.
                    words[word] = found_words;
                    tfidf[word] = 1;
                    found_words++;
                }
                else // this word is not new. its id is words[word]
                {
                    tfidf[word]++;
                }
                
                ////////////////////////////////////////////////////////////////////////////// 
                //operators logic;
                //Operators:
                //* boosted_words.
                //! forbidden_words.
                //^ only_words.
                //¿ similar_words.
                //////////////////////////////////////////////////////////////////////////////
                for (int j = start-1; j >= 0; j--)
                {

                    ////////////////////////////////////////////////////////////////////////////// 
                    //save in a dict how many **** got a word;
                    //////////////////////////////////////////////////////////////////////////////
                    if(q[j]  == '*')
                    {
                        if (!boosted_words.ContainsKey(word))
                        {
                            boosted_words[word] = 0;
                        }
                        boosted_words[word]++;
                    }
                    else if (q[j] == '!')
                    {
                        forbidden_words.Add(word);
                    }
                    else if (q[j] == '^')
                    {
                        only_words.Add(word);
                    }
                    else if (q[j] == '¿')
                    {
                        similar_words.Add(word);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////// 
        // end looping the queey now seek for the ~~~~ words;
        //////////////////////////////////////////////////////////////////////////////
        this.q = string.Join("", this.q.Split('*', '!' ,'^' ,'¿'));  // from stackoverflow they say its the better perfomance solution, would be interesting to test that. 
        // now the string is formed by the word, spaces and ~,
        if (q.Contains("~"))
        {        
            string[] temp = q.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in temp)
            {
                if(item.Contains("~"))
                {
                    string[] clo = item.Split("~", StringSplitOptions.RemoveEmptyEntries);
                    clo = clo.Where(y => (x.word_in_corpus(y) && !forbidden_words.Contains(y))).ToArray();
                    op_cerc.Add(clo);
                    for (int i = 0; i < clo.Length; i++)
                    {
                            closest_words.Add(clo[i]); // keep track of all words that are related by ~
                    }
                }
            }
        }

    ////////////////////////////////////////////////////////////////////////////// 
    // set tf-idf to each word in the query.
    // compute norm of the query.
    // select the words that we are going to request/ take in account. These are the ones that doesn't
    // belong to the forbidden ones and the common ones, also this ensure that the words of op_cerc are 
    // a subset of request_words.
    //////////////////////////////////////////////////////////////////////////////
    double len = tfidf.Values.Sum();
    foreach (var item in words)
    {
        if(!x.word_in_corpus(item.Key))
        {
            tfidf[item.Key] = 0;
            common_words.Add(item.Key);
            words_to_suggest.Add(item.Key);
        }
        else
        {
            double idf_word = x.request_word_idf(item.Key, similar_words.Contains(item.Key));
            tfidf[item.Key] = (tfidf[item.Key] / len ) * (x.cant_docs) / (double)(idf_word+1) ;
            norm = norm + (tfidf[item.Key])*(tfidf[item.Key]);
            // ignore words that are in the 98% of the docs if there are more than 20 docs.
            /*
            this introduce the problem that if our database is about harry potter books, the word harry will be ignored always,
            when doing a normal search without ^~. 
            */
            if ( !(only_words.Contains(item.Key)) && !(boosted_words.ContainsKey(item.Key)) && (idf_word >= 0.90*x.cant_docs) && !(closest_words.Contains(item.Key)) && (x.word_is_popular(item.Key)))
            {
                    common_words.Add(item.Key);
            }
        }    
        if (!(common_words.Contains(item.Key)) && !(forbidden_words.Contains(item.Key)))
        {
            words_to_request.Add(item.Key);
        }
        
    }
    norm = Math.Sqrt(norm);
    if (norm == 0){norm = 1;}


    ////////////////////////////////////////////////////////////////////////////// 
    // stuff of ****
    //////////////////////////////////////////////////////////////////////////////
    foreach (var item in boosted_words)
    {
        tfidf[item.Key] = Math.Pow(10, boosted_words[item.Key]+1)*tfidf[item.Key];
    }

    ////////////////////////////////////////////////////////////////////////////// 
    // filter the docs to consider due to ^^ !!.
    //////////////////////////////////////////////////////////////////////////////    

    List<int> index_of_docs_to_consider = new List<int>();
    // we loop through the docs to and stay with the ones that doens't contain forbidden words and contain only words.
    for (int i = 0; i <x.cant_docs; i++)
    {   
        the_snippets[i] = new snippet(i);
        medallas[i] = new double[3];
        bool d = true;
        /*
        forbidden words.
        */
        foreach (var word in forbidden_words)
        {
            if(x.word_in_corpus(word) && x.bd[word].docs.ContainsKey(i))
            {
               d = false;
               break; 
            }
        }
        /*
        only if words.^
        */
        foreach (var word in only_words)
        {
            if (!x.word_in_corpus(word))
            {
                d = false;
                break;
            }
            else if(x.word_in_corpus(word) && !x.bd[word].docs.ContainsKey(i))
            {
               d = false;
               break; 
            }
        }
        if (d)
        {
            index_of_docs_to_consider.Add(i);
        }        
    }

    ////////////////////////////////////////////////////////////////////////////// 
    // start scoring part creating arrays for each scorer and a relevant info
    // dictionary to keep all the information of each word requested to the query. 
    //////////////////////////////////////////////////////////////////////////////

    this.score_by_tfidf = new double[x.cant_docs];
    this.score_by_cercania = new double[x.cant_docs];
    this.score_by_min_interval = new double[x.cant_docs];
    this.relevant_info = new Dictionary<int, List<id_element<int>>>();


    scorers(x, index_of_docs_to_consider);

    ////////////////////////////////////////////////////////////////////////////// 
    // At this point we have sorted score based on three components now decide what
    // document is better having these three scores is what follows.
    //////////////////////////////////////////////////////////////////////////////


    ////////////////////////////////////////////////////////////////////////////////
    // Select best results based on medals, the medals in the category op cercania 
    // are the better ones. 
    /////////////////////////////////////////////////////////////////////////////////
    best_docs = get_medals();
    
   
    // first ten elements in the medallas array son nuestro resultados.

/*     foreach( var doc in  best_docs)
    {
        Console.WriteLine(x.the_docs
        [doc.Item1].name + " " + doc.Item2+doc.Item3+doc.Item4);
        Console.WriteLine("Este es el snippet chama: ");
        Console.WriteLine(the_snippets[doc.Item1].generate_snippet(x));
    }*/ 

     

    }
}
