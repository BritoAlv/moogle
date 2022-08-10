public static class tokenization
{
    //function to read a .txt
    public static string read_txt(string name)
    {
        string text = System.IO.File.ReadAllText("./"+name+".txt");
        return text;
    }
}