using System.Text.RegularExpressions;
using Importer.console.Domain;

namespace Importer.console.Infra;

public class MapperStringToIngredientList
{
    public static List<Ingredient> ParseIngredients(string input)
    {
        var ingredients = new List<Ingredient>();
        var unitMappings = new Dictionary<string, Unit>
        {
            { "shot", Unit.shot },
            { "ml", Unit.ml },
            { "oz", Unit.oz },
            { "cl", Unit.cl },
            { "fresh", Unit.leaf },
            { "leaf", Unit.leaf },
            { "leaves", Unit.leaf },
            { "barspoon", Unit.tsp },
        };
        var lines = input.Split("\\n");
        foreach (var line in lines)
        {
            var parts = line.Split("\\t");
            if (parts.Length >= 2)
            {
                var quantityAndUnitText = parts[0].Trim();
                var quantityText = quantityAndUnitText.Split(" ")[0];
                var unitText = quantityAndUnitText.Split(" ")[1];
                var nameAndNotesText = parts[1].Trim();
                var nameText = nameAndNotesText.Split(" (")[0];
                var notes = nameAndNotesText.Split(" (").Length > 1
                    ? nameAndNotesText.Split(" (")[1].Trim(')')
                    : string.Empty;
                float quantity = quantityText.Contains("⁄") ? FractionToFloat(quantityText) : float.Parse(quantityText);

                var unit = Unit.none;
                foreach (var unitMapping in unitMappings)
                {
                    if (unitText.ToLower().Contains(unitMapping.Key))
                    {
                        unit = unitMapping.Value;
                        unitText = unitText.Replace(unitMapping.Key, "").Trim();
                        break;
                    }
                }

                var ingredient = new Ingredient(nameText, quantity, unit, notes);
                ingredients.Add(ingredient);
            }
        }

        return ingredients;
    }

    public static IEnumerable<Ingredient> Map(string input)
    {
        var ingredients = new List<Ingredient>();
        var unitMappings = new Dictionary<string, Unit>
        {
            { "shot", Unit.shot },
            { "ml", Unit.ml },
            { "oz", Unit.oz },
            { "cl", Unit.cl },
            { "leaves", Unit.leaf },
            { "leaf", Unit.leaf }
        };

        var lines = input.Split("\n");
        foreach (var line in lines)
        {
            // var ingredient = parseLine(line);
        }

        return ingredients;
    }

    // private static Ingredient parseLine(string line)
    // {
    //     var part = line.Split("\t");
    //     return new Ingredient()
    // }

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