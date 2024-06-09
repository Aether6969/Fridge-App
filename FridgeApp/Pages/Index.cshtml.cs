using FrigeCore.Structures;
using static FrigeCore.HtmlGen;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using static FrigeCore.Server.Server;

namespace FridgeApp.Pages
{
    public class IndexModel : PageModel
    {
        public string IngredientSearchContent { get; set; } = "<select></select>";
        public string SearchTerm { get; set; } = "";
        public List<string> IngredientSearchResults { get; set; } = new List<string>();
        public List<(Recipe,int)> availableRecipes {get;set;} = new List<(Recipe,int)>();
        public string availableRecipesContent { get; set; } = string.Empty;
        public List<string> UserIngredientSearchResults { get; set; } = new List<string>();
        public string UserIngredientSavedContent { get; set; } = string.Empty;

        public void OnGet()
        {
            UserIngredientSavedContent = GetFridgeIngrediantsHtml();
        }
        public void OnPost()
        {
            if (Request.Form.ContainsKey("submitIngredientSearch"))
            {
                SearchTerm = Request.Form["searchIngredient"].ToString();
                IngredientSearchResults = SearchIngrediants(SearchTerm).Select((x) => x.Name).ToList();

                // Create a new select element
                StringBuilder sb = new StringBuilder();

                sb.Append("<select>");
                // Loop through the filtered items and create option elements
                foreach(string ingredient in IngredientSearchResults)
                {
                    sb.Append($"<option value='{ingredient}'>{ingredient}</option>");
                }
                sb.Append("</select>");

                IngredientSearchContent = sb.ToString();
            }
            if(Request.Form.ContainsKey("addIngredients"))
            {
                string AddTerm = Request.Form["selectedIngredient"].ToString();
                addIngredients(AddTerm);
            }   
            if(Request.Form.ContainsKey("findRecipes"))
            {
                availableRecipes = findRecipes().Select((x) => (x.Item1,x.Item2)).ToList();
                int recipesToShow = 3;
                for(int i = 0; i < Math.Min(availableRecipes.Count, recipesToShow); i++)
                {
                    (Recipe, int) recipe = availableRecipes[i];
                    availableRecipesContent += GetRecipeHtml(recipe.Item1, recipe.Item2);
                }
            }
            UserIngredientSavedContent = GetFridgeIngrediantsHtml();
        }
    }
}
