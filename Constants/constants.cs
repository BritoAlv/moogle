namespace Constants;
using System.Text.Json;
public class constant
{
    public Dictionary<string, double> constants;
    public List<string> cons = new List<string>()
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
    };
    string path = "../Constants/config.json";
    public constant()
    {
        constants = new Dictionary<string, double>();
        try
        {
            Dictionary<string, double> from_file = JsonSerializer.Deserialize<Dictionary<string, double>>(File.ReadAllText(path));
            foreach (var key in cons)
            {
                constants[key] = from_file[key];
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
            string jsonString1 = JsonSerializer.Serialize(constants, new JsonSerializerOptions{WriteIndented = true});
            File.WriteAllText(path, jsonString1);
        }
    }
}
