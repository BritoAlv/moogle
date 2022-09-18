namespace tokenizer;
using docc;
using System.Text.Json;

public partial class token
{
    public void stem(Dictionary<string, info_word_doc> A, bool use_cache = false)
    {
        Dictionary<string , string> linked = new Dictionary<string, string>();
        if (use_cache)
        {
            linked = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText("../cache/similar_words.json"));
        }
        else
        {
            linked = stemmer.stem(A.Keys.ToArray());
            string jsonString1 = JsonSerializer.Serialize(linked, new JsonSerializerOptions{WriteIndented = true});
            File.WriteAllText("../cache/similar_words.json", jsonString1);
        }

        foreach (var item in linked)
        {
            A[item.Key].linked = item.Value;
        }
    }
}