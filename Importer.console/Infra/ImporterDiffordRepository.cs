using Importer.console.Domain;
using Newtonsoft.Json;


namespace Importer.console.Infra;

public class ImporterDiffordRepository : IImporterRepository
{
    public IList<CocktailRecipe> readFromFile(string path)
    {
        try
        {
            string json = File.ReadAllText(path);
            IList<DiffordCocktailRecipe> cocktails = JsonConvert.DeserializeObject<IList<DiffordCocktailRecipe>>(json);
            
            return new List<CocktailRecipe>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading from file: {ex.Message}");
            return new List<CocktailRecipe>();
        }
    }
}