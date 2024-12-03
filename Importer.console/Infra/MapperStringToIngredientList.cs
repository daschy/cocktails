using System.Diagnostics;
using System.Text.RegularExpressions;
using Importer.console.Domain;
using Microsoft.VisualBasic.CompilerServices;

namespace Importer.console.Infra;

public class MapperStringToIngredientList
{
    public static Dictionary<string, DUnit> UnitMappings = new()
    {
        { "", DUnit.none },
        { "bag", DUnit.unit },
        { "barspoon", DUnit.tsp },
        { "cube", DUnit.cube },
        { "cupful", DUnit.cup },
        { "dash", DUnit.dash },
        { "disc", DUnit.peel },
        { "dried", DUnit.unit },
        { "drop", DUnit.drop },
        { "drops", DUnit.drop },
        { "fresh", DUnit.piece },
        { "gram", DUnit.grams },
        { "grind", DUnit.pinch },
        { "inch", DUnit.slice },
        { "knob", DUnit.twist },
        { "leaf", DUnit.leaf },
        { "pea", DUnit.piece },
        { "pinch", DUnit.pinch },
        { "pint", DUnit.pint },
        { "ring", DUnit.wheel },
        { "scoop", DUnit.wheel },
        { "segment", DUnit.slice },
        { "shot", DUnit.shot },
        { "slice", DUnit.slice },
        { "splash", DUnit.dash },
        { "sprig", DUnit.sprig },
        { "swath", DUnit.peel },
        { "twist", DUnit.twist },
        { "unit", DUnit.unit },
        { "wedge", DUnit.wedge },
        { "whole", DUnit.none }
    };

    public static DIngredient[] ParseIngredients(string input, string? recipeName)
    {
        try
        {
            var ingredients = new List<DIngredient>();
            var lines = input.Split("\n");
            foreach (var line in lines)
            {
                var parts = line.Split("\t");
                if (parts.Length >= 2)
                {
                    var quantityAndUnit = ParseQuantityAndUnit(parts[0], UnitMappings);

                    var nameAndNotes = ParseNameAndNotes(parts[1]);

                    var ingredient = new DIngredient(nameAndNotes.Item1,
                        quantityAndUnit.Item1,
                        quantityAndUnit.Item2,
                        nameAndNotes.Item2);
                    ingredients.Add(ingredient);
                }
            }

            return ingredients.ToArray();
        }
        catch (Exception ex)
        {
            Debug.Print($"Failed to parse ingredient list for {recipeName}");
            throw;
        }
    }

    public static (float, DUnit) ParseQuantityAndUnit(string part, Dictionary<string, DUnit> unitMappings)
    {
        var quantityAndUnitText = part.Trim();
        string quantityText;
        string unitText;

        if (quantityAndUnitText.ToLower().Contains("top up with"))
        {
            unitText = "shot";
            var result = (
                4f,
                unitMappings.First(
                    x => string.Equals(x.Key, unitText, StringComparison.OrdinalIgnoreCase)).Value
            );
            return result;
        }

        if (quantityAndUnitText.ToLower().Contains("float lightly whipped"))
        {
            return (
                1f,
                DUnit.piece
            );
        }

        if (quantityAndUnitText.ToLower().Contains("float"))
        {
            return (
                0.5f,
                DUnit.shot
            );
        }

        if (quantityAndUnitText.ToLower().Contains("1 grated zest of"))
        {
            return (
                1f,
                DUnit.peel
            );
        }

        quantityText = quantityAndUnitText.Split(" ")[0];
        unitText = quantityAndUnitText.Split(" ")[1];

        float quantity = quantityText.Contains("⁄")
            ? FractionToFloat(quantityText)
            : float.Parse(quantityText);

        var unit = DUnit.none;
        foreach (var unitMapping in unitMappings)
        {
            if (unitText.ToLower().Contains(unitMapping.Key))
            {
                unit = unitMapping.Value;
                unitText = unitText.Replace(unitMapping.Key, "").Trim();
                break;
            }
        }

        return (quantity, unit);
    }

    public static (string, string) ParseNameAndNotes(string part)
    {
        var nameAndNotesText = part.Trim();
        var nameText = nameAndNotesText.Split(" (")[0];
        var notes = nameAndNotesText.Split(" (").Length > 1
            ? nameAndNotesText.Split(" (")[1].Trim(')')
            : string.Empty;
        return (nameText, notes);
    }


    private static float FractionToFloat(string fraction)
    {
        var parts = fraction.Split('⁄');
        if (parts.Length == 2)
        {
            return float.Parse(parts[0]) / float.Parse(parts[1]);
        }

        return float.Parse(fraction);
    }
}