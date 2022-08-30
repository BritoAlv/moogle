namespace MoogleEngine;

// with corpus. access to the classes of the Library Class corpus
using corpus;

public static class Moogle
{
    public static SearchResult Query(string query)
    {
        // Modifique este método para responder a la búsqueda
        SearchItem[] items = new SearchItem[3]
        {
            new SearchItem("Hello World", "No tengo ganas de hacer el Moogle", 0.9f),
            new SearchItem("Hello World", "Que fastidio tener que hacer esto en las vacaciones", 0.8f),
            new SearchItem("Hello World", "Life goes on and on and on", 0.1f),
        };

        return new SearchResult(items, query);
    }
}
