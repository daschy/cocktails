using System.Data;
using System.Diagnostics;
using Importer.console.Domain;
using Importer.test;
using Infra.BarAssistant.Gen.Api;
using Infra.BarAssistant.Gen.Client;
using Infra.BarAssistant.Gen.Model;

namespace Importer.console.Infra;

public class BarAssistantRepository(IAuthenticationApi authApi, IImportApi importApi) : IBarAssistantRepository
{
    private string? _accessToken;

    public void Authenticate(string email, string pwd)
    {
        var loginRequest = new LoginRequest(email, password: pwd);
        try
        {
            // Authenticate user and get a token
            Login200Response result = authApi.Login(loginRequest);
            _accessToken = result.Data.VarToken;
            Debug.WriteLine(result);
        }
        catch (ApiException e)
        {
            Debug.Print("Exception when calling AuthenticationApi.Login: " + e.Message);
            Debug.Print("Status Code: " + e.ErrorCode);
            Debug.Print(e.StackTrace);
            throw;
        }
    }

    public CocktailRecipeDraft02 ScrapeDraftCocktailRecipe(string recipeUrl, int barId, int barAssistantBarId)
    {
        if (!IsAuthenticated())
        {
            throw new ConstraintException("Token is empty");
        }

        var scrapeRecipeRequest = new ScrapeRecipeRequest(recipeUrl);
        try
        {
            // Scrape a recipe
            ScrapeRecipe200Response scrapedRecipe =
                importApi.ScrapeRecipe(scrapeRecipeRequest, barId, barAssistantBarId);
            Debug.WriteLine(scrapedRecipe);
            return scrapedRecipe.Data.Schema;
        }
        catch (ApiException e)
        {
            Debug.Print("Exception when calling ImportApi.ScrapeRecipe: " + e.Message);
            Debug.Print("Status Code: " + e.ErrorCode);
            Debug.Print(e.StackTrace);
            throw;
        }
    }

    public DCocktail MergeDraftAndDiffordData(CocktailRecipeDraft02 recipe,
        DiffordCocktailRecipe diffordData)
    {
        if (!IsAuthenticated())
        {
            throw new ConstraintException("Token is empty");
        }
        
        throw new NotImplementedException();
    }

    public bool IsAuthenticated()
    {
        return !string.IsNullOrEmpty(_accessToken);
    }
}