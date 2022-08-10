public static class tokenization
{
    // function to read a .txt
    public static string read_txt(string name)
    {
        string text = System.IO.File.ReadAllText("./"+name+".txt");
        return text;
    }
    
    public static Dictionary<string, info>  words_in_document(string text)
    {
        Dictionary<string, info > document_info = new Dictionary<string, info>();
        for (int i = 0; i < text.Length; i++)
        {
            // reach an index that is alphanumeric
            if(char.IsLetterOrDigit(text[i]))
            {
                int start = i;
                string word = (text[i]).ToString();
                // move i to where the word ends
                i = i+1;
                while(char.IsLetterOrDigit(text[i]))
                {
                    word = word + text[i];
                    i = i+1;
                }
                // i-1 is where the word ends
                // put the word in the dict.

                // if no la contiene
                if(!document_info.ContainsKey(word))
                {
                    document_info[word] = new info();
                }


                // en este punto ya la contiene
                document_info[word].term_frequency += 1;
                document_info[word].positions.Append(new Tuple<int, int>(start,i-1));   
            }
        }
        return document_info;    
    }
}