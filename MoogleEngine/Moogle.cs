namespace MoogleEngine;
using System.Diagnostics;
// with corpus. access to the classes of the Library Class corpus
using corpuss;
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
        this.time = ((double)(a.ElapsedMilliseconds)/1000).ToString() + "s";
        List<SearchItem> items = new List<SearchItem>();
        foreach (var doc in this.the_query.best_docs)
        {
            items.Add(new SearchItem(x.the_docs[doc.Item1].name + " ---> " + doc.Item2+doc.Item3+doc.Item4, this.the_query.the_snippets[doc.Item1].generate_snippet(x)));
        }
        return new SearchResult(items.ToArray(), la_query);
    }
}
