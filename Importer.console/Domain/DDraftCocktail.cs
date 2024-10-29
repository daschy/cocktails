using Infra.BarAssistant.Gen.Model;

namespace Importer.console.Domain;

public class DCocktailDraft : CocktailRecipeDraft02
{
    public DCocktailDraft(CocktailRecipeDraft02Recipe recipe = default(CocktailRecipeDraft02Recipe), List<D> ingredients = default(List<Ingredient1>)) : base(recipe, ingredients)
    {
    }
}