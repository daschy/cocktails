using Importer.console.Domain;

namespace Importer.console.Infra;

public record DiffordCocktailRecipe(
    string Name,
    string Image,
    string Glass,
    string Garnish,
    string Preparation,
    string Ingredients,
    string Review,
    string History,
    string Link
)
{
    // private DIngredient[] IngredientList { get; set; } =
    //     MapperStringToIngredientList.ParseIngredients(Ingredients, Name);
}