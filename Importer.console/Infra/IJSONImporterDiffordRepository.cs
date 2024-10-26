using Importer.console.Domain;

namespace Importer.console.Infra;

public interface IJSONImporterDiffordRepository
{
    IList<DiffordCocktailRecipe> readFromFile(string path);
}