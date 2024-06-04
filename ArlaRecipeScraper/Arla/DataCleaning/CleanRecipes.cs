using FrigeCore.Structures;
using System.Text.RegularExpressions;

namespace RecipeWebScraper.Arla
{
    public static partial class DataCleaning
    {
        public static IEnumerable<Recipe> CleanRecipes(IEnumerable<RecipeSurrogate> recipes)
        {
            //TEST:



            foreach (var recipe in recipes)
            {
                //TODO: make regex static
                string name = recipe.Name;

                string link = recipe.Link;

                RecipeType recipeType = ParseRecipeType(recipe.RecipeType);

                int totalTimeMin = ParseTotalTimeMin(recipe.TotalTimeMin);

                bool isFreezable = recipe.IsFreezable == "true";

                Regex numberRegex = new Regex(@"[0-9]+", RegexOptions.Compiled);
                int rating = int.Parse(numberRegex.Match(recipe.Rating).Value);

                Regex imageLinkRegex = new Regex(@"https://images.arla.com/recordid/.+?(?=\?)", RegexOptions.Compiled);
                string imageLink = imageLinkRegex.Match(recipe.ImageLink).Value;

                Regex ingrediantRegex = new Regex(@"\([^,]*,[^\)]*\)", RegexOptions.Compiled);

                //TODO: bad
                string[] ingrediants = ingrediantRegex.Matches(recipe.IngrediantsAmount).Select((x) => x.Value).ToArray();
                RecipeIngredient[] ingredeantsAmount = new RecipeIngredient[ingrediants.Length];
                for (int i = 0; i < ingrediants.Length; i++)
                {
                    string ingrediant = ingrediants[i];

                    string ingName = ingrediant.Split(',')[0];
                    string ingAmount = ingrediant.Split(',')[1];

                    int amount = int.Parse(numberRegex.Match(ingAmount == string.Empty ? "0" : ingAmount).Value);

                    Regex unitRegex = new Regex(@"[a-zA-Z]*", RegexOptions.Compiled);
                    string unit = unitRegex.Match(ingName).Value;

                    ingredeantsAmount[i] = new RecipeIngredient() 
                    { 
                        Name = ingName, 
                        Amount = amount, 
                        Unit = unit
                    };
                }

                yield return new Recipe()
                {
                    Name = name,
                    Link = link,
                    RecipeType = recipeType,
                    TotalTimeMin = totalTimeMin,
                    IsFreezable = isFreezable,
                    Rating = rating,
                    ImageLink = imageLink,
                    IngrediantsAmount = ingredeantsAmount,
                };
            }
        }
        private static RecipeType ParseRecipeType(string s)
        {
            return default;
        }
        private static int ParseTotalTimeMin(string s)
        {
            //TODO: make regex static
            Regex numberRegex = new Regex(@"[0-9]+", RegexOptions.Compiled);

            Regex isHourRegex = new Regex(@"TIME", RegexOptions.Compiled);
            Match match = isHourRegex.Match(s);

            if (match.Success)
            {
                return int.Parse(numberRegex.Match(s).Value) * 60;
            }

            return int.Parse(numberRegex.Match(s).Value);
        }
    }
}
