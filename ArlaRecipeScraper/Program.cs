using FrigeCore.Structures;
using RecipeWebScraper.Arla;
using static RecipeWebScraper.Arla.RecipeScraping;
using static RecipeWebScraper.Arla.RecipeLinksScraping;
using static RecipeWebScraper.Arla.DataManegement;
using static RecipeWebScraper.Arla.DataCleaning;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Text.RegularExpressions;
using System.Globalization;

internal class Program
{
    //TODO: move
    private static WebDriver driver = SetupDriver();
    private static WebDriver SetupDriver()
    {
        ChromeOptions options = new ChromeOptions();
        //Makes the chrome instance run as background task
        options.AddArgument("headless");
        options.ImplicitWaitTimeout = TimeSpan.FromSeconds(10);
        options.PageLoadTimeout = TimeSpan.FromSeconds(10);
        
        ChromeDriverService service = ChromeDriverService.CreateDefaultService();
        service.InitializationTimeout = TimeSpan.FromSeconds(10);
        service.HideCommandPromptWindow = true;
        
        //Makes sure the browser and driver gets closed when this application closes 
        AppDomain.CurrentDomain.ProcessExit += new EventHandler(CloseDriver);

        return new ChromeDriver(service, options);
    }
    private static void CloseDriver(object? sender, EventArgs e)
    {
        driver?.Quit();
    }

    private static void Main(string[] args)
    {
        bool scrapeRecipeLinks = false;
        bool scrapeRecipes = false;
        bool cleanRecipes = false;

        if (scrapeRecipeLinks)
        {
            string[] links = ScrapeRecipeLinks(driver);
            SaveLinks(RecipeLinksPath, links);
        }
        
        if (scrapeRecipes)
        {
            IEnumerable<string> links = LoadLinks(RecipeLinksPath);

            IEnumerable<RecipeSurrogate> recipes = ScrapeArlaRecipes(driver, links);

            SaveRawRecipesCSV(RawRecipesPath, recipes);
        }

        if (cleanRecipes)
        {
            IEnumerable<RecipeSurrogate> rawRecipes = LoadRawRecipesCSV(RawRecipesPath);

            Recipe[] recipes = CleanRecipes(rawRecipes).ToArray();

            SaveCleanRecipesCSV(CleanRecipesPath, recipes);
            SaveCleanRecipeIngrediantsCSV(CleanIngrediantsPath, recipes);
        }

        Console.Beep();
    }

    public static void SaveCleanRecipesCSV(string path, IEnumerable<Recipe> Recpies)
    {
        using (StreamWriter sw = new StreamWriter(path))
        {
            string header = $"Name,Link,RecipeType,TotalTimeMin,IsFreezable,Rating,ImageLink";
            sw.WriteLine(header);

            foreach (Recipe recipe in Recpies)
            {
                string s =
                    $"{recipe.Name},{recipe.Link},{recipe.RecipeType},{recipe.TotalTimeMin},{recipe.IsFreezable},{recipe.Rating},{recipe.ImageLink}";
                sw.WriteLine(s);
            }
        }
    }
    public static void SaveCleanRecipeIngrediantsCSV(string path, IEnumerable<Recipe> rawRecipes)
    {
        using (StreamWriter sw = new StreamWriter(path))
        {
            string header = $"Recipe,Name,Amount,Unit";
            sw.WriteLine(header);

            foreach (Recipe recipe in rawRecipes)
            {
                foreach (Ingredient ingrediant in recipe.IngrediantsAmount)
                {
                    //TODO: move to igrediant tostring
                    string s =
                        $"{recipe.Name},{ingrediant.Name},{ingrediant.Amount.ToString(System.Globalization.CultureInfo.InvariantCulture)},{ingrediant.Unit}";
                    sw.WriteLine(s);
                }
            }
        }
    }

}