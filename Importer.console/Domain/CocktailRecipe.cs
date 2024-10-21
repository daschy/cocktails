namespace Importer.console.Domain;

public record CocktailRecipe(string name, IEnumerable<Ingredient> ingredientList) : BaseEntity;