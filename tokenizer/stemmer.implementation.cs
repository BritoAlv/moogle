public static class stemmer
{
    public static string[] spanish_endings = 
    {"ba","bas","ron","ste","mos","ndo","o", "a", "r", "as", "n", "ras", "ria", "me","los", "ia","nos", "i","es","rlos","rlas","ste", "do", "s", "ndolo", "ndo", "ndola", "ndome","ndonos", "ndolos","an", "ran", "ra", "rte","le", "te", "da", "dos", "la", "se", "d", "nlo", "rle","rnos", "rse"};
    public static char[] vocals = {'a','e','i','o','u','á','é','í','ó','ú'};
    /* 
    the stemmer will take the dictionary with all words in the document and will try put words with same family. How this will work, this will take an array of words, and will return a dict where key is the word and value is the word a la cual fue linked.
    Por ejemplo: ayudamos : ayudar
    */

    // beginning of a lot of auxiliar functions.

    public static string remove_tildes(string a)
    {
        return a.Replace('á','a').Replace('é','e').Replace('í','i').Replace('ó','o').Replace('ú','u');
    }
    public static void linkk_dict(List<string> words, Dictionary<string, string> thedict)
    {
        foreach (string word in words)
        {
            thedict[word] = words[0];
        }
    }
    public static List<string>  inflected(List<string> words, string lexem)
    {
        List<string> result = new List<string>();
        int lex = lexem.Length;
        int lenq = words.Count;
        for (int i = 0; i < words.Count; i++)
        {
            if(words[i].Length >= lex +2)
            {
                if( !vocals.Contains(words[i][lex]) & !vocals.Contains(words[i][lex+1]))
                {
                    result.Add(words[i]);
                    words.RemoveAt(i);
                }
            }
        }
        lenq = words.Count;
        for (int i = 0; i < lenq; i++)
        {
            foreach (string ending in spanish_endings)
            {
                if( (remove_tildes(words[i]) == lexem + ending) | remove_tildes(words[i]) == lexem)
                {
                    result.Add(words[i]);
                    break;
                }  
            }
        }
        return result;
    }
    public static string find_plural(string word)
    {
        foreach (char vocal in vocals)
        {
            if(word.EndsWith(vocal))
            {
                return word+"s";
            }
        }    
        return word+"es";   
    }

    public static string common_root(List<string> words)
    {
        
        int min = words[0].Length;
        for (int i = 0; i < words.Count; i++)
        {
            int temp = words[i].Length;
            if (temp < min)
            {
                min = temp;
            }
        }
        string common_start = "";
        char check;
        for (int i = 0; i < min; i++)
        {
            check = remove_tildes(words[0].ToLower()[i].ToString()).ToCharArray()[0];
            bool find = false;
            foreach (string word in words)
            {
                if ( ! ( check == remove_tildes(word[i].ToString().ToLower()).ToCharArray()[0]))
                {
                    find = true;
                    break;
                } 
            }
            if (find)
            {
                break;
            }
            else
            {
                common_start = common_start + check;
            }
            
        }
        return common_start;
    }

    public static string lexem_by_last_consonant(string word)
    {
        for (int i = word.Length-1; i >=0; i--)
        {
            if (!vocals.Contains(word[i]))
            {
                return remove_tildes(word.Substring(0,i+1));
            }
        }
        return " ";
    }
    public static List<string> closest_words(int index, List<string> words)
    {
        List<string> closest = new List<string>();
        closest.Add(words[index]);
        for (int i = 1; i < 21; i++)
        {
            if(index-i >=0)
            {
                closest.Add(words[index-i]);
            }
            if(index+i <= words.Count-1)
            {
                closest.Add(words[index+i]);
            }
        }
        return closest;
    }    
    public static bool inside_rectangle(string root, string word)
    {
        if(remove_tildes(root) == remove_tildes(word))
        {
            return true;
        }
        string a = common_root(new List<string>(){root,word});
        int len = a.Length;
        if (len>=4  & root.Length - len <=3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // ending of a lot of auxiliar functions.
    
    public static Dictionary<string, string> stem(string[] words)
    {

        // first step is sort the array
        Array.Sort(words);
        List<string> b = words.ToList();
        Dictionary<string, string> result = new Dictionary<string, string>();
        // create a list with weird words, this will remain in the same place.
        List<string> weird_words = new List<string>();
        int len = b.Count;
        for (int i = 0; i < len; i++)
        {
            if(i < len)
            {
                if(b[i].EndsWith('l')) // words ended with l are a mess
                {
                    // singular
                    weird_words.Add(b[i]);
                    b.RemoveAt(i);
                    len = len - 1;

                    // plural
                    string plural = find_plural(weird_words[weird_words.Count -1 ]);
                    if(b.Contains(plural))
                    {
                        weird_words.Add(plural);
                        b.Remove(plural);
                        len = len-1;
                    }
                }
                else
                {
                    string root = b[i]; // words will be linked to here.
                    List<string> clase = new List<string>() {root};
                    if (b.Contains(root+"mente"))
                    {
                        string lexem = lexem_by_last_consonant(root);
                        List<string> closest = closest_words(i, b);
                        foreach (string word in closest)
                        {
                            if((lexem_by_last_consonant(word) == lexem) | word == lexem.Substring(0, lexem.Length-1))
                            {
                                clase.Add(word);
                            }    
                        }
                        clase.Add(root+"mente");
                        // we have all related words in a clase, so let's ut them in the dict.
                        linkk_dict(clase, result);
                        foreach (string word in clase)
                        {
                            b.Remove(word);
                            len = len-1;
                        }
                    }
                    else
                    {
                        List<string> closest = closest_words(i,b);
                        List<string> rectangle_words = new List<string>();
                        foreach (string word in closest)
                        {
                            if(inside_rectangle(root, word))
                            {
                                rectangle_words.Add(word);
                            }
                        }
                        string lexem = common_root(rectangle_words);
                        bool bo = false;
                        int lens = lexem.Length;
                        if (!vocals.Contains(lexem[lens-1]))
                        {
                            bo = true;
                            
                            foreach (string word in rectangle_words)
                            {
                                if ( !( word.Length == lens)  && !(vocals.Contains(word[lens])))
                                {
                                    bo = false;
                                    break;    
                                }   
                            }
                        }
                        else
                        {
                            rectangle_words = inflected(rectangle_words, lexem);
                            bo = (rectangle_words.Count >0);
                        }
                        if (bo)
                        {
                            linkk_dict(rectangle_words, result);
                            foreach (string word in rectangle_words)
                            {
                                b.Remove(word);
                                len = len -1;
                            }
                        }
                    }
                }        
            }
        }
        foreach (string word in weird_words)
        {
            b.Add(word);
        }
        while(b.Count > 0)
        {
            string root = b[0];
            List<string> clase = new List<string>() {root};
            if(b.Contains(find_plural(root)))
            {
                clase.Add(find_plural(root));
            }
            linkk_dict(clase, result);
            foreach(string word in clase)
            {
                b.Remove(word);
            }
        }
        return result;
    }
}