using corpuss;
using Constants;
using qquery;
using System.Text.RegularExpressions;
public class snippet
{
    public int extra_length;
    public Dictionary<int,  bool> already_words; // id vs words is in the snippet
    public List<Tuple<int, int>> positions; // each tuple represent a diferent portion of snippet
    public int index_document; // the index of the document
    public Dictionary<int, int> respuesto;

    public snippet(int a, constant cons)
    {
        this.index_document = a;
        this.already_words = new Dictionary<int, bool>();
        this.positions = new List<Tuple<int, int>>();
        this.respuesto = new Dictionary<int, int>();
        this.extra_length = ((int)cons.constants["extra_length_snippet"]);
    }

    public void add_(Tuple<int, int> A, List<int> ids)
    {
        this.positions.Add(A);
        foreach (var item in ids)
        {
            already_words[item] = true;
        }
    }

    public string generate_snippet(corpus x, query c)
    {
        string snippet = "";

        string pedazo_texto(Tuple<int, int> A)
        {
            int start = A.Item1-extra_length;
            while (start >= 0 && char.IsLetterOrDigit(x.the_docs[this.index_document].text[start]))
            {
                start--;
            }
            start = Math.Max(0, start);
            int end = A.Item2+extra_length;
            while (end <= x.the_docs[this.index_document].text.Length-1 && char.IsLetterOrDigit(x.the_docs[this.index_document].text[end]))
            {
                end++;
            }
            end = Math.Min(x.the_docs[this.index_document].text.Length-1, end);
            return x.the_docs[this.index_document].text.Substring(start, end-start);            
        }

        void fill_snippet()
        {
            foreach (var item in respuesto)
            {
                if (!(already_words.ContainsKey(item.Key)))
                {
                    positions.Add(new Tuple<int, int>(item.Value-30, item.Value+30));
                    already_words[item.Key] = true;
                }
            }
        }
        fill_snippet();
        // remove repeated elements in a list
        List<Tuple<int, int>> pos_to_snippet = positions.GroupBy(z => z.Item1).Select(z => z.Last()).ToList();

        bool not_inside_other_tuple(Tuple<int, int> a, int index)
        {
            // if in the list there are two equal tuples this will not work, 
            for (int i = 0; i < pos_to_snippet.Count; i++)
            {
                if ( (!(i == index)) && ( a.Item1 >= pos_to_snippet[i].Item1    ) && (a.Item2 <= pos_to_snippet[i].Item2))
                {
                    return false;
                }
            }
            return true;

        }


        // remove the ones that are inside of other.
        pos_to_snippet = pos_to_snippet.Where( (a, index) => not_inside_other_tuple(a, index ) ).ToList();      
        foreach (var item in pos_to_snippet)
        {
            snippet = snippet + "<br>" + pedazo_texto(item) + "</br>";        
        }

        // this triple loop makes results slower to show but they get colored.
        foreach (var word in c.words_to_request)
        {
            
            IEnumerable<string> similares = x.bd[x.bd[word].linked].similar;
            foreach (var similar in similares)
            {
                int id = (c.words[word] <= 9)?c.words[word]:10;
                string Similar = similar[0].ToString().ToUpper()+similar.Substring(1);
                string pattern1 = @"\b" + similar + @"\b"; 
                string pattern2 = @"\b" + Similar + @"\b";
                string reemplazo1 = "<mark style = \"background:" + x.colors[id] + "\"> " + similar + " </mark>";
                string reemplazo2 = "<mark style = \"background:" + x.colors[id] + "\"> " + Similar + " </mark>";
                snippet = Regex.Replace(snippet, pattern1, reemplazo1 );
                snippet = Regex.Replace(snippet, pattern2, reemplazo2 );
            }
            
        }

        return snippet;
    }

}