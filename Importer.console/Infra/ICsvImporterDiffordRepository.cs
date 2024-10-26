using Importer.console.Domain;

namespace Importer.console.Infra;

public interface ICsvImporterDiffordRepository
{
    IList<DiffordCocktailRecipe> readFromFile(string path);
}