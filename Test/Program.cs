global using qquery;
global using corpuss;
using System.Diagnostics;

public class Test
{
	public static void Main()
	{   
		//corpus a = new corpus(true);
		//query b = new query("noche", a);
      	corpus a = new corpus(true);
		Console.WriteLine("Empieza A Poner Frases");
		while (true) 
			{
				Console.WriteLine("\n");
				Console.Write("Pon una frase chama: ");
				Stopwatch aw = new Stopwatch();
				string h = Console.ReadLine();
				Console.Write("\n");
				aw.Start();
				query b = new query(h, a);
				aw.Stop();

				Console.Write("Se Usaron en ~:");
				foreach (var item in b.closest_words)
				{
					Console.Write(" " + item);	
				}
				Console.WriteLine(" ");	

				Console.Write("Se Usaron:");
				foreach (var item in b.words_to_request)
				{
					Console.Write(" " + item);	
				}
				Console.WriteLine(" ");			
				Console.WriteLine("Se demoró todo este tiempo mi rey: " + aw.Elapsed);
				Console.WriteLine("////////////////////////////////////////////////////////////////////");
			}	      
	}			
}
