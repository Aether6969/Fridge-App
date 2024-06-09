using System.Text;
using static FrigeCore.Server.Server;
namespace FrigeCore
{
    public static partial class HtmlGen
    {
        public static string GetFridgeIngrediantsHtml()
        {
            List<string> UserIngredientSearchResults = GetFridgeIngrediants("Bombur").Select((x) => x.Name).ToList();

            StringBuilder sb = new StringBuilder();

            sb.Append("<table class=\"center\">");
            const int ingredientsPerRow = 3;
            for (int i = 0; i < UserIngredientSearchResults.Count; i++)
            {
                string ingredient = UserIngredientSearchResults[i];
                switch (i % ingredientsPerRow)
                {
                    case 0:
                        sb.Append($"<tr><td>{ingredient}</td>");
                        break;
                    case 1:
                        sb.Append($"<td>{ingredient}</td>");
                        break;
                    case 2:
                        sb.Append($"<td>{ingredient}</td></tr>");
                        break;
                }
            }
            if (UserIngredientSearchResults.Count % ingredientsPerRow != 0)
            {
              sb.Append("</tr>");
            }
            sb.Append("</table>");

            return sb.ToString();
        }
  }
}
