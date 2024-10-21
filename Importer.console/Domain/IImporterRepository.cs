namespace Importer.console.Domain;

public interface IImporterRepository
{
    public IList<CocktailRecipe> readFromFile(string path);
}