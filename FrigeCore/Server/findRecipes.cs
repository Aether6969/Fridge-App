using FrigeCore.Structures;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FrigeCore.Server
{
    public static partial class Server
    {
        public static List<Tuple<Recipe,int>> findRecipes()
        {
            List<Tuple<Recipe,int>>  matches = new List<Tuple<Recipe,int>>();
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                FileInfo createTablesFile = new FileInfo("../Scripts/findRecipes.sql");
                string createTablesScript = createTablesFile.OpenText().ReadToEnd();
                createTablesScript = createTablesScript.Replace("@fridge", "3");
                using (NpgsqlCommand searchCommand = dataSource.CreateCommand(createTablesScript))
                {
                    using (NpgsqlDataReader reader = searchCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Recipe recipe= new Recipe()
                            {
                                Name=reader.GetString(4),
                                Link=reader.GetString(5),
                                RecipeType=Enum.Parse<RecipeType>(reader.GetString(6)),
                                TotalTimeMin=reader.GetInt32(7),
                                IsFreezable=reader.GetBoolean(8),
                                Rating=reader.GetInt32(9),
                                ImageLink= reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                IngrediantsAmount= new Ingredient[0],
                            };
                        string recipeIngredients = "SELECT ingredient,amount,unit FROM recipeingredients WHERE recipe = (@recipe)";
                        List<Ingredient> ingredients = new List<Ingredient> ();
                        using (NpgsqlCommand searchCommand1 = dataSource.CreateCommand(recipeIngredients))
                        {
                            searchCommand1.Parameters.Add(new("@recipe",recipe.Name));
                            using (NpgsqlDataReader reader1 = searchCommand1.ExecuteReader())
                            {
                                while (reader1.Read())
                                {
                                    Ingredient ingredient = new Ingredient()
                                    {
                                        Name=reader1.GetString(0),
                                        Amount=reader1.GetDouble(1),
                                        Unit=reader1.GetString(2), 
                                    };
                                    ingredients.Add(ingredient);
                                }
                            }
                        }
                        Ingredient[] ingredientss = ingredients.ToArray();
                        recipe.IngrediantsAmount=ingredientss;
                        matches.Add(Tuple.Create(recipe, (int)reader.GetDouble(3)));
                        }
                    }
                }
            }
            return matches;
        }
    }
}
