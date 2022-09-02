namespace tokenizer;
using docc;


public partial class token
{
    public void update_tf_idf(Dictionary<string, info_word_doc> A, int cant_docs)
    {
        foreach (var word in A)
        {
            foreach (var key_word_in_doc in word.Value.docs)
            {
                key_word_in_doc.Value.weight = key_word_in_doc.Value.weight * ((double)word.Value.idf) / ((double)cant_docs);
            } 
        }
    }
}    