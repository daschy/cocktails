namespace Importer.console.Domain;

public record DDraftIngredient(
    string? Id = default(string),
    string? Name = default(string),
    decimal? Strength = default(decimal?),
    string? Description = default(string),
    string? Origin = default(string),
    string? Category = default(string))
{
}