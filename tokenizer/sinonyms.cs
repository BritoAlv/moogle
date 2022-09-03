namespace tokenizer;
using docc;


public partial class token
{
    public void sinonyms(Dictionary<string, info_word_doc> A)
    {
        Dictionary<string, string> B = new Dictionary<string, string>();
        foreach (var item in A)
        {
            B[item.Key] = item.Value.linked;
        }
        syn.work(B);
        foreach (var item in B)
        {
            A[item.Key].linked = item.Value;
        }        
    }
}

