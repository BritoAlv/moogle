// stackoverflow code.


public static class r_string
{
    private static Random random = new Random();
    /// <summary>
    /// generar un string aleatorio.
    /// </summary>
    /// <param name="size"> longitud del string </param>
    /// <returns></returns>
    public static string random_string(int size)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, size).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
