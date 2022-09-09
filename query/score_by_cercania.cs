namespace qquery;
using corpuss;
using d_t_h;
public partial class query
{
    public void by_cercania(int doc_index)
    {
        /////////////////////////////////////////////////////////////////////////////////
        /*
        este score buscará en todos los intervalos de tamaño fixed_length la cantidad de palabras
        del ~ y también tendra en cuenta el menor intervalo donde aparecen.
        */
        ////////////////////////////////////////////////////////////////////////////////
        List<double> mini_results = new List<double>(); // store length of each minimal interval.
        double sum_over_each_operator = 0;
        int fixed_length = 500;
        foreach (var close_words in this.op_cerc) // close_words array of words 
        {
            // get a score for each set of words related by ~
            Dictionary<int, List<id_element<int>>> xx = new Dictionary<int, List<id_element<int>>>();
            foreach (var word in close_words)
            {
                xx[words[word]] = this.relevant_info[words[word]].Select(x => x).ToList();   
            }
            List<id_element<int>> yep = idk.merge_k_list(xx);
            Dictionary<int, int> counts = new Dictionary<int, int>();
            // if there are menos de 2 ids in yep set result for this operator to 0. and no nothing.
            foreach (var item in yep)
            {
                counts[item.id] = 0;
            }
            if (counts.Count > 1)
            {
                // loop through all windows of size fixed_length and take the one that contains the mayor
                // cantidad de elementos y de esa window añadir a miniresults el menor intervalo que 
                // contiene dichos elementos.
                
                /////////////////////////////////////////////////////////////////////////////////
                // an algorithm that will loop through all the windows of size fixed_length and take
                // the best, puede ser el caso en que no haya ninguna, o whatever.
                ////////////////////////////////////////////////////////////////////////////////
                sum_over_each_operator = sum_over_each_operator + looper(yep,mini_results, fixed_length, the_snippets[doc_index]);
            }
        }
        double min_interval = (mini_results.Count>0)?mini_results.Min():0;
        this.score_by_cercania[doc_index] = sum_over_each_operator + min_interval;
    }

    public static double looper(List<id_element<int>> yep,  List<double> min_intervals, int fixed_length, snippet A)
    {

        // define first window because the length of the window may be greater than the length of the last
        // positions the algorithm take as base the first window and after it moves to the next window
        //  and update the count object if this windows contains more words we update window to the result.
        // once we have the best window we need to minimizarla hallando el menor intervalo que contiene
        // sus palabras, este menor intervalo se lo añadimos a la lista min_interval.

        // get first window, yep contains at east two elements.
        int answer = 0; // final results
        int st = 0; // final results
        int et = yep.Count-1; // final results
        int interval_length = 100000; // for comparing two windows with the same number of words
        for (int i = 0; i < yep.Count; i++)
        {
            int index_start = i;
            int index_end = i;
            int current_distinct = 0;
            Dictionary<int, int> counts = new Dictionary<int, int>();

            int start = Math.Max(0, yep[i].val-fixed_length/2);
            int end = Math.Min(yep[yep.Count-1].val ,yep[i].val+fixed_length/2);
            for (int r = i; r >= 0; r--)
            {
                if (yep[r].val < start)
                {
                    break;
                }
                else
                {
                   if (!counts.ContainsKey(yep[r].id) || counts[yep[r].id] == 0)
                   {
                        current_distinct++;
                   }
                   counts[yep[r].id] = 1;
                   index_start = r;
                }
            }

            for (int r = i+1; r < yep.Count; r++)
            {
                if (yep[r].val > end)
                {
                    break;
                }
                else
                {
                   if (!counts.ContainsKey(yep[r].id) || counts[yep[r].id] == 0)
                   {
                        current_distinct++;
                   }
                   counts[yep[r].id] = 1; 
                   index_end = r;
                }
            }
            if (current_distinct == answer &&  ((yep[index_end].val - yep[index_start].val) < interval_length))
            {
                st = index_start;
                et = index_end;
                interval_length = (yep[index_end].val - yep[index_start].val);
            }
            else if (current_distinct > answer)
            {
                answer = current_distinct;
                st = index_start;
                et = index_end;
                interval_length = (yep[index_end].val - yep[index_start].val);
            }            
        }
        if (answer > 1)
        {
            // I have at st,et the minimal window that satisfies
            // the condition only is left to minimize it and add
            // its score to the list of minimal intervals.

            ////////////////////////////////////////////////////////////////////////
            // Steps to do that:
            //  
            ////////////////////////////////////////////////////////////////////////
            int start_min = yep[st].val;
            int end_min = yep[et].val;
            int score_by_min_in = yep[et].val-yep[st].val;
            Dictionary<int, int> count  = new Dictionary<int, int>();
            for (int e = st; e <= et; e++)
            {
                count[yep[e].id] = 0;
            }
            int em = st;
            int cc = count.Keys.Count;
            int dint = 0;
            for (int j = st; j <= et ; j++)
            {
                if(count[yep[j].id] == 0)
                {
                    dint++;
                }
                count[yep[j].id]++;
                if(dint == cc)
                {
                        while (count[yep[em].id] > 1)
                        {
                            count[yep[em].id]--;
                            em++;   
                        }
                        // if found interval is better than the old one keep it.
                        if ( (yep[j].val-yep[em].val) < score_by_min_in)
                        {
                            start_min = yep[em].val;
                            end_min = yep[j].val;
                            score_by_min_in = end_min - start_min;
                        }
                }
            }
            A.Add(new Tuple<int, int>(start_min, end_min), count.Keys.ToList());
            if (score_by_min_in > 0)
            {
                min_intervals.Add((double)(1)/(double)score_by_min_in);
            }
        return answer;    
        }
        return 0;






























        
    }        
}    