/* 

*/
namespace qquery;
using corpuss;
using d_t_h;
public partial class query
{
    string q; // the query
    Dictionary<string, int> words; // words in the query with its id.
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
    
    List<string> word_to_suggest;
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
        this.word_to_suggest = new List<string>();

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
                    op_cerc.Add(clo);
                    for (int i = 0; i < clo.Length; i++)
                    {
                        if (x.word_in_corpus(clo[i]) && !forbidden_words.Contains(clo[i])) // find only words that are not forbidden and are in the corpus.
                        {
                            closest_words.Add(clo[i]); // keep track of all words that are related by ~
                        }
                        
                    }
                }
            }
        }

    ////////////////////////////////////////////////////////////////////////////// 
    // set tf-idf to each word in the query.
    // compute norm of the query.
    // ignore words that are in 95% of documents if there are more than 20 docs and not in ~.
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
            word_to_suggest.Add(item.Key);
        }
        else
        {
            double idf_word = x.request_word_idf(item.Key, similar_words.Contains(item.Key));
            tfidf[item.Key] = (tfidf[item.Key] / len ) * (x.cant_docs) / (double)(idf_word+1) ;
            norm = norm + (tfidf[item.Key])*(tfidf[item.Key]);
            if (idf_word >= (0.95)*x.cant_docs && (x.cant_docs > 20) && !(closest_words.Contains(item.Key)))
                {
                    if(!only_words.Contains(item.Key))
                    {
                        common_words.Add(item.Key);
                    }
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

    double max = tfidf.Values.Max();
    foreach (var item in boosted_words)
    {
        tfidf[item.Key] = max*item.Value;
    }

    ////////////////////////////////////////////////////////////////////////////// 
    // filter the docs to consider due to ^^ !!.
    //////////////////////////////////////////////////////////////////////////////    

    List<int> index_of_docs_to_consider = new List<int>();
    // we loop through the docs to and stay with the ones that doens't contain forbidden words and contain only words.
    for (int i = 0; i <x.cant_docs; i++)
    {
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

    for (int i = 0; i < x.cant_docs; i++)
    {
        Console.WriteLine(i + " " + x.the_docs[i].name + " " + score_by_tfidf[i]+ "  " + score_by_cercania[i] + "  " + score_by_min_interval[i]);
    }

     

    }
}
