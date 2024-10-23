using Importer.console.Domain;

namespace Importer.console.Infra;

public interface IImporterDiffordRepository
{
    IList<DiffordCocktailRecipe> readFromFile(string path);
}