using System.Text;
using System.Text.RegularExpressions;

namespace RecipeWebScraper.Arla
{
    public static partial class DataManegement
    {
        public static IEnumerable<RecipeSurrogate> LoadRawRecipesCSV(string path)
        {
            IEnumerable<string> lines = File.ReadLines(path);

            //NOTE: we are assuming that attributes are consistant and in same order as type
            //string collumnHeader = lines.First();

            foreach (string line in lines.Skip(1))
            {
                //TODO: insted of spilt match between  
                // TODO: Do we read the IngrediantsAmount correctly here?
                //TODO: bad
                // in the csv file they are organized as [(Gær, 50 g),(Vand, 3 dl),...]
                string[] attributes = line.Split(',').Select((x) => x.Trim()).ToArray();

                Regex ingrediantsRegex = new Regex(@"\[[^\]]*\]");
                string IngrediantsAmount = ingrediantsRegex.Match(line).Value;


                RecipeSurrogate item = new RecipeSurrogate()
                {
                    Name                = attributes[0],
                    Link                = attributes[1],
                    RecipeType          = attributes[2],
                    TotalTimeMin        = attributes[3],
                    IsFreezable         = attributes[4],
                    Rating              = attributes[5],
                    ImageLink           = attributes[6],
                    IngrediantsAmount   = IngrediantsAmount,
                };

                yield return item;
            }
        }
    }
}
