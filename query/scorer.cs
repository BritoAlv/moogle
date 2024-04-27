/* 
The part of the query that will calculate scorers.
*/
namespace qquery;
using corpuss;
using d_t_h;
using Constants;
public partial class query
{
    public void scorers(corpus x, List<int> index_of_docs_to_consider, Constant cons)
    {
        foreach (var doc_index in index_of_docs_to_consider)
        {
            // score by tf-idf.
            double tfidf_score = 0 ;
            int cont = 0;
            foreach (var word in this.words_to_request)
            {
                this.relevant_info[words[word]] = x.request_word_positions(word, doc_index, this.similar_words.Contains(word)).Select(x => new id_element<int>( words[word] , x)).ToList();
                tfidf_score = tfidf_score + this.tfidf[word]*x.request_word_weight(word, doc_index, similar_words.Contains(word));
                cont++;
            }
            tfidf_score = (tfidf_score) / (x.the_docs[doc_index].norm * this.norm);
            this.score_by_tfidf[doc_index] = tfidf_score;

            //////////////////////////////////////////////////////////////////////////////////////////////////
            // start of scoring by cercania
            //////////////////////////////////////////////////////////////////////////////////////////////////
            by_cercania(doc_index, cons);
            //////////////////////////////////////////////////////////////////////////////////////////////////
            // end of scoring by cercania
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////////////
            // start of scoring by min interval
            //////////////////////////////////////////////////////////////////////////////////////////////////
            by_min_in(doc_index, cons);
            //////////////////////////////////////////////////////////////////////////////////////////////////
            // end of scoring by min interval
            //////////////////////////////////////////////////////////////////////////////////////////////////
        }
        double max = (score_by_tfidf.Length > 0)?(score_by_tfidf.Max()):0;
        if (max == 0){max = 1;}
        foreach (var doc_index in index_of_docs_to_consider)
        {
            this.score_by_tfidf[doc_index] = this.score_by_tfidf[doc_index] / max;
        }        
    }
}