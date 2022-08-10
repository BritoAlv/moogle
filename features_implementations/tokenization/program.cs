class program
{
    public static void Main()
    {
        string text = tokenization.read_txt("Principito");
        Dictionary<string, info> A = tokenization.words_in_document(text);
        foreach (var key in A.Keys)
        {
            Console.WriteLine(key + " " + A[key].term_frequency);
        }
    }
}