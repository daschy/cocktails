using System.Data;
using System.Diagnostics;
using Importer.console.Domain;
using Importer.test;
using Infra.BarAssistant.Gen.Api;
using Infra.BarAssistant.Gen.Client;
using Infra.BarAssistant.Gen.Model;

namespace Importer.console.Infra;

public class BarAssistantRepository : IBarAssistantRepository
{
    private readonly ApiClient _client;
    private readonly Configuration _config;

    public BarAssistantRepository(ApiClient client, string basePath)
    {
        _client = client;
        _config = new Configuration()
        {
            BasePath = basePath
        };
    }

    public string Token { get; private set; }

    public bool Authenticate(string email, string pwd)
    {
        var apiInstance = new AuthenticationApi(
            _client,
            _client,
            _config);
        var loginRequest = new LoginRequest(email, password: pwd);

        try
        {
            // Authenticate user and get a token
            Login200Response result = apiInstance.Login(loginRequest);
            Token = result.Data.VarToken;
            Debug.WriteLine(result);
            return true;
        }
        catch (ApiException e)
        {
            Debug.Print("Exception when calling AuthenticationApi.Login: " + e.Message);
            Debug.Print("Status Code: " + e.ErrorCode);
            Debug.Print(e.StackTrace);
        }


        throw new NotImplementedException();
    }

    public CocktailRecipeDraft02Recipe ScrapeCocktailRecipe(string recipeUrl, int barId, int barAssistantBarId)
    {
        if (!string.IsNullOrEmpty(Token))
        {
            var apiInstance = new ImportApi(_config);
            var scrapeRecipeRequest = new ScrapeRecipeRequest(recipeUrl); // ScrapeRecipeRequest | 
            var
                _barId = barId; // int? | Database id of a bar. Required if you are not using `Bar-Assistant-Bar-Id` header. (optional) 
            var _barAssistantBarId =
                barAssistantBarId; // int? | Database id of a bar. Required if you are not using `bar_id` query string. (optional) 

            try
            {
                // Scrape a recipe
                ScrapeRecipe200Response result =
                    apiInstance.ScrapeRecipe(scrapeRecipeRequest, _barId, _barAssistantBarId);
                Debug.WriteLine(result);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling ImportApi.ScrapeRecipe: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }

        throw new ConstraintException("Token is empty");
    }

    public CocktailRecipe ImportCocktailRecipe(CocktailRecipeDraft02Recipe recipeDraft,
        DiffordCocktailRecipe additionalData)
    {
        throw new NotImplementedException();
    }
}