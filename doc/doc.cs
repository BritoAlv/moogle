namespace docc;
public class doc
{
    public int id;
    public string name;
    public string text;
    public string path;
    public int cant_palabras;
    public double norm;
    public doc(string name, int id)
    {
        this.path = name;
        this.id = id;
        this.name = name.Substring(0, name.Length-4);
        this.text = System.IO.File.ReadAllText("../Content/"+this.path);
    }
}