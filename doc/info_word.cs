namespace docc;
public class info_word
{
    public double weight;
    public List<int> pos; // it's essential that this list be sorted.

    public info_word()
    {
        weight = 0;
        pos = new List<int>();
    }

    public info_word(List<int> C)
    {
        weight = 0;
        pos = C;
    }
}