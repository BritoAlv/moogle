public class info {
    public int term_frequency{get;set;}
    public double tf_idf{get;set;}
    public List<Tuple<int,int>> positions{get; set;}
    public string linked{get; set;}
    public info(string word)
    {
        term_frequency = 0;
        positions = new List<Tuple<int, int>>();
        linked = word;
    } 
}