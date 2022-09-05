namespace corpuss;
using docc;
using tokenizer;
using d_t_h;

public partial class corpus
{
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
        if(this.bd[word].docs.ContainsKey(id))
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
        if(this.bd[word].docs.ContainsKey(id))
        {
        return this.bd[word].docs[id].pos;
        }
        return new List<int>();
    }
}