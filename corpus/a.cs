namespace corpus;

public static class A
{
    public static void B()
    {
        DirectoryInfo c = new DirectoryInfo("../Content/");
        Console.WriteLine(c.GetFiles()[0].ToString());
    }
}