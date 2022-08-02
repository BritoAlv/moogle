using System.Buffers;
public static class levensthein_implementations
{
    public static int levensthein_0 (string a, string b)
    {
        int[,] costMatrix = new int[a.Length+1, b.Length+1];
        for(int i = 0; i<= a.Length; i++)
        {
            costMatrix[i,0] = i;
        }
        for (int i = 0; i <= b.Length; i++)
        {
            costMatrix[0,i] = i;
        }
        for (int i = 1; i <= a.Length; i++)
        {
            for (int j = 1; j <= b.Length; j++)
            {
                int insertion = costMatrix[i,j- 1] + 1;
                int deletion = costMatrix[i-1,j] + 1;
                int substitution = costMatrix[i-1,j-1] + ((a[i-1] == b[j-1])?0:1);
                costMatrix[i,j] = Math.Min(Math.Min(insertion, deletion), substitution);
            }
        }
        return costMatrix[a.Length, b.Length];
    }

    public static int levensthein_1(string a, string b)
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
        return fila[b.Length-1];          
    }

    public static int levensthein_2(string a, string b)
    {
        // https://adamsitnik.com/Array-Pool/
        int[] fila = ArrayPool<int>.Shared.Rent(b.Length);
        // int[] fila = new int[b.Length];
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
        ArrayPool<int>.Shared.Return(fila);
        return result;          
    }
    public static int levensthein_3(string a, string b)
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

        // https://adamsitnik.com/Array-Pool/
        int[] fila = ArrayPool<int>.Shared.Rent(b.Length);
        // int[] fila = new int[b.Length];
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
        ArrayPool<int>.Shared.Return(fila);
        return result;          
    }    
    public static int levensthein_4(string a, string b)
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
        
        // now it should come avoid substring allocations
        ReadOnlySpan<char> a_span = a.AsSpan().Slice(start, a_end);
        ReadOnlySpan<char> b_span = b.AsSpan().Slice(start, b_end);
        // above code 

        // https://adamsitnik.com/Array-Pool/
        int[] fila = ArrayPool<int>.Shared.Rent(b_span.Length);
        // int[] fila = new int[b.Length];
        for (int i = 0; i < b_span.Length; i++)
        {
            fila[i] = i+1;    
        }
        for (int i = 0; i < a_span.Length; i++)
        {
            int last_substitution = i;
            int last_insertion = i+1;
            for (int j = 0; j < b_span.Length; j++)
            {
                int deletion = fila[j];
                int substitution = last_substitution + (a_span[i] == b_span[j] ? 0 : 1);
                last_insertion = Math.Min(Math.Min(last_insertion, deletion)+1, substitution);
                last_substitution = deletion;
                fila[j] = last_insertion;
            }
        }
        int result = fila[b_span.Length-1];
        ArrayPool<int>.Shared.Return(fila);
        return result;          
    }    
    public static int levensthein_5(string a, string b)
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
        
        // now it should come avoid substring allocations
        ReadOnlySpan<char> a_span = a.AsSpan().Slice(start, a_end);
        ReadOnlySpan<char> b_span = b.AsSpan().Slice(start, b_end);
        // above code 

        // https://adamsitnik.com/Array-Pool/
        int[] fila = ArrayPool<int>.Shared.Rent(b_span.Length);
        // int[] fila = new int[b.Length];
        for (int i = 0; i < b_span.Length; i++)
        {
            fila[i] = i+1;    
        }
        for (int i = 0; i < a_span.Length; i++)
        {
            int last_substitution = i;
            int last_insertion = i+1;
            for (int j = 0; j < b_span.Length; j++)
            {
                int cost = last_substitution;
                int deletion_cost = fila[j];
                if(a_span[i] != b_span[j])
                {
                    cost = Math.Min(last_insertion, cost);
                    cost = Math.Min(deletion_cost, cost);
                    cost++;
                }
                last_insertion = cost;
                fila[j] = cost;
                last_substitution = deletion_cost;
            }
        }
        int result = fila[b_span.Length-1];
        ArrayPool<int>.Shared.Return(fila);
        return result;          
    }    
public static int levensthein_6(string a, string b)
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
        
        // now it should come avoid substring allocations
        ReadOnlySpan<char> a_span = a.AsSpan().Slice(start, a_end);
        ReadOnlySpan<char> b_span = b.AsSpan().Slice(start, b_end);
        // above code 

        // https://adamsitnik.com/Array-Pool/
        int[] fila = ArrayPool<int>.Shared.Rent(b_span.Length);
        // int[] fila = new int[b.Length];
        for (int i = 0; i < b_span.Length; i++)
        {
            fila[i] = i+1;    
        }
        for (int i = 0; i < a_span.Length; i++)
        {
            int last_substitution = i;
            int last_insertion = i+1;
            char aChar = a_span[i];
            for (int j = 0; j < b_span.Length; j++)
            {
                int cost = last_substitution;
                int deletion_cost = fila[j];
                if(aChar != b_span[j])
                {
                    cost = Math.Min(last_insertion, cost);
                    cost = Math.Min(deletion_cost, cost);
                    cost++;
                }
                last_insertion = cost;
                fila[j] = cost;
                last_substitution = deletion_cost;
            }
        }
        int result = fila[b_span.Length-1];
        ArrayPool<int>.Shared.Return(fila);
        return result;          
    }    
}