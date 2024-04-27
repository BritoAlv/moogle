namespace Constants;
using System.Text.Json;
public class Constant
{
    public readonly Dictionary<string, double> constants;
    private List<string> _cons = new List<string>()
    {
        "medalla_bronce_tf_idf",
        "medalla_bronce_cercania",
        "medalla_bronce_min_interval",
        "medalla_plata_tf_idf",
        "medalla_plata_cercania",
        "medalla_plata_min_interval",
        "medalla_oro_tf_idf",
        "medalla_oro_cercania",
        "medalla_oro_min_interval",
        "idf_factor",
        "popular_factor",
        "min_interval_length_to_be_considered_as_good",
        "min_interval_length_to_snippet",
        "diameter_cercania",
        "extra_length_snippet",
    };

    private readonly string _path = "../Constants/config.json";
    public Constant()
    {
        constants = new Dictionary<string, double>();
        try
        {
            Dictionary<string, double> fromFile = JsonSerializer.Deserialize<Dictionary<string, double>>(File.ReadAllText(_path));
            foreach (var key in _cons)
            {
                constants[key] = fromFile[key];
            }
        }
        catch (System.Exception)
        {
            constants["medalla_bronce_tf_idf"] = 2;
            constants["medalla_bronce_cercania"] = 2;
            constants["medalla_bronce_min_interval"] = 2;
            constants["medalla_plata_tf_idf"] = 4;
            constants["medalla_plata_cercania"] = 4;
            constants["medalla_plata_min_interval"] = 3;
            constants["medalla_oro_tf_idf"] = 5;
            constants["medalla_oro_cercania"] = 6;
            constants["medalla_oro_min_interval"] = 4;
            constants["idf_factor"] = 0.90;
            constants["popular_factor"] = 0.80;
            constants["min_interval_length_to_be_considered_as_good"] = 2000;
            constants["min_interval_length_to_snippet"] = 300;
            constants["diameter_cercania"] = 500;
            constants["extra_length_snippet"] = 100;
            string jsonString1 = JsonSerializer.Serialize(constants, new JsonSerializerOptions{WriteIndented = true});
            File.WriteAllText(_path, jsonString1);
        }
    }
}
