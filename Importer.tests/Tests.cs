using Importer.console.Domain;
using Importer.console.Infra;


namespace Importer.test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void test_read_cocktail_from_difford_json()
    {
        var diffordRepo = new ImporterDiffordRepository();
        IEnumerable<CocktailRecipe> recipeList = diffordRepo.readFromFile("data/difford_cocktail_mojito.json");
        Assert.IsNotEmpty(recipeList);
    }

    [Test(Description = "Ingredients parsing")]
    [TestCase(5,
        "14 fresh\\tMint leaves\\n2 shot\\tLight gold rum (1-3 year old molasses column)\\n1\u20442 shot\\tLime juice (freshly squeezed)\\n1\u20443 shot\\tSugar syrup 'rich' (2 sugar to 1 water, 65.0\u00b0Brix)\\n1\u20442 shot\\tThomas Henry Soda Water")]
    [TestCase(4,
        "1\u20442 fresh\\tLime (fresh) (chopped)\\n3 barspoon\\tPowdered sugar (white sugar ground in mortar and pestle)\\n3\u20444 shot\\tChilled water (omit if using wet ice)\\n2 shot\\tCachaça (from freezer)")]
    public void test_parse_ingredients_from_difford_json(int numberOfIngredients, string ingredientStr)
    {
        var ingredientList = MapperStringToIngredientList.ParseIngredients(ingredientStr);
        Assert.IsNotEmpty(ingredientList);
        Assert.IsTrue(numberOfIngredients == ingredientList.Count());
    }
}