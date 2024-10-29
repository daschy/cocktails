namespace Importer.console.Domain;

public record DIngredient(
    string? Name = null,
    float Quantity = default,
    DUnit Unit = DUnit.none,
    string? Notes = default)
{
}