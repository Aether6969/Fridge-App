using FrigeCore.Structures;
using static RecipeWebScraper.Arla.RecipeScraping;
using static RecipeWebScraper.Arla.RecipeLinksScraping;
using static RecipeWebScraper.DataManegement;
using static RecipeWebScraper.Arla.DataCleaning;
using RecipeWebScraper;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

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
        bool scrapeRecipes = true;
        bool cleanRecipes = true;

        if (scrapeRecipeLinks)
        {
            string[] links = ScrapeRecipeLinks(driver);
            SaveLinks(RecipeLinksPath, links);
        }

        if (scrapeRecipes)
        {
            IEnumerable<string> links = LoadLinks(RecipeLinksPath);

            links = ["https://www.arla.dk/opskrifter/pizzasnegle-med-skinke/"]; //TEST:
            RecipeSurrogate[] recipes = ScrapeArlaRecipes(driver, links);

            SaveRawRecipesCSV(RawRecipesPath, recipes);
        }

        if (cleanRecipes)
        {
            IEnumerable<RecipeSurrogate> rawRecipes = LoadRawRecipesCSV(RawRecipesPath);

            IEnumerable<Recipe> recipes = CleanRecipes(rawRecipes);
        }

        Console.Beep();
    }
}