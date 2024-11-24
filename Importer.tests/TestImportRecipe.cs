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
    private Mock<IIngredientsApi> _mockIngredientApiOk;

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
        IBarAssistantRepository barRepo = new BarAssistantRepository(_mockAuthApiOk.Object, _mockImportApiOk.Object,
            _mockIngredientApiOk.Object);
        barRepo.Authenticate("test@test.com", "123454");
        Assert.True(barRepo.IsAuthenticated());
    }

    [Test]
    public void test_scrape_recipe()
    {
        var diffordRepository = new JsonImporterDiffordRepository();
        IBarAssistantRepository barRepo = new BarAssistantRepository(_mockAuthApiOk.Object, _mockImportApiOk.Object,
            _mockIngredientApiOk.Object);
        barRepo.Authenticate(_validUserEmail, _validPassword);
        var diffordScrapedRecipeList = diffordRepository.readFromFile("data/difford_mojito-cocktail.json");
        IList<Ingredient> ingredientList = barRepo.GetIngredientList();

        CocktailRecipeDraft02 barScrapedRecipe =
            barRepo.ScrapeDraftCocktailRecipe(diffordScrapedRecipeList[0].Link, 1, 1);
        DCocktail dRecipe =
            barRepo.RecipeWithMatchedIngredientAndInfo(barScrapedRecipe, ingredientList, diffordScrapedRecipeList[0]);
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

        var importedIngredientListResponse = File.ReadAllText("data/get_ingredient_list_response_200.json");
        var importedIngredientListResponseObj =
            JsonConvert.DeserializeObject<GetIngredientList200Response>(importedIngredientListResponse,
                new JsonSerializerSettings
                {
                }) ??
            new GetIngredientList200Response();

        _mockIngredientApiOk = new Mock<IIngredientsApi>();
        _mockIngredientApiOk.Setup(
            c => c.GetIngredientList(
                It.IsAny<int?>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<GetIngredientListFilterParameter>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<int>()
            )
        ).Returns(() => importedIngredientListResponseObj);
    }
}