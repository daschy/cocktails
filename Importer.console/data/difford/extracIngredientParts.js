const fs = require('fs');

function readFileAndExtractQuantity(file) {
    try {
        const data = fs.readFileSync(file, 'utf8');
        const jsonData = JSON.parse(data); // Parse JSON string
        const unitSet = new Set();
        const quantitySet = new Set();
        const quantityAndUnitSet = new Set();
        const ingredientSet = new Set();
        const glassSet = new Set();
        jsonData.forEach(recipe => {
            const quantityAndUnitPartList = `${recipe.ingredients}`.split('\n')
                .map(parts => `${parts}`.split('\t')[0])

            quantityAndUnitPartList
                .map(quantityAndUnit => `${quantityAndUnit}`.split(" ").slice(-1))
                .forEach(unit => unitSet.add(`${unit[0]}`.toLowerCase().trim()));

            quantityAndUnitPartList
                .map(quantityAndUnit => `${quantityAndUnit}`.split(" ").slice(0, -1))
                .forEach(quantity => quantitySet.add(`${quantity.join(" ")}`.toLowerCase().trim()));

            quantityAndUnitPartList
                .forEach(quantityAndUnit => quantityAndUnitSet.add(`${quantityAndUnit}`.toLowerCase().trim()));

            const recipeIngredientList = `${recipe.ingredients}`.split('\n')
                .map(parts => `${parts}`.split('\t')[1])
                .map(parts => `${parts}`.split('(')[0])
            recipeIngredientList
                .forEach(ingr => ingredientSet.add(`${ingr}`.toLowerCase().trim()));

            glassSet.add(`${recipe.glass}`.toLowerCase().trim());

        })
        const quantityAndUnitSetJson = JSON.stringify(Array.from(quantityAndUnitSet).sort(), null, 3);
        fs.writeFileSync('./set_quantityAndUnit.json', quantityAndUnitSetJson);
        const unitSetJson = JSON.stringify(Array.from(unitSet).sort(), null, 3);
        fs.writeFileSync('./set_unit.json', unitSetJson);
        const ingredientSetJson = JSON.stringify(Array.from(ingredientSet).sort(), null, 3);
        fs.writeFileSync('./set_ingredient.json', ingredientSetJson);
        const glassSetJson = JSON.stringify(Array.from(glassSet).sort(), null, 3);
        fs.writeFileSync('./set_glass.json', glassSetJson);
        return;

    } catch (error) {
        console.error('Error reading the file:', error);
    }
}


readFileAndExtractQuantity("./details_cocktails_all_ml.json")