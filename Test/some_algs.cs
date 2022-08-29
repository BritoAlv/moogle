public static class alg
{
    // given a dict< string, string> A, create a dict<string, string[]> B where the keys in B are the values in A and the values of B are all the keys in A that are asociated to that string in A.
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

    public static Dictionary<string, int> sort_dict_length(Dictionary<string, int> A)
    {
        Dictionary<string, int> result = new Dictionary<string, int>();
        foreach(var key in A.Keys.OrderBy(key => key.Length))
        {
            result[key] = A[key];
        }
        return result;
    }
}
