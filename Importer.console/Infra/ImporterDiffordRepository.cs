using Importer.console.Domain;
using Newtonsoft.Json;


namespace Importer.console.Infra;

public class ImporterDiffordRepository : IImporterDiffordRepository
{
    public IList<DiffordCocktailRecipe> readFromFile(string path)
    {
        try
        {
            string json = File.ReadAllText(path);
            IList<DiffordCocktailRecipe> cocktails = JsonConvert.DeserializeObject<IList<DiffordCocktailRecipe>>(json);

            return cocktails;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading from file: {ex.Message}");
            throw;
        }
    }
}