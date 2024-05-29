using OpenQA.Selenium;

namespace RecipeWebScraper
{
    public static partial class WebNavigation
    {
        public static bool ElementExists(WebDriver driver, By by)
        {
            //TODO: possibly redesigen to use findelements.count = 0
            bool exists = false;
            try
            {
                _ = driver.FindElement(by);
                exists = true;
            }
            catch { }

            return exists;
        }
    }
}
