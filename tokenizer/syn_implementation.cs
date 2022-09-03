/* 
Los sinonimos buscar las keys que sean sinonimmos, digamos que son 3, entonces tomar uno de sus values, digamos que es T, entonces hacer que todas las keys en el dict que apunten a uno de esos valores apunten a T.
 */
public static class syn
{
    public static void work(Dictionary<string, string> A)
    {
        List<string[]> sin = obtain_sin();
        foreach (string[] ss in sin)
        {
            // ss is a string[] of words that are synonimus, finds its values in A is the first thing.

            List<string> values = new List<string>();
            foreach (string sinonimo in ss)
            {
                if(A.ContainsKey(sinonimo))
                {
                    values.Add(A[sinonimo]);
                }            
            }
            if (values.Count > 0)
            {
                string value = values[0];
                // now we have to make that all the keys in the dict that points to one of those values point to value.
                foreach (var item in A)
                {
                    if(values.Contains(item.Value))
                    {
                        A[item.Key] = value;
                    }
                }

            }
        }

               
    }
    public static List<string[]> obtain_sin()
    {
        List<string[]> sin = new List<string[]>();
        string[] a = System.IO.File.ReadAllLines("../cache/sin.txt");
        foreach (string text in a)
        {
            List<string> temp = new List<string>();
            for (int i = 0; i < text.Length; i++)
            {
                // reach an index that is alphanumeric
                if(char.IsLetterOrDigit(text[i]))
                {
                    int start = i;
                    string word = (text[i]).ToString();
                    // move i to where the word ends
                    i = i+1;
                    while(i < text.Length && char.IsLetterOrDigit(text[i]))
                    {
                        word = word + text[i];
                        i = i+1;
                    }
                    // i-1 is where the word ends
                    // put the word in the dict.
                    temp.Add(word);
                }
            }
            sin.Add(temp.ToArray());    
        }
        return sin; 
    }
}    
        
