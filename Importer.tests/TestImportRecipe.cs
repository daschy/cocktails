using System.Net;
using System.Text.Json.Nodes;
using Importer.console.Domain;
using Importer.console.Infra;
using Infra.BarAssistant.Gen.Client;
using Infra.BarAssistant.Gen.Model;
using Moq;
using Newtonsoft.Json;

namespace Importer.test;

public class TestImportRecipe
{
    private Mock<ApiClient> _mockApiClient;
    private string _validUserEmail;
    private string _validPassword;
    private string _validBasePath;

    [SetUp]
    public void Setup()
    {
        _mockApiClient = SetupApiClient();
        _validUserEmail = "test@test.com";
        _validPassword = "123454";
        _validBasePath = "http://localhost:8000/api";
    }

    [Test]
    public void test_login()
    {
        IBarAssistantRepository barRepo = new BarAssistantRepository(_mockApiClient.Object, _validBasePath);
        bool isAuthenticated = barRepo.Authenticate("test@test.com", "123454");
        Assert.True(isAuthenticated);
        Assert.IsNotEmpty(barRepo.Token);
    }

    [Test]
    public void test_import_recipe()
    {
        IBarAssistantRepository barRepo = new BarAssistantRepository(_mockApiClient.Object, _validBasePath);
        barRepo.Authenticate(_validUserEmail, _validPassword);
        IJSONImporterDiffordRepository diffordRepository = new JSONImporterDiffordRepository();
        var recipeList = diffordRepository.readFromFile("data/difford_mojito-cocktail.json");
        foreach (var diffordCocktailRecipe in recipeList)
        {
            CocktailRecipeDraft02 recipeDraft = barRepo.ScrapeCocktailRecipe("http://server.com/12314", 1, 1);
            Cocktail recipe = barRepo.ImportCocktailRecipe(
                recipeDraft,
                additionalData: diffordCocktailRecipe
            );
            Assert.IsNull(recipe.Instructions);
        }
    }

    private Mock<ApiClient> SetupApiClient()
    {
        var mockApiClient = new Mock<ApiClient>();
        // mockApiClient
        //     .As<ISynchronousClient>()
        //     .Setup(
        //         c => c.Post<Login200Response>(
        //             "/auth/login",
        //             It.IsAny<RequestOptions>(),
        //             It.IsAny<IReadableConfiguration>()
        //         )
        //     )
        //     .Returns(
        //         () => new ApiResponse<Login200Response>(
        //             HttpStatusCode.OK,
        //             new Login200Response(new Token("123456"))
        //         )
        //     );
        var respoJson = File.ReadAllText("data/scraped_mojito-cocktail.json");
        var scrapedCocktailResponse =
            JsonConvert.DeserializeObject<ScrapeRecipe200Response>(respoJson);
        mockApiClient
            .As<ISynchronousClient>()
            .Setup(
                c => c.Post<ScrapeRecipe200Response>(
                    It.IsAny<string>(),
                    It.IsAny<RequestOptions>(),
                    It.IsAny<IReadableConfiguration>()
                )
            )
            .Returns(
                () => new ApiResponse<ScrapeRecipe200Response>(
                    HttpStatusCode.OK,
                    scrapedCocktailResponse
                )
            );
        return mockApiClient;
    }
}