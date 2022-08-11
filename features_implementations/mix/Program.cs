public class Program
{
    public static void Main()
    {
        // first step is create a corpus X.
        corpus X = new corpus();
        document a = new document("Principito", X);
        a.update_idf(X);
    }
}