/* 
*/
namespace qquery;
using corpuss;
using d_t_h;
public partial class query
{
    public void by_min_in(int doc_index)
    {
        /*
        now apply the algorithm to result to get the score of this document based on min interval, 
        score will we # of words in the document from the query + 3/ length of min_interval that contains
        all words in the query.
        */
        List<id_element<int>> result = idk.merge_k_list(this.relevant_info); // this may be empty.
        if (result.Count == 0)
        {
            this.score_by_min_interval[doc_index] = 0;
        }
        else
        {   
            // start of this else clausula.
            double score_by_min_in = (double)2000;
            int start_min = result[0].val;
            int end_min = result[result.Count-1].val;
            Dictionary<int, int> count = new Dictionary<int, int>();
            foreach (var item in result)
            {
                count[item.id] = 0;            
            }
            int i = 0;
            int cc = count.Keys.Count;
            if (cc > 1)
            {
                int dint = 0; // distinct integers found
                for (int j = 0; j < result.Count; j++) // for every j from 0 to n-1, find the menor intervalo that ends at j that contains all distinct numbers.
                {
                    // notice that this code doesn't change j
                    // move until we found a j such that [0,j] works.
                    if (count[result[j].id] == 0)
                    {
                        dint++;
                    }
                    count[result[j].id]++;
                    if(dint == cc) // aument i so that get the min intervalo ending at such j.
                    {
                        while (count[result[i].id] > 1)
                        {
                            count[result[i].id]--;
                            i++;   
                        }
                        // if found interval is better than the old one keep it.
                        if ( (result[j].val-result[i].val) < score_by_min_in)
                        {
                            start_min = result[i].val;
                            end_min = result[j].val;
                            score_by_min_in = end_min - start_min;
                    
                        }
                    }      
                }
                this.score_by_min_interval[doc_index] = cc + ((double)2) / (double)( score_by_min_in);
            }
            else
            {
                score_by_min_interval[doc_index] = 1; 
            }
        } 
    }
}