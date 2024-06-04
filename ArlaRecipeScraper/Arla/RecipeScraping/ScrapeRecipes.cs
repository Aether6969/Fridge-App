using static RecipeWebScraper.WebNavigation;
using OpenQA.Selenium;
using System.Text.RegularExpressions;

namespace RecipeWebScraper.Arla
{
    public static partial class RecipeScraping
    {
        private static readonly Regex _ingNameReg = new Regex(@"(?<=u-text-lowercase..>)[^<]*", RegexOptions.Compiled);
        private static readonly Regex _ingAmountReg = new Regex(@"(?<=u-mr--xxs..>)[^<]*", RegexOptions.Compiled);

        private static readonly string fcQuote = new string(new char[] { '\\', '"' });

        public static RecipeSurrogate[] ScrapeArlaRecipes(WebDriver driver, IEnumerable<string> links)
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
        private static RecipeSurrogate ScrapeArlaRecipe(WebDriver driver, string link)
        {
            driver.Navigate().GoToUrl(link);

            //TODO: Set number of persons to 1

            //Give the webpage time to load and prevent sending to many requests to fast
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

            string xPathImg = "/html/body/div[1]/div[1]/div[2]/div[1]/div[1]/picture/img";
            IWebElement imageElement = driver.FindElement(By.XPath(xPathImg));
            recipe.ImageLink = imageElement.GetAttribute("src");

            recipe.IngrediantsAmount = GetIngrediants(driver); //TODO: add " to start and end and remove ,()[] from ingrediant and bad

            return recipe;
        }
        private static string GetIngrediants(WebDriver driver)
        {
            //TODO: set portion isze to 1
            string xPathIt = "/html/body/div[1]/div[1]/div[3]/div[2]/div[1]/div[2]";
            IWebElement ingrediantsTableElement = driver.FindElement(By.XPath(xPathIt));

            IWebElement[] ingrediantElements = ingrediantsTableElement.FindElements(By.ClassName("u-width-70")).ToArray();

            (string name, string amount)[] ingrediantAmounts = new (string, string)[ingrediantElements.Length];
            for (int i = 0; i < ingrediantAmounts.Length; i++)
            {
                //TODO: is bad also consider selenium api if plausable
                string ingrediantHtml = ingrediantElements[i].GetAttribute("innerHTML");

                var hc = ingrediantHtml.Replace('"'.ToString(), fcQuote);

                ingrediantAmounts[i] = GetNameAndAmount(ingrediantElements[i], hc);
            }
            string s = $"[{string.Join(',', ingrediantAmounts.Select(FormatIngediant))}]";
            return s;

            //TODO:
            static string FormatIngediant((string name, string amount) x)
            {
                //TODO:make static
                Regex regex = new Regex(@"[,()\[\]]", RegexOptions.Compiled);
                return $"({regex.Replace(x.name, string.Empty)},{regex.Replace(x.amount, string.Empty)})";
            }
        }
        private static (string, string) GetNameAndAmount(IWebElement ingrediantElement, string hc)
        {
            Match match = _ingNameReg.Match(hc);

            if (match.Success)
            {
                //If the name element is present then so is the amount element
                string name = match.Value.Trim();
                string amount = _ingAmountReg.Match(hc).Value.Trim();

                return (name, amount);
            }

            //If name element is not present then the entire element contains only the name
            return (ingrediantElement.Text.Trim(), string.Empty);
        }
    }
}
