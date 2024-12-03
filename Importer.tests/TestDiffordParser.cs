using Importer.console.Domain;
using Importer.console.Infra;


namespace Importer.test;


[Ignore("Obsolete")]
public class TestDiffordParser
{
    [SetUp]
    public void Setup()
    {
    }


    [Test]
    [TestCase(
        "Top up with", 4f, DUnit.shot
    )]
    public void test_ingredient_quantity_and_unit_parser(string quantityAndUnitStr, float expectedQuantity,
        DUnit expectedUnit)
    {
        var quantityAndUnit =
            MapperStringToIngredientList.ParseQuantityAndUnit(quantityAndUnitStr,
                MapperStringToIngredientList.UnitMappings);
        Assert.That(quantityAndUnit.Item1, Is.EqualTo(expectedQuantity));
        Assert.That(quantityAndUnit.Item2, Is.EqualTo(expectedUnit));
    }

    [Test]
    public void test_read_cocktail_from_difford_json()
    {
        var diffordRepo = new JsonImporterDiffordRepository();
        IEnumerable<DiffordCocktailRecipe>? recipeList = diffordRepo.readFromFile("data/difford_mojito-cocktail.json");
        Assert.IsNotEmpty(recipeList);
    }


    [Test(Description = "Ingredients parsing")]
    [TestCase(
        "6 2\u20443 shot\tCold black breakfast tea\n6 2\u20443 shot\tLemon juice (freshly squeezed)\nfresh\tLemon peel\n6 2\u20443 shot\tOrange juice (freshly squeezed)\nfresh\tOrange peel\n2 shot\tKetel One Vodka\n1 shot\tSugar syrup 'rich' (2 sugar to 1 water, 65.0\u00b0Brix)\n6 2\u20443 shot\tMilk (whole milk/full 3-4% fat)\n1 2\u20443 shot\tKetel One Vodka\n1\u20444 shot\tSugar syrup 'rich' (2 sugar to 1 water, 65.0\u00b0Brix)",
        10,
        new[] { 6 + 2 / 3f, 6 + 2 / 3f, 1, 6 + 2 / 3f, 1, 2, 1, 6 + 2 / 3f, 1 + 1 / 3f, 1 / 4f },
        new[]
        {
            "Cold black breakfast tea", "Lemon juice", "Lemon peel", "Orange juice", "Orange peel", "Ketel One Vodka",
            "Sugar syrup 'rich'", "Milk", "Ketel One Vodka", "Sugar syrup 'rich'"
        },
        new[]
        {
            "", "freshly squeezed", "", "freshly squeezed", "", "", "2 sugar to 1 water, 65.0\u00b0Brix",
            "whole milk/full 3-4% fat", "", "2 sugar to 1 water, 65.0\u00b0Brix"
        }
    )]
    [TestCase(
        "2 shot\tBlended Scotch whisky\nTop up with\tChilled water",
        2,
        new[] { 2f, 4f },
        new[] { "Blended Scotch whisky", "Chilled water" },
        new[] { "", "" }
    )]
    [TestCase(
        "14 fresh\tMint leaves\n2 shot\tLight gold rum (1-3 year old molasses column)\n1\u20442 shot\tLime juice (freshly squeezed)\n1\u20443 shot\tSugar syrup 'rich' (2 sugar to 1 water, 65.0\u00b0Brix)\n1\u20442 shot\tThomas Henry Soda Water",
        5,
        new[] { 14f, 2, 1f / 2f, 1f / 3f, 1f / 2f },
        new[] { "Mint leaves", "Light gold rum", "Lime juice", "Sugar syrup 'rich'", "Thomas Henry Soda Water" },
        new[]
        {
            "", "1-3 year old molasses column", "freshly squeezed", "2 sugar to 1 water, 65.0\u00b0Brix", ""
        }
    )]
    [TestCase(
        "1\u20442 fresh\tLime (fresh) (chopped)\n3 barspoon\tPowdered sugar (white sugar ground in mortar and pestle)\n3\u20444 shot\tChilled water (omit if using wet ice)\n2 shot\tCachaça (from freezer)",
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
        var ingredientList = MapperStringToIngredientList.ParseIngredients(ingredientStr, string.Empty);
        Assert.IsNotEmpty(ingredientList);
        Assert.IsTrue(numberOfIngredients == ingredientList.Count());
        Assert.That(ingredientList.Select((ing) => ing.Quantity), Is.EquivalentTo(quantities));
        Assert.That(ingredientList.Select((ing) => ing.Name), Is.EquivalentTo(names));
        Assert.That(ingredientList.Select((ing) => ing.Notes), Is.EquivalentTo(notes));
    }
}