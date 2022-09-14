namespace MoogleEngine;
using System.Diagnostics;
// with corpus. access to the classes of the Library Class corpus
using corpuss;
using string_algss;
using qquery;

public class Moogle
{
    public string time = "0s";
    public corpus x = new corpus();
    public query the_query;
    public SearchResult Query(string la_query)
    {
        Stopwatch a = new Stopwatch();
        a.Start();
        this.the_query = new query(la_query, this.x);
        a.Stop();
        this.time = ((double)(a.ElapsedMilliseconds) / 1000).ToString() + "s";
        List<SearchItem> items = new List<SearchItem>();
        foreach (var doc in this.the_query.best_docs)
        {
            items.Add(new SearchItem(x.the_docs[doc.Item1].name + " ---> " + doc.Item2 + doc.Item3 + doc.Item4, this.the_query.the_snippets[doc.Item1].generate_snippet(x)));
        }
        return new SearchResult(items.ToArray(), la_query);
    }
    public string get_suggestion(string actual_query)
    {
        // take first word in words_to suggest and find a similar word to it, now in the query, now loop the
        // query and if found old word change it by new, if we have a new word, update query.
        int edit_distance = 10;
        string old_word = the_query.words_to_suggest[0];
        string new_word = old_word;
        if (old_word.Length > 2)
        {
            int start = x.index[old_word.Length - 2];
            int end = x.index[old_word.Length + 3];
            for (int i = start; i < end; i++)
            {
                int d = string_algs.Levensthein(old_word, x.words[i]);
                //Console.WriteLine("Levenstein de " +word + " y " + b.wordds[i] + " es " + d);
                if (d <= edit_distance)
                {
                    edit_distance = d;
                    new_word = x.words[i];
                }
            }
            // replace in old_query all ocurrences if there are any of the old_word by the new word.
            /* when the search button is pressed a list of suggestions words will be created, the number of words is
            indicated by the number_of_suggestions, if the suggestion button is touched, we try to replace the suggest word
            by alguna correcta que esté en el corpus.
            */
            string new_query = string_algs.replace(new_word, old_word, actual_query);
            return new_query;
        }
        return actual_query;
    }
}
