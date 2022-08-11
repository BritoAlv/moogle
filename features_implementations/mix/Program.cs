public class Program
{
    public static void Main()
    {
        corpus X = new corpus();
	    var txtFiles = Directory.EnumerateFiles("./", "*.txt").ToArray();
        document[] rr = new document[txtFiles.Length];
        for (int i = 0; i < rr.Length; i++)
        {
            rr[i] = new document(txtFiles[i].Substring(0, txtFiles[i].Length-4 ), X);
        }
        for (int i = 0; i < rr.Length; i++)
        {
            rr[i].update_tf_idf(X);
        }
            
    }
}
