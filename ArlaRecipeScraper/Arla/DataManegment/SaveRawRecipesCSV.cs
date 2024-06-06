using System.Reflection;
using System.Text;
using RecipeWebScraper.Arla;

namespace RecipeWebScraper.Arla
{
    public static partial class DataManegement
    {
        public static void SaveRawRecipesCSV(string path, IEnumerable<RecipeSurrogate> rawRecpies)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                string header = $"{string.Join(',', typeof(RecipeSurrogate).GetFields(BindingFlags.Instance | BindingFlags.Public).Select((x) => x.Name))},";
                sw.WriteLine(header);

                foreach (RecipeSurrogate recipe in rawRecpies)
                {
                    string s =
                        $"{recipe.Name},{recipe.Link},{recipe.RecipeType},{recipe.TotalTimeMin},{recipe.IsFreezable},{recipe.Rating},{recipe.ImageLink},{recipe.IngrediantsAmount}";
                    sw.WriteLine(s);
                }
            }
        }
    }
}
