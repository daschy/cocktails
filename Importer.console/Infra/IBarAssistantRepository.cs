using Importer.console.Domain;
using Importer.console.Infra;
using Infra.BarAssistant.Gen.Model;

namespace Importer.test;

public interface IBarAssistantRepository
{
    bool Authenticate(string testTestCom, string s);
    CocktailRecipeDraft02Recipe ScrapeCocktailRecipe(string httpServerCom, int barId, int barAssistantBarId);
    CocktailRecipe ImportCocktailRecipe(CocktailRecipeDraft02Recipe recipeDraft, DiffordCocktailRecipe additionalData);
    string Token { get; }
}