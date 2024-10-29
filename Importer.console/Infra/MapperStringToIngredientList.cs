using System.Text.RegularExpressions;
using Importer.console.Domain;

namespace Importer.console.Infra;

public class MapperStringToIngredientList
{
    public static List<DIngredient> ParseIngredients(string input)
    {
        var ingredients = new List<DIngredient>();
        var unitMappings = new Dictionary<string, DUnit>
        {
            { "shot", DUnit.shot },
            { "ml", DUnit.ml },
            { "oz", DUnit.oz },
            { "cl", DUnit.cl },
            { "fresh", DUnit.leaf },
            { "leaf", DUnit.leaf },
            { "leaves", DUnit.leaf },
            { "barspoon", DUnit.tsp },
        };
        var lines = input.Split("\n");
        foreach (var line in lines)
        {
            var parts = line.Split("\t");
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

                var ingredient = new DIngredient(nameText, quantity, unit, notes);
                ingredients.Add(ingredient);
            }
        }

        return ingredients;
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