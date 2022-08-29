public class pos
{
    public string word;
    public int start;

    public pos(string word, int start)
    {
        this.word = word;
        this.start = start;
    }
    public int end()
    {
        return this.start + word.Length;
    }
    
    // sort the pos object basen on start
    public static List<pos> sort(List<pos> some_pos)
    {
        // sort pos objects based on start only
        // not implemented 
        some_pos.OrderBy(x => x.start);
        return some_pos;
    }

    // check if two pos refer to the same word
    public static bool same_word(pos a, pos b)
    {
        return a.word == b.word;
    }


}