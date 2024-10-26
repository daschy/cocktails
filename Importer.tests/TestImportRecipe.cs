using System.Net;
using Importer.console.Domain;
using Importer.console.Infra;
using Infra.BarAssistant.Gen.Client;
using Infra.BarAssistant.Gen.Model;
using Moq;

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
        var basePath = "http://localhost:8000/api";
        IBarAssistantRepository barRepo = new BarAssistantRepository(_mockApiClient.Object, basePath);
        bool isAuthenticated = barRepo.Authenticate("test@test.com", "123454");
        Assert.True(isAuthenticated);
        Assert.IsNotEmpty(barRepo.Token);
    }

    [Ignore("not ready")]
    [Test]
    public void test_import_recipe()
    {
        IBarAssistantRepository barRepo = new BarAssistantRepository(_mockApiClient.Object, _validBasePath);
        barRepo.Authenticate(_validUserEmail, _validPassword);
        ICsvImporterDiffordRepository diffordRepository = new CsvImporterDiffordRepository();
        var recipeList = diffordRepository.readFromFile("data/difford_cocktail_mojito.json");
        foreach (var diffordCocktailRecipe in recipeList)
        {
            CocktailRecipeDraft02Recipe recipeDraft = barRepo.ScrapeCocktailRecipe("http://server.com/12314", 3, 4);
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
        mockApiClient.As<ISynchronousClient>().Setup(
            c => c.Post<Login200Response>(
                It.IsAny<string>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<IReadableConfiguration>()
            )
        ).Returns(
            () => new ApiResponse<Login200Response>(
                HttpStatusCode.OK,
                new Login200Response(new Token("123456"))
            )
        );
        return mockApiClient;
    }
}