namespace tokenizer;
using docc;


public partial class token
{
    public void group_similar_words(Dictionary<string, info_word_doc> A)
    {
    Dictionary<string, string> B = new Dictionary<string, string>();
    foreach (var item in A)
    {
        B[item.Key] = item.Value.linked;
    }
    Dictionary<string, string[]> C = link_words(B);
    foreach (var item in C)
    {
        A[item.Key].similar = item.Value;
    }
    }
    
    public static Dictionary<string, string[]> link_words(Dictionary<string, string> A)
    {
        Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
        foreach (var item in A)
        {
            if (!result.ContainsKey(item.Value))
            {
                result[item.Value] = new List<string>();
            }
            result[item.Value].Add(item.Key);
        }
        Dictionary<string, string[]> resultt = new Dictionary<string, string[]>();
        foreach (var item in result)
        {
            resultt[item.Key] = item.Value.ToArray();
        }
        return resultt;
    }
}