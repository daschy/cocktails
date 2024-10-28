using Importer.console.Domain;
using Importer.console.Infra;


namespace Importer.test;

public class TestDiffordParser
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void test_read_cocktail_from_difford_json()
    {
        var diffordRepo = new JSONImporterDiffordRepository();
        IEnumerable<DiffordCocktailRecipe> recipeList = diffordRepo.readFromFile("data/difford_mojito-cocktail.json");
        Assert.IsNotEmpty(recipeList);
    }

    [Test(Description = "Ingredients parsing")]
    [TestCase(
        "14 fresh\\tMint leaves\\n2 shot\\tLight gold rum (1-3 year old molasses column)\\n1\u20442 shot\\tLime juice (freshly squeezed)\\n1\u20443 shot\\tSugar syrup 'rich' (2 sugar to 1 water, 65.0\u00b0Brix)\\n1\u20442 shot\\tThomas Henry Soda Water",
        5,
        new[] { 14f, 2, 1f / 2f, 1f / 3f, 1f / 2f },
        new[] { "Mint leaves", "Light gold rum", "Lime juice", "Sugar syrup 'rich'", "Thomas Henry Soda Water" },
        new[]
        {
            "", "1-3 year old molasses column", "freshly squeezed", "2 sugar to 1 water, 65.0\u00b0Brix", ""
        }
    )]
    [TestCase(
        "1\u20442 fresh\\tLime (fresh) (chopped)\\n3 barspoon\\tPowdered sugar (white sugar ground in mortar and pestle)\\n3\u20444 shot\\tChilled water (omit if using wet ice)\\n2 shot\\tCachaça (from freezer)",
        4,
        new[] { 1f / 2f, 3f, 3f / 4f, 2 },
        new[]
        {
            "Lime", "Powdered sugar", "Chilled water", "Cachaça"
        },
        new[]
        {
            "fresh", "white sugar ground in mortar and pestle", "omit if using wet ice", "from freezer"
        }
    )]
    public void test_parse_ingredients_from_difford_json(
        string ingredientStr,
        int numberOfIngredients,
        float[] quantities,
        string[] names,
        string[] notes
    )
    {
        var ingredientList = MapperStringToIngredientList.ParseIngredients(ingredientStr);
        Assert.IsNotEmpty(ingredientList);
        Assert.IsTrue(numberOfIngredients == ingredientList.Count());
        Assert.That(ingredientList.Select((ing) => ing.quantity), Is.EquivalentTo(quantities));
        Assert.That(ingredientList.Select((ing) => ing.name), Is.EquivalentTo(names));
        Assert.That(ingredientList.Select((ing) => ing.notes), Is.EquivalentTo(notes));
    }
}