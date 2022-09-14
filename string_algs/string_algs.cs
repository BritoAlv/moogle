namespace string_algss;
using System.Text.RegularExpressions;
public static class string_algs
{
    public static int Levensthein(string a, string b)
    {
        // trim common start and common end
        int start = 0;
        int a_end = a.Length;
        int b_end = b.Length;
        while ((start < a_end) && (start < b_end) && (a[start] == b[start]))
        {
            start++;
        }
        while ((start < a_end) && (start < b_end) && (a[a_end-1] == b[b_end-1]))
        {
            a_end--;
            b_end--;
        }        
        // end trim area

        a_end = a_end-start; // now a_end is a_length
        b_end = b_end-start;
        if(a_end == 0){return a_end;}
        if(b_end == 0){return b_end;}

        // now it should come
        a = a.Substring(start, a_end);
        b = b.Substring(start,b_end);
        // above code 

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
        return fila[b.Length-1];  
    }

    public static string replace( string replacement , string old, string input)
    {
        // replace every ocurrence of old by new in the replace string,
        string pattern = @"\b" + old + @"\b";
        string result = Regex.Replace(input, pattern, replacement);
        return result;   
    }

    

}