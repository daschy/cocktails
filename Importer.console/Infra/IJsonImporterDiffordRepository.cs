using Importer.console.Domain;

namespace Importer.console.Infra;

public interface IJsonImporterDiffordRepository
{
    IList<DiffordCocktailRecipe> readFromFile(string path);
}