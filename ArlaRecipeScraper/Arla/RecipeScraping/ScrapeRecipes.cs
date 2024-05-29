using static RecipeWebScraper.WebNavigation;
using OpenQA.Selenium;
using System.Text.RegularExpressions;

namespace RecipeWebScraper.Arla
{
    public static partial class RecipeScraping
    {
        private static Regex _ingNameReg = new Regex(@"(?<=u-text-lowercase..>)[^<]*", RegexOptions.Compiled);
        private static Regex _ingAmountReg = new Regex(@"(?<=u-mr--xxs..>)[^<]*", RegexOptions.Compiled);

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

            //Give the webpage time to load and prevent sending to many requests at to fast
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
                //TODO: is bad also consider selenium api if plausable
                string ingrediantHtml = ingrediantElements[i].GetAttribute("innerHTML");

                var hc = new string(ingrediantHtml.SelectMany<char, char>((x) => x == '"' ? ['\\', '"'] : [x]).ToArray());

                Match match = _ingNameReg.Match(hc);

                string name = string.Empty;
                string amount = string.Empty;

                if (match.Success)
                {
                    name = match.Value.Trim();
                    amount = _ingAmountReg.Match(hc).Value.Trim();
                }
                else
                {
                    name = ingrediantElements[i].Text.Trim();
                }

                ingrediantAmounts[i] = (name, amount);
            }
            recipe.IngrediantsAmount = $"""[{string.Join(',', ingrediantAmounts.Select((x) => x.ToString()))}]"""; //TODO: add " to start and end and bad

            string xPathImg = "/html/body/div[1]/div[1]/div[2]/div[1]/div[1]/picture/img";
            IWebElement imageElement = driver.FindElement(By.XPath(xPathImg));
            recipe.ImageLink = imageElement.GetAttribute("src");

            return recipe;
        }
    }
}
