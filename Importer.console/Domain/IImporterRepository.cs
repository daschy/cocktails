using Infra.BarAssistant.Gen.Model;

namespace Importer.console.Domain;

public interface IImporterRepository
{
    public IList<Cocktail> readFromFile(string path);
}