/* 

*/
namespace qquery;
using corpuss;
using d_t_h;
public partial class query
{

    public Tuple<int, string, string, string>[] get_medals()
    {
        // we determine medals in the tf-idf category by comparing to the best result.
        // we determine medals in the op_cerc by comparing the items and also with respect
        // to the number of words that are related in the ~
        // we determine the medals in the category of min interval by comparing with respect to
        // the number of words that are in the words_to-request.

        // este número es la cantidad de palabras que hay en el mejor resultado respecto al scorer de cercanía.
        double cercania_max_words = Math.Floor(score_by_cercania.Max());
        Tuple<int, double>[] sum_of_scores = new Tuple<int, double>[score_by_tfidf.Length];
        // este número es la cantidad de palabras que hay en el mejor resultado respecto al scorer de min_interval .
        double min_interval_max_words = Math.Floor(score_by_min_interval.Max());
        int stop = Math.Min(10, score_by_cercania.Length);
        Tuple<int, string, string, string>[] medals = new Tuple<int, string, string, string>[stop];

        // now after every document has medals how determine between two documents who is better, sum
        // the scores of each medal, and there you go.
        for (int i = 0; i < this.score_by_cercania.Length; i++)
        {
            /////////////////////////////
            // ranking by tf-idf
            /////////////////////////////
            if (score_by_tfidf[i] > 0.85)
            {
                medallas[i][0] = 4;
                
            }
            else if (score_by_tfidf[i] > 0.7)
            {
                medallas[i][0] = 3;
            }
            else if (score_by_tfidf[i] > 0.5)
            {
                medallas[i][0] = 2;
            }

            /////////////////////////////
            // ranking by cercania.
            // A la hora de hacer esto recordar que mientras más palabras
            // tenga el documento y  mientras más pequeño sea el intervalo
            // mejor, that's it.
            // Las medallas en la cercaía y en el min_interval serán ligeramente
            // más valiosas que en el de tfidf. 
            /////////////////////////////

            if (score_by_cercania[i] > cercania_max_words && score_by_cercania[i] > 0.7*this.closest_words.Count )
            {
                medallas[i][1] = 6 + (score_by_cercania[i]- Math.Floor(score_by_cercania[i]));
            }
            else if (score_by_cercania[i] > cercania_max_words)
            {
                medallas[i][1] = 4 + (score_by_cercania[i]- Math.Floor(score_by_cercania[i]));
            }
            else if (score_by_cercania[i] > 0.5*cercania_max_words)
            {
                medallas[i][1] = 2+ (score_by_cercania[i]- Math.Floor(score_by_cercania[i]));
            }                        

            /////////////////////////////
            // ranking by min_interval.
            // A la hora de hacer esto recordar que mientras más palabras
            // tenga el documento y  mientras más pequeño sea el intervalo
            // mejor, that's it.
            // Las medallas en la cercaía y en el min_interval serán ligeramente
            // más valiosas que en el de tfidf. 
            /////////////////////////////

            if (score_by_min_interval[i] > min_interval_max_words && score_by_min_interval[i] > 0.7*this.words_to_request.Count )
            {
                medallas[i][2] = 6 + (score_by_min_interval[i]- Math.Floor(score_by_min_interval[i]));
            }
            else if (score_by_min_interval[i] > min_interval_max_words)
            {
                medallas[i][2] = 4 + (score_by_min_interval[i]- Math.Floor(score_by_min_interval[i]));
            }
            else if (score_by_min_interval[i] > 0.5*min_interval_max_words)
            {
                medallas[i][2] = 2+ (score_by_min_interval[i] - Math.Floor(score_by_min_interval[i]));
            }

            sum_of_scores[i] = new Tuple<int, double> (i, medallas[i][0]+ medallas[i][1]+medallas[i][2]);
        }
        //
        sort.quickSort(sum_of_scores, 0, sum_of_scores.Length-1);
        // remove the zero ones.
        
        Array.Reverse(sum_of_scores);
        for (int i = 0; i < stop; i++)
        {
            // get medals of sum_of_scores[i].Item1;
            string[] r = new string[3];
            if (medallas[sum_of_scores[i].Item1][0] >3)
            {
                r[0] = "🥇";
            }
            else if (medallas[sum_of_scores[i].Item1][0] >2)
            {
                r[0] = "🥈";
            }
            else if (medallas[sum_of_scores[i].Item1][0] >1)
            {
                r[0] = "🥉";
            }
            else
            {
                r[0] = " ";
            }


            if (medallas[sum_of_scores[i].Item1][1] >6)
            {
                r[1] = "🥇";
            }
            else if (medallas[sum_of_scores[i].Item1][1] >4)
            {
                r[1] = "🥈";
            }
            else if (medallas[sum_of_scores[i].Item1][1] >2)
            {
                r[1] = "🥉";
            }
            else
            {
                r[1] = " ";
            }

            if (medallas[sum_of_scores[i].Item1][2] >6)
            {
                r[2] = "🥇";
            }
            else if (medallas[sum_of_scores[i].Item1][2] >4)
            {
                r[2] = "🥈";
            }
            else if (medallas[sum_of_scores[i].Item1][2] >2)
            {
                r[2] = "🥉";
            }
            else
            {
                r[2] = " ";
            }
            medals[i] = new Tuple<int, string, string, string>(sum_of_scores[i].Item1, r[0], r[1], r[2]);
        }
        
    List<Tuple<int, string, string, string>> A = new List<Tuple<int, string, string, string>>();
    for (int i = 0; i < medals.Length; i++)
    {
        if (sum_of_scores[i].Item2 > 0)
        {
            A.Add(medals[i]);
        }
    }    
   
    return A.ToArray();
    }
}