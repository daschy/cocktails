using Importer.console.Domain;
using Importer.console.Infra;
using Infra.BarAssistant.Gen.Model;

namespace Importer.test;

public interface IBarAssistantRepository
{
    void Authenticate(string testTestCom, string s);
    DCocktailDraft ScrapeDraftCocktailRecipe(string httpServerCom, int barId, int barAssistantBarId);
    DCocktail ImportCocktailRecipe(DCocktailDraft recipeDraft, DiffordCocktailRecipe additionalData);

    bool IsAuthenticated();
}