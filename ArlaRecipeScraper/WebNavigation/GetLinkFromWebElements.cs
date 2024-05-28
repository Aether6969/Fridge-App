using OpenQA.Selenium;

namespace ArlaRecipeScraper
{
    public static partial class WebNavigation
    {
        public static string[] GetLinkFromWebElements(IWebElement[] elements)
        {
            List<string> links = [];
            for (int i = 0; i < elements.Length; i++)
            {
                IWebElement e = elements[i];

                string? link = e.GetAttribute("href");

                if (!string.IsNullOrWhiteSpace(link))
                {
                    links.Add(link!);
                }
            }

            return links.ToArray();
        }
    }
}
