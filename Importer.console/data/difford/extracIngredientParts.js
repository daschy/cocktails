const fs = require('fs');

function readFileAndExtractQuantity(file) {
    try {
        const data = fs.readFileSync(file, 'utf8');
        const jsonData = JSON.parse(data); // Parse JSON string
        const unitSet = new Set();
        const quantitySet = new Set();
        const quantityAndUnitSet = new Set();
        jsonData.forEach(recipe => {
            const quantityPartList = recipe.ingredients.split('\n')
                .map(parts => parts.split('\t')[0])
            quantityPartList
                .map(quantityAndUnit => quantityAndUnit.split(" ").slice(-1))
                .forEach(unit =>
                    unitSet.add(unit[0])
                );
            quantityPartList
                .map(quantityAndUnit => quantityAndUnit.split(" ").slice(0, -1))
                .forEach(quantity =>
                    quantitySet.add(quantity.join(" "))
                );
            quantityPartList
                .forEach(quantityAndUnit =>
                    quantityAndUnitSet.add(quantityAndUnit)
                );
        })
        const quantityAndUnitSetJson = JSON.stringify(Array.from(quantityAndUnitSet).map(i => i.toLowerCase()).sort(), null, 3);
        fs.writeFileSync('./quantityAndUnitSet.json', quantityAndUnitSetJson);
        const unitSetJson = JSON.stringify(Array.from(unitSet).map(i => i.toLowerCase()).sort(), null, 3);
        fs.writeFileSync('./unitSet.json', unitSetJson);

    } catch (error) {
        console.error('Error reading the file:', error);
    }
}


readFileAndExtractQuantity("./details_cocktails_all.json")