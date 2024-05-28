using static ArlaRecipeScraper.WebNavigation;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace ArlaRecipeScraper.Arla
{
    public static partial class ArlaRecipeLinksScraping
    {
        public static string[] ScrapeRecipeLinks()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("headless");

            using (WebDriver driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl(ArlaRecipesBaseUrl);

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

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
}
