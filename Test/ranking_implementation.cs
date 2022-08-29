public static class ranking
{
    public static Tuple<double, double, double> rank(doc A, query B, corpus C)
    {
        return new Tuple<double, double, double>(rank_by_weight(A,B,C), clos(A,B,C) ,rank_by_min_interval(A,B,C));
    }


    public static double clos(doc A, query B, corpus C)
    {
        double result = 0;
        if(B.close_words.Count > 0)
        {
            foreach (var item in B.close_words)
            {
                double temp = compute_score(A, item, C);
                if(temp > result)
                {
                    result = temp;
                }

            }
        }
        return result;
    }
    public static double compute_score(doc A, string[] close, corpus C)
    {
        return 1;
    }

    public static double rank_by_weight(doc A, query B, corpus C)
    {
        double score = 0;
        foreach(var item in B.words)
        {
            score = score + item.Value*A.get_info(item.Key, C).weight;
        }
        return Math.Cos(score / (A.norm * B.norm));
    }

    public static double rank_by_min_interval(doc A, query B, corpus C)
    {
        List<pos> words_in_document = new List<pos>();
        int cant = 0;
        foreach(var item in B.words)
        {
            if (A.contain_word(item.Key, C))
            {
                cant = cant +1;
                foreach (var pos in A.get_info(item.Key,C).places)
                {
                    words_in_document.Add(pos);
                }
            }
        }
        words_in_document = pos.sort(words_in_document);
        if(cant > 0){
        int min = words_in_document[0].start;
        int max = words_in_document[words_in_document.Count-1].end();
        return cant + 1/((double)( max - min ));
        }
        return 0;
    }



}