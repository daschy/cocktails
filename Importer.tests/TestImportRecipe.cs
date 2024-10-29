using System.Net;
using System.Text.Json.Nodes;
using Importer.console.Domain;
using Importer.console.Infra;
using Infra.BarAssistant.Gen.Api;
using Infra.BarAssistant.Gen.Client;
using Infra.BarAssistant.Gen.Model;
using Moq;
using Moq.Language.Flow;
using Newtonsoft.Json;

namespace Importer.test;

public class TestImportRecipe
{
    private Mock<ApiClient> _mockApiClient;
    private string _validUserEmail;
    private string _validPassword;
    private string _validBasePath;
    private Mock<IAuthenticationApi> _mockAuthApiOk;
    private Mock<IImportApi> _mockImportApiOk;

    [SetUp]
    public void Setup()
    {
        SetupApis();
        _validUserEmail = "test@test.com";
        _validPassword = "123454";
        _validBasePath = "http://localhost:8000/api";
    }

    [Test]
    public void test_login()
    {
        IBarAssistantRepository barRepo = new BarAssistantRepository(_mockAuthApiOk.Object, _mockImportApiOk.Object);
        barRepo.Authenticate("test@test.com", "123454");
        Assert.True(barRepo.IsAuthenticated());
    }

    [Test]
    public void test_import_recipe()
    {
        IBarAssistantRepository barRepo = new BarAssistantRepository(_mockAuthApiOk.Object, _mockImportApiOk.Object);
        barRepo.Authenticate(_validUserEmail, _validPassword);
        var diffordRepository = new JsonImporterDiffordRepository();
        var recipeList = diffordRepository.readFromFile("data/difford_mojito-cocktail.json");
        foreach (var diffordCocktailRecipe in recipeList)
        {
            DCocktailDraft recipeDraft = barRepo.ScrapeDraftCocktailRecipe("http://server.com/12314", 1, 1);
            DCocktail importedRecipe = barRepo.ImportCocktailRecipe(
                recipeDraft,
                additionalData: diffordCocktailRecipe
            );
            Assert.That(importedRecipe.Instructions, Is.EqualTo(diffordCocktailRecipe.Preparation));
            foreach (var (ingredient, i) in importedRecipe.Ingredients.Select((v, i) => (v, i)))
            {
                Assert.That(ingredient.Note, Is.EqualTo(diffordCocktailRecipe.Ingredients));
                
            }
        }
    }

    private void SetupApis()
    {
        _mockAuthApiOk = new Mock<IAuthenticationApi>();
        _mockAuthApiOk.Setup(
                c => c.Login(
                    It.IsAny<LoginRequest>(),
                    It.IsAny<int>()
                )
            )
            .Returns(
                () => new Login200Response(new Token("1234"))
            );

        var respoJson = File.ReadAllText("data/scraped_mojito-cocktail.json");
        var scrapedCocktailResponse =
            JsonConvert.DeserializeObject<ScrapeRecipe200Response>(respoJson) ?? new ScrapeRecipe200Response();

        _mockImportApiOk = new Mock<IImportApi>();
        _mockImportApiOk.Setup(
            c => c.ScrapeRecipe(
                It.IsAny<ScrapeRecipeRequest>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>()
            )
        ).Returns(() => scrapedCocktailResponse);
    }
}