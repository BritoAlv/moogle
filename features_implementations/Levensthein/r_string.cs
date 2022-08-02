// stackoverflow code.
using System;
using System.Text;
public static class r_string
{
    private static Random random = new Random();
    public static string random_string(int size)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, size).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
