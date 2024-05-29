using OpenQA.Selenium;

namespace RecipeWebScraper.Arla
{
    public static partial class DataManegement
    {
        public static IEnumerable<string> LoadLinks(string path)
        {
            return File.ReadAllLines(path);
        }
    }
}
