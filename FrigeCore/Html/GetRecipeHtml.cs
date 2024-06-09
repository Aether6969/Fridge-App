using FrigeCore.Structures;
using System.Text;

namespace FrigeCore
{
    public static partial class HtmlGen
    {
        public static string GetRecipeHtml(Recipe recipe, int fraction)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table class=\"recipes\">");
            // row 1: Total time + freezable | image
            sb.Append($"<tr><td style=\"color:blue\">{recipe.TotalTimeMin} MIN ");
            if (recipe.IsFreezable)
            {
              sb.Append("\n\n<font color=\"green\">KAN FRYSES</font>");
            }
            sb.Append("</td></tr>");
            if (recipe.ImageLink != null && recipe.ImageLink != string.Empty)
            {
              sb.Append($"<tr><td><img src=\"{recipe.ImageLink}\"/></td></tr>");
            }
            else
            {
              sb.Append("<tr><td><img src=\"https://www.arla.dk/UI/img/arla-logo.a7388293.svg\"/></td></tr>");
            }
            // row 2: Name + rating
            sb.Append($"<tr><td><a href=\"{recipe.Link}\">{recipe.Name}</a>\t Karakter: <font color=\"green\">{recipe.Rating} af 100</font></td></tr>");
            // row 3: fraction of ingredients available
            sb.Append($"<tr><td> Du har {fraction}% af ingredienserne</td></tr>");
            // row 4+: Ingredients of recipe
            foreach (Ingredient ingredient in recipe.IngrediantsAmount)
            {
              sb.Append($"<tr><td>{ingredient.Name}</td></tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }
    }
}
