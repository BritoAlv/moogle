namespace string_algss;
using System.Text.RegularExpressions;
public static class string_algs
{
    public static int Levensthein(string a, string b)
    {
        int[] fila = new int[b.Length];
        for (int i = 0; i < b.Length; i++)
        {
            fila[i] = i+1;    
        }
        for (int i = 0; i < a.Length; i++)
        {
            int last_substitution = i;
            int last_insertion = i+1;
            for (int j = 0; j < b.Length; j++)
            {
                int deletion = fila[j];
                int substitution = last_substitution + (a[i]==b[j] ? 0 : 1);
                last_insertion = Math.Min(Math.Min(last_insertion, deletion)+1,substitution);
                last_substitution = deletion;
                fila[j] = last_insertion;
            }
        }
        int result = fila[b.Length-1];
        return result;   
    }

    public static string replace( string replacement , string old, string input)
    {
        // replace every ocurrence of old by new in the replace string,
        string pattern = @"\b" + old + @"\b";
        string result = Regex.Replace(input, pattern, replacement);
        return result;   
    }

    

}