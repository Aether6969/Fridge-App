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

        public static IEnumerable<RecipeSurrogate> ScrapeArlaRecipes(WebDriver driver, IEnumerable<string> links)
        {
            foreach (string link in links)
            {
                yield return ScrapeArlaRecipe(driver, link);
            }
        }
        private static RecipeSurrogate ScrapeArlaRecipe(WebDriver driver, string link)
        {
            driver.Navigate().GoToUrl(link);

            //Gives the webpage time to load and prevents sending to many requests to fast
            Thread.Sleep(100);

            string xPathNa = "/html/body/div[1]/div[1]/div[2]/div[1]/div[2]/h1";
            IWebElement nameElement = driver.FindElement(By.XPath(xPathNa));
            string Name = nameElement.Text;

            string Link = link;

            string RecipeType = (string)driver.ExecuteScript("""return gtmData["recipeMealType"]""");

            string xPathTt = "/html/body/div[1]/div[1]/div[2]/div[1]/div[2]/div[3]/div[1]/div/span";
            string TotalTimeMin = "0";
            try
            {
                IWebElement totalTimeMinElement = driver.FindElement(By.XPath(xPathTt));
                TotalTimeMin = totalTimeMinElement.Text;
            }
            catch { }

            string xPathFr = "/html/body/div[1]/div[1]/div[2]/div[1]/div[2]/div[3]/div[1]/div[2]";
            bool isFreezable = ElementExists(driver, By.XPath(xPathFr));
            string IsFreezable = isFreezable.ToString();

            IWebElement ratingElement = driver.FindElement(By.ClassName("c-rating-static__selected"));
            string Rating = ratingElement.GetAttribute("style");

            string xPathImg = "/html/body/div[1]/div[1]/div[2]/div[1]/div[1]/picture/img";
            IWebElement imageElement = driver.FindElement(By.XPath(xPathImg));
            string ImageLink = imageElement.GetAttribute("src");

            string IngrediantsAmount = GetIngrediants(driver); //TODO: add " to start and end and remove ,()[] from ingrediant and bad

            return new RecipeSurrogate() 
            { 
                Name = Name,
                Link = link,
                RecipeType = RecipeType,
                TotalTimeMin = TotalTimeMin,
                IsFreezable = IsFreezable,
                Rating = Rating,
                ImageLink = ImageLink,
                IngrediantsAmount = IngrediantsAmount,
            };
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
