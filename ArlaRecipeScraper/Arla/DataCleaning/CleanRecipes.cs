using FrigeCore.Structures;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace RecipeWebScraper.Arla
{
    public static partial class DataCleaning
    {
        public static IEnumerable<Recipe> CleanRecipes(IEnumerable<RecipeSurrogate> recipes)
        {
            foreach (RecipeSurrogate recipe in recipes)
            {
                //TODO: make regex static
                string name = recipe.Name;

                string link = recipe.Link;

                RecipeType recipeType = ParseRecipeType(recipe.RecipeType);

                int totalTimeMin = ParseTotalTimeMin(recipe.TotalTimeMin);

                bool isFreezable = recipe.IsFreezable == "true";

                Regex numberRegex = new Regex(@"[0-9]+", RegexOptions.Compiled);
                int rating = int.Parse(numberRegex.Match(recipe.Rating).Value);

                string imageLink = recipe.ImageLink;

                Regex ingrediantRegex = new Regex(@"(?<=\()[^,]*,[^\)]*(?=\))", RegexOptions.Compiled);

                //TODO: bad
                string[] ingrediants = ingrediantRegex.Matches(recipe.IngrediantsAmount).Select((x) => x.Value).ToArray();
                RecipeIngredient[] ingredeantsAmount = new RecipeIngredient[ingrediants.Length];
                for (int i = 0; i < ingrediants.Length; i++)
                {
                    string ingrediant = ingrediants[i];

                    string ingName = ingrediant.Split(',')[0];
                    string ingAmount = ingrediant.Split(',')[1];
                    var v = numberRegex.Match(ingAmount).Value;
                    if (v == string.Empty)
                    {
                        v = "0";
                    }

                    Regex fracRegex = new Regex(@"[¼½¾⅐⅑⅒⅓⅔⅕⅖⅗⅘⅙⅚⅛⅜⅝⅞]", RegexOptions.Compiled);

                    string frac = fracRegex.Match(ingAmount).Value;

                    double amount = int.Parse(numberRegex.Match(v).Value) + GetRealFromVoulgarFReac(frac);

                    Regex unitRegex = new Regex(@"[\d ¼½¾⅐⅑⅒⅓⅔⅕⅖⅗⅘⅙⅚⅛⅜⅝⅞]*", RegexOptions.Compiled);
                    string unit = unitRegex.Replace(ingAmount, "");
                    if (unit == string.Empty)
                    {
                        unit = "stk";
                    }

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

        private static double GetRealFromVoulgarFReac(string frac)
        {
            switch (frac)
            {
                case "":
                    return 0;
                case "¼":
                    return 1.0 / 4.0;
                case "½":
                    return 1.0 / 2.0;
                case "¾":
                    return 3.0 / 4.0;
                case "⅐":
                    return 1.0 / 7.0;
                case "⅑":
                    return 1.0 / 9.0;
                case "⅒":
                    return 1.0 / 10.0;
                case "⅓":
                    return 1.0 / 3.0;
                case "⅔":
                    return 2.0 / 3.0;
                case "⅕":
                    return 1.0 / 5.0;
                case "⅖":
                    return 2.0 / 5.0;
                case "⅗":
                    return 3.0 / 5.0;
                case "⅘":
                    return 4.0 / 5.0;
                case "⅙":
                    return 1.0 / 6.0;
                case "⅚":
                    return 5.0 / 6.0;
                case "⅛":
                    return 1.0 / 8.0;
                case "⅜":
                    return 3.0 / 8.0;
                case "⅝":
                    return 5.0 / 8.0;
                case "⅞":
                    return 7.0 / 8.0;
                default:
                    return 0;
            }
        }
        private static RecipeType ParseRecipeType(string s)
        {
            Regex firstwordRegex = new Regex(@"^[a-zA-Z]*", RegexOptions.Compiled);
            string firstword = firstwordRegex.Match(s).Value;

            switch (firstword)
            {
                case "Hovedret":
                    return RecipeType.Hovedret;
                case "Dessert":
                    return RecipeType.Dessert;
                case "Tilbehør":
                    return RecipeType.Tilbehør;
                case "Mellemmåltid":
                    return RecipeType.Mellemmåltid;
                case "Frokost":
                    return RecipeType.Frokost;
                case "Forret":
                    return RecipeType.Forret;
                case "Madpakke":
                    return RecipeType.Madpakke;
                case "Morgenmad":
                    return RecipeType.Morgenmad;
                case "Ost":
                    return RecipeType.Ost;
                default:
                    return RecipeType.Unknown;
            }
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
