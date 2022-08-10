public class Program{
    public static void Main()
    {
        string[] words = System.IO.File.ReadAllLines("./words.txt");
        Dictionary<string, string> a = stemmer.stem(words);
        foreach(KeyValuePair<string, string> m in a)
        {
            Console.WriteLine(m.Key + " " + m.Value);
        }
    }
}