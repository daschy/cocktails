// See https://aka.ms/new-console-template for more information

using Importer.console.Infra;

var diffordImporter = new JsonImporterDiffordRepository();
var dir = "data/difford";
var recipes = diffordImporter.readFromFile($"{dir}/details_cocktails_all.json");
// diffordImporter.writeToFile(recipes, $"{dir}/cocktails_with_ingredients.json");