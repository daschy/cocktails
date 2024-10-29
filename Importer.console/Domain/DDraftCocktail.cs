using Infra.BarAssistant.Gen.Model;

namespace Importer.console.Domain;

public record DDraftCocktail(
    string Id,
    string? Name = default,
    string? Instructions = default,
    DateTime? CreatedAt = default,
    string? Description = default,
    string? Source = default,
    string? Garnish = default,
    decimal? Abv = default,
    List<string>? Tags = default,
    string? Glass = default,
    string? Method = default,
    List<string>? Utensils = default,
    List<string>? ImageUriList = default,
    IList<DIngredient>? Ingredients = default,
    IList<DDraftIngredient>? DraftIngredients = default
) : BaseEntity
{
}