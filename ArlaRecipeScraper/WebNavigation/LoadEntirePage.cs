using OpenQA.Selenium;

namespace ArlaRecipeScraper
{
    public static partial class WebNavigation
    {
        public static void LoadEntirePage(WebDriver driver)
        {
            long lastHeight = 0;
            while (true)
            {
                driver.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

                Thread.Sleep(5000);

                var newHeight = (long)driver.ExecuteScript("return document.body.scrollHeight");

                if (lastHeight == newHeight)
                {
                    break;
                }

                lastHeight = newHeight;
            }
        }
    }
}
