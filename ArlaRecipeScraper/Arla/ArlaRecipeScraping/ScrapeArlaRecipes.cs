using static ArlaRecipeScraper.WebNavigation;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace ArlaRecipeScraper.Arla
{
    public static partial class ArlaRecipeScraping
    {
        public static RecipeSurrogate[] ScrapeArlaRecipes(IEnumerable<string> links)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("headless");

            using (WebDriver driver = new ChromeDriver(options))
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                List<RecipeSurrogate> recipes = [];
                foreach (string link in links)
                {
                    RecipeSurrogate recipeInfo = ScrapeArlaRecipe(driver, link);

                    recipes.Add(recipeInfo);
                }

                return recipes.ToArray();
            }
        }
        private static RecipeSurrogate ScrapeArlaRecipe(WebDriver driver, string link)
        {
            driver.Navigate().GoToUrl(link);

            //Give the webpage time to load and prevent sending to many requests at once
            Thread.Sleep(100);

            RecipeSurrogate recipe = new RecipeSurrogate();

            string xPathNa = "/html/body/div[1]/div[1]/div[2]/div[1]/div[2]/h1";
            IWebElement nameElement = driver.FindElement(By.XPath(xPathNa));
            recipe.Name = nameElement.Text;

            recipe.Link = link;

            recipe.RecipeType = (string)driver.ExecuteScript("""return gtmData["recipeMealType"]""");

            string xPathTt = "/html/body/div[1]/div[1]/div[2]/div[1]/div[2]/div[3]/div[1]/div/span";
            IWebElement totalTimeMinElement = driver.FindElement(By.XPath(xPathTt));
            recipe.TotalTimeMin = totalTimeMinElement.Text;

            string xPathFr = "/html/body/div[1]/div[1]/div[2]/div[1]/div[2]/div[3]/div[1]/div[2]";
            bool isFreezable = ElementExists(driver, By.XPath(xPathFr));
            recipe.IsFreezable = isFreezable.ToString();

            string xPathRat = "/html/body/div[1]/div[1]/div[2]/div[1]/div[2]/div[3]/div[2]/div/div/div/div/div[2]";
            IWebElement ratingElement = driver.FindElement(By.XPath(xPathRat));
            recipe.Rating = ratingElement.GetAttribute("style");

            //TODO: decease number of persons that amounts are scaled by
            string xPathIt = "/html/body/div[1]/div[1]/div[3]/div[2]/div[1]/div[2]";
            IWebElement ingrediantsTableElement = driver.FindElement(By.XPath(xPathIt));
            IWebElement[] ingrediantElements = ingrediantsTableElement.FindElements(By.ClassName("u-width-70")).ToArray();
            (string, string)[] ingrediantAmounts = new (string, string)[ingrediantElements.Length];
            for (int i = 0; i < ingrediantAmounts.Length; i++)
            {
                //IWebElement ingrediantNameElement = ingrediantElements[i].FindElement(By.ClassName("u-mr--xxs"));

                //TODO: figure out how to grab name and amount seperatly
                string name = ingrediantElements[i].Text;
                string amount = "";

                ingrediantAmounts[i] = (name, amount);
            }
            recipe.IngrediantsAmount = $"[{string.Join(',', ingrediantAmounts.Select((x) => x.ToString()))}]"; //TODO:

            string xPathImg = "/html/body/div[1]/div[1]/div[2]/div[1]/div[1]/picture/img";
            IWebElement imageElement = driver.FindElement(By.XPath(xPathImg));
            recipe.ImageLink = imageElement.GetAttribute("src");

            recipe.EnergyKj = ""; //TODO: oh oh

            recipe.NutritionalInfo = ""; //TODO:

            return recipe;
        }
    }
}
