using static RecipeWebScraper.WebNavigation;
using OpenQA.Selenium;

namespace RecipeWebScraper.Arla
{
    public static partial class RecipeLinksScraping
    {
        public static string[] ScrapeRecipeLinks(WebDriver driver)
        {
            driver.Navigate().GoToUrl(ArlaRecipesBaseUrl);

            LoadEntirePage(driver);

            string boxClass = "o-grid";
            IWebElement box = driver.FindElement(By.ClassName(boxClass));

            string cardClass = "u-flex";
            IWebElement[] elements = box.FindElements(By.ClassName(cardClass)).ToArray();
            string[] links = GetLinkFromWebElements(elements);

            return links;
        }
    }
}
