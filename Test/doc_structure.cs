public class doc
{
    public double norm; // to be computed after the mix.
    public Dictionary<string, info> dic;
    public string name;
    public string text;

    public doc(string title, corpus x)
    {
        this.name = title; // title to send to snippet
        this.text = tokenization.read_txt(title); // text to be used by snippet
        this.dic = tokenization.words_in_document(text); // words in document each one with tf.
        x.update_corpus(this);
    }

    // esta maravillosa función ha de ser llamada después de haber creado todos los documentos.
    public void compute_tf_idf(corpus x)
    {
        foreach (var item in this.dic)
        {
            item.Value.weight = item.Value.weight * Math.Log10( ((double)(x.number_of_docs +1)) / ((double)x.idf[item.Key]));
        }
    }

    public void mix_keys(corpus x)
    {
        foreach (var listt in x.linked_list)
        {
            string[] lis = listt.Value;
            List<string> found_word = new List<string>();
            foreach (var word in lis)
            {
                if(this.dic.ContainsKey(word))
                {
                    found_word.Add(word);
                }
            }
            if (found_word.Count > 0)
            {
                string first_word = found_word[0];
                found_word.RemoveAt(0);
                string[] other_words = found_word.ToArray();
                double cant = ((double)(other_words.Length +1));
                double promedio = this.dic[first_word].weight / cant;
                List<pos> aa = new List<pos>();
                foreach (pos item in this.dic[first_word].places)
                {
                    aa.Add(item);
                }
                foreach (string word in other_words)
                {
                    promedio = promedio + this.dic[word].weight/ cant;
                    foreach (pos item in this.dic[word].places)
                    {
                        aa.Add(item);
                    }
                    this.dic[word] = new info(new List<pos>(), this.dic[word].weight);
                }
                this.dic[first_word] = new info(aa, promedio); 
            }
        }
    }
    public void set_norm()
    {
        this.norm = 0;
        foreach (var item in this.dic)
        {
            this.norm = this.norm + (item.Value.weight)*(item.Value.weight);
        }
        this.norm = Math.Sqrt(this.norm);
    }
    public bool contain_word(string word, corpus X)
    {
        if (this.dic.ContainsKey(word))
        {
            return true;
        }
        // word
        // X.word_linked[word]
        // X.linked_list[X.word_linked[word]]
        foreach (var item in X.linked_list[X.words_linked[word]])
        {
            if(this.dic.ContainsKey(item))
            {
                return true;
            }
        }
        return false;
    }
    public info get_info(string word, corpus X)
    {
        // this function was implemented assumin the word is in the document.
        
        foreach (var item in X.linked_list[X.words_linked[word]])
        {
            if(this.dic.ContainsKey(item))
            {
                return this.dic[item];
            }
        }
        return new info();
    }
}