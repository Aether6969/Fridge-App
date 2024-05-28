using FrigeCore.Structures;
using static ArlaRecipeScraper.Arla.ArlaRecipeScraping;
using static ArlaRecipeScraper.Arla.ArlaRecipeLinksScraping;
using static ArlaRecipeScraper.DataManegement;
using ArlaRecipeScraper;

internal class Program
{
    private static void Main(string[] args)
    {

        bool scrapeRecipeLinks = false;
        bool scrapeRecipes = true;
        bool cleanRecipes = true;

        if (scrapeRecipeLinks)
        {
            string[] links = ScrapeRecipeLinks();
            SaveLinks(RecipeLinksPath, links);
        }

        if (scrapeRecipes)
        {
            IEnumerable<string> links = LoadLinks(RecipeLinksPath);

            links = ["https://www.arla.dk/opskrifter/pizzasnegle-med-skinke/"]; //TEST:
            RecipeSurrogate[] recipes = ScrapeArlaRecipes(links);

            SaveRawRecipesCSV(RawRecipesPath, recipes);
        }

        if (cleanRecipes)
        {
            IEnumerable<RecipeSurrogate> rawRecipes = LoadRawRecipesCSV(RawRecipesPath);

            IEnumerable<Recipe> recipes = default!;
        }

        Console.Beep();
    }
}