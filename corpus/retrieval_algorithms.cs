namespace corpuss;
using docc;
using tokenizer;
using d_t_h;

public partial class corpus
{
    public bool word_is_popular(string word)
    {
        // decide if the world is popular in the docs. this function is expected to return true. design to ignore stopwords.
        int c = 4*this.cant_docs/5;
        int total = 0;
        foreach (var doc in  this.bd[word].docs)
        {
            if (doc.Value.pos.Count > 100)
            {
                total++;
                if(total > c)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public double request_word_idf(string word, bool allow_similar)
    {
        if (allow_similar)
        {
            double total = 0;
            int cont = 0;
            foreach (var item in this.bd[this.bd[word].linked].similar)
            {
                total = total + this.bd[item].idf;
                cont++;                    
            }
            return total/(double)(cont+0.00000001);
        }    
        return this.bd[word].idf;
        
    }
    public bool word_in_corpus(string word)
    {
        if (this.bd.ContainsKey(word))
        {
            return true;
        }
        return false;
    }
    public double request_word_weight(string word, int id, bool allow_similar = false)
    {
        if (!this.word_in_corpus(word)) 
        {
            return 0;
        }
        // before call this function check if word exists in the corpus.
        // search in the doc[id] the weight of this word and return it. 
        // if the word is not found return 0.
        if (allow_similar)
        {
            double total = 0;
            int cont = 0;
            foreach (var item in this.bd[this.bd[word].linked].similar)
            {
                if (this.bd[item].docs.ContainsKey(id))
                {
                total = total + this.bd[item].docs[id].weight;
                cont++;                    
                }
            }
            total = total / (double)(cont+0.00000001);
            if(this.bd[word].docs.ContainsKey(id))
            {
            return Math.Max(total, this.bd[word].docs[id].weight);
            }
            return total;
        }
        else if(this.bd[word].docs.ContainsKey(id))
        {
            return this.bd[word].docs[id].weight;
        }
        return 0;
    }

    public List<int> request_word_positions(string word, int id, bool allow_similar = false)
    {
        // before call this function check if word exists in the corpus.
        // search in the doc[id] the positions where appears the word and return it.
        //if the word is not found return an empty list.
        if (!this.word_in_corpus(word)) 
        {
            return new List<int>();
        }
        if (allow_similar)
        {
            List<List<int>> a = new List<List<int>>();
            foreach (var item in this.bd[this.bd[word].linked].similar)
            {
                if (this.bd[item].docs.ContainsKey(id))
                {
                    a.Add(this.bd[item].docs[id].pos);
                }
                
            }
            return idk.merge_k_list(a.ToArray());
        }
        else if(this.bd[word].docs.ContainsKey(id))
        {
        return this.bd[word].docs[id].pos;
        }
        return new List<int>();
    }
}