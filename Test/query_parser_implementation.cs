public static class parser
{
    public static string[] ignore(Dictionary<string, double> A, corpus B)
    {
        List<string> a = new List<string>();
        foreach (var item in A)
        {
            if(B.idf[item.Key] > 9*B.number_of_docs/10)
            {
                a.Add(item.Key);
            }
        }
        return a.ToArray();
    }
    public static int first_pos(char a, string b, int start = 0, int direction = 1)
    {
        for (int i = start; i < b.Length; i = i + direction)
        {
            if(b[i] == a)
            {
                return i;
            }
        }
        return -1;
    }
    public static Dictionary<string, double> update_idf(Dictionary<string, double> A, corpus X)
    {
        int len = A.Keys.Count;
        foreach (var item in A)
        {
            double idf = 0;
            if(X.idf.ContainsKey(item.Key))
            {
                idf = X.idf[item.Key];
            }
            A[item.Key] = A[item.Key] / ((double)len) * Math.Log10( ((double)(X.number_of_docs + 1)) / (1 + idf)  );
        }
        return A;
    }
    public static Dictionary<string, double> query_words(string query)
    {
        Dictionary<string, double> A = new Dictionary<string, double>();
        for (int i = 0; i < query.Length; i++)
        {
            // reach an index that is alphanumeric
            if(char.IsLetterOrDigit(query[i]))
            {
                int start = i;
                string word = (query[i]).ToString();
                // move i to where the word ends
                i = i+1;
                while(i < query.Length && char.IsLetterOrDigit(query[i]))
                {
                    word = word + query[i];
                    i = i+1;
                }
                // i-1 is where the word ends
                // put the word in the dict.

                // if no la contiene
                if(!A.ContainsKey(word))
                {
                    A[word] = 0;
                }


                // en este punto ya la contiene
                A[word] += 1;
                // word starts at start, and end at i-1.
                

            }
        }
        return A;
    }
    public static string[] forbiddenwords(string a)
    {
        List<string> result = new List<string>();        
        int temp = parser.first_pos('!', a);
        while(temp >= 0 && temp+1 < a.Length)
        {
            
            int i = temp+1;
            string word = "";
            // word starts at i
            while(i < a.Length && char.IsLetterOrDigit(a[i]))
            {
            
                word = word + a[i];
                i = i+1;
                
            }
            result.Add(word);
            temp = parser.first_pos('!', a, i-1);
        }
        return result.ToArray();
    }
    public static Dictionary<string, double> important_words(string a, Dictionary<string, double> A )
    {
        
        int temp = parser.first_pos('*', a);
        while( temp >= 0 && temp+1 < a.Length)
        {
            
            int count = 1;
            int i = temp+1;
            while(a[i] == '*')
            {
                count++;
                i++;
            }
            string word = "";
            // word starts at i
            while(i < a.Length &&(char.IsLetterOrDigit(a[i]) || a[i] == '^' || a[i] == '¿'))
            {
                if(a[i] == '^' || a[i] == '¿')
                {
                    i = i+1;
                }
                else
                {
                word = word + a[i];
                i = i+1;
                }
            }
            A[word] = A.Values.Max()*(count);
            temp = parser.first_pos('*', a, i-1);
        }
        return A;
    }    
    public static string[] onlywords(string a)
    {
        List<string> result = new List<string>();        
        int temp = parser.first_pos('^', a);
        while(temp >= 0 && temp+1 < a.Length)
        {
            
            int i = temp+1;
            string word = "";
            // word starts at i
            while(i < a.Length &&(char.IsLetterOrDigit(a[i])  || a[i] == '*' || a[i] == '¿'))
            {
                if(a[i] == '*' || a[i] == '*' || a[i] == '¿')
                {
                    i = i+1;
                }
                else
                {
                word = word + a[i];
                i = i+1;
                }
            }
            result.Add(word);
            temp = parser.first_pos('^', a, i-1);
        }
        return result.ToArray();
    }

    public static string[] explicitt(string a)
    {
        List<string> result = new List<string>();        
        int temp = parser.first_pos('¿', a);
        while(temp >= 0 && temp+1 < a.Length)
        {
            
            int i = temp+1;
            string word = "";
            // word starts at i
            while(i < a.Length &&(char.IsLetterOrDigit(a[i]) || a[i] == '^' || a[i] == '*'))
            {
                if(a[i] == '^' || a[i] == '*' || a[i] == '*' )
                {
                    i = i+1;
                }
                else
                {
                word = word + a[i];
                i = i+1;
                }
            }
            result.Add(word);
            temp = parser.first_pos('¿', a, i-1);
        }
        return result.ToArray();
    }
    public static List<string[]> close_words(string a)
    {
        List<string[]> result = new List<string[]>();        
        int temp = parser.first_pos('~', a);
        while(temp >= 0 && temp+1 < a.Length)
        {
            int i = temp+1; // to move forward
            string cl = " ";
            int j = temp-1;
            while(j >=0 && char.IsLetterOrDigit(a[j]))
            {
                cl = a[j] + cl;
                j--;
            }
            while(i <a.Length && ( char.IsLetterOrDigit(a[i]) || a[i] == '~' ))
            {
                cl = cl + a[i];
                i++;
            }
            result.Add(cl.Replace("~", " ").Split(" ", StringSplitOptions.RemoveEmptyEntries));
            temp = parser.first_pos('~', a, i-1);
            
        }
        return result;
    }
}    
