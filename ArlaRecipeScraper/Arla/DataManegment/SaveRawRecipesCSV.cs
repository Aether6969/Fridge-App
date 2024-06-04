using System.Reflection;
using System.Text;

namespace RecipeWebScraper.Arla
{
    public static partial class DataManegement
    {
        public static void SaveRawRecipesCSV(string path, RecipeSurrogate[] rawRecpies)
        {
            StringBuilder sb = new StringBuilder();

            //TODO: rewrite (make static?)
            string header = $"{string.Join(',', typeof(RecipeSurrogate).GetFields(BindingFlags.Instance | BindingFlags.Public).Select((x) => x.Name))},";
            sb.AppendLine(header);

            for (int i = 0; i < rawRecpies.Length; i++)
            {
                RecipeSurrogate e = rawRecpies[i];

                //TODO: line to long, maybe use reflection
                string s = 
                    $"""
                    "{e.Name}","{e.Link}","{e.RecipeType}","{e.TotalTimeMin}","{e.IsFreezable}","{e.Rating}","{e.ImageLink}","{e.IngrediantsAmount}",
                    """;
                sb.AppendLine(s);
            }
            File.WriteAllText(path, sb.ToString());
        }
    }
}
