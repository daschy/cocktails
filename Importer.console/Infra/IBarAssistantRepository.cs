using Importer.console.Domain;
using Importer.console.Infra;
using Infra.BarAssistant.Gen.Model;

namespace Importer.test;

public interface IBarAssistantRepository
{
    void Authenticate(string testTestCom, string s);
    bool IsAuthenticated();
    CocktailRecipeDraft02 ScrapeDraftCocktailRecipe(string httpServerCom, int barId, int barAssistantBarId);
    
    List<MatchedIngredients> MatchIngredient(string httpServerCom, int barId, int barAssistantBarId);
    
}

public class MatchedIngredients
{
    
}