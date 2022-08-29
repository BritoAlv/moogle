/* 
Stop_Words.
we need to define an estructure for the query
its words: string[]
ignored_words: string[]
close_words: a list of string[]
notallowedwords: string[]
onlyifwords: string[]
explicitwords:
Now for each document taking in-count this structures we will generate its three scores based on tf-idf, operador de cercania, min interval, for each document we get what we need respect to the query,  for example to do the operator of closeness what i need is 

*/

public class query
{
    public string the_query;
    public Dictionary<string ,double> words;

    public string[] forbidden_words;
    public string[] onlyif_words;
    public string[] explicit_words;
    public List<string[]> close_words;
    public string[] ignored_words;
    public Dictionary<int, Tuple<double,double,double> > scores;
    public double norm;
    public int[] working_docs;
    public query(string a, corpus X)
    {
        // obtener las palabras de la query  y ponerlas en el dict words, con tf 1 por default, si una palabra tiene un ! (no puede aparecer hacemos su tf 0), si tiene * esta palabra debe tener el doble de prioridad que cualquier palabra de lla query, asi que calculamos el maximo tf-idf que hay y lo multiplicamos por 2.
        this.the_query = a;
        this.words = parser.query_words(this.the_query);
        this.words = parser.update_idf(this.words, X);
        this.forbidden_words = parser.forbiddenwords(the_query);
        the_query = the_query.Replace("!", "");
        this.onlyif_words = parser.onlywords(the_query);
        the_query = the_query.Replace("^", "");
        this.explicit_words = parser.explicitt(the_query);
        the_query = the_query.Replace("¿", "");
        this.words = parser.important_words(the_query, this.words);
        the_query = the_query.Replace("*","");
        this.close_words = parser.close_words(the_query);
        this.ignored_words = parser.ignore(this.words, X);

        // una vez que está hecha la query definir los documentos con los cuales se va a trabajar o sea me refiero
        List<int> aa = new List<int>();
        for (int i = 0; i < X.number_of_docs; i++)
        {
            bool e = true;
            foreach (var word in forbidden_words)
            {
                if(X.the_docs[i].contain_word(word, X))
                {
                    e = false; 
                    break;
                }
            }
            if(e){aa.Add(i);}    
        }
        int c = aa.Count;
        for (int i = 0; i < c; i++)
        {
            bool e = false;
            foreach (var word in onlyif_words)
            {
                if(!X.the_docs[aa[i]].contain_word(word, X))
                {
                    e = true;
                    break;
                }
            }
            if(e){aa.Remove(i);} 
        }
        double n = 0.000001;
        foreach (var item in this.words)
        {
            n = n + item.Value*item.Value;
        }
        this.norm = Math.Sqrt(n);
        working_docs = aa.ToArray();
        scores = new Dictionary<int, Tuple<double, double, double>>();
        foreach (var item in working_docs)
        {
            this.scores[item] = ranking.rank(X.the_docs[item], this, X);
        }



    }

}

