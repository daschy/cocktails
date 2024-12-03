using Importer.console.Domain;
using Newtonsoft.Json;


namespace Importer.console.Infra;

public class JsonImporterDiffordRepository : IJsonImporterDiffordRepository
{
    public IList<DiffordCocktailRecipe> readFromFile(string path)
    {
        try
        {
            string json = File.ReadAllText(path);
            IList<DiffordCocktailRecipe>? cocktails = JsonConvert.DeserializeObject<IList<DiffordCocktailRecipe>>(json);

            return cocktails ?? new List<DiffordCocktailRecipe>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading from file: {ex.Message}");
            throw;
        }
    }

    public void writeToFile(IList<DiffordCocktailRecipe> data, string path)
    {
        try
        {
            string text = JsonConvert.SerializeObject(data, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
            File.WriteAllText(path, text);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading from file: {ex.Message}");
            throw;
        }
    }
}