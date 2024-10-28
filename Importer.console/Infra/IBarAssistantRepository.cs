using Importer.console.Domain;
using Importer.console.Infra;
using Infra.BarAssistant.Gen.Model;

namespace Importer.test;

public interface IBarAssistantRepository
{
    bool Authenticate(string testTestCom, string s);
    CocktailRecipeDraft02 ScrapeCocktailRecipe(string httpServerCom, int barId, int barAssistantBarId);
    Cocktail ImportCocktailRecipe(CocktailRecipeDraft02 recipeDraft, DiffordCocktailRecipe additionalData);
}