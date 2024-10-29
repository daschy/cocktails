using Importer.console.Domain;
using Importer.console.Infra;
using Infra.BarAssistant.Gen.Model;

namespace Importer.test;

public interface IBarAssistantRepository
{
    void Authenticate(string testTestCom, string s);
    CocktailRecipeDraft02 ScrapeDraftCocktailRecipe(string httpServerCom, int barId, int barAssistantBarId);
    DCocktail MergeDraftAndDiffordData(CocktailRecipeDraft02 recipe, DiffordCocktailRecipe diffordData);

    bool IsAuthenticated();
}