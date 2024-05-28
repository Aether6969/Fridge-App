using System.Text;

namespace ArlaRecipeScraper
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
                string[] attributes = line.Split(',').Select((x) => x.Trim()).ToArray();

                RecipeSurrogate item = new RecipeSurrogate()
                {
                    Name                = attributes[0],
                    Link                = attributes[1],
                    RecipeType          = attributes[2],
                    TotalTimeMin        = attributes[3],
                    IsFreezable         = attributes[4],
                    Rating              = attributes[5],
                    IngrediantsAmount   = attributes[6],
                    ImageLink           = attributes[7],
                    EnergyKj            = attributes[8],
                    NutritionalInfo     = attributes[9],
                };

                yield return item;
            }
        }
    }
}
