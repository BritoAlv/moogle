public class info {
    public int term_frequency{get;set;}
    public double tf_idf{get;set;}
    public List<Tuple<int,int>> positions{get; set;}

    public info()
    {
        term_frequency = 0;
    } 
}