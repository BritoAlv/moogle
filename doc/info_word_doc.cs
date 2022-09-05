namespace docc;
public class info_word_doc
{
    public int idf; // store number of docs that contain this word
    public string linked; // store linked word
    public IEnumerable<string>? similar; // in case of being root store similar words
    public Dictionary<int, info_word> docs;  // store positions of this word in the docs.


    public info_word_doc(string linked)
    {
        this.linked = linked; // by default each word is linked to itself
        this.docs = new Dictionary<int, info_word>();
        this.idf = 0;

    }

    public void set_linked(string linked)
    {
        this.linked = linked;
        
    }
}