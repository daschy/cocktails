namespace Importer.console.Domain;

public record Ingredient(string name, float quantity, Unit unit, string notes)
{
}