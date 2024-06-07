using FrigeCore.Structures;
using Npgsql;

namespace FrigeCore.Server
{
    public static partial class Server
    {
        public static List<Recipe> GetRecipes(NpgsqlDataSource dataSource, string sqlquery)
        {
            using (NpgsqlCommand cmd = dataSource.CreateCommand(sqlquery))
            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                List<Recipe> ret = new List<Recipe>();
                while (reader.Read())
                {
                    string recipeName = reader.GetString(0);

                    Recipe recipe = new Recipe()
                    {
                        Name = recipeName,
                        Link = reader.GetString(1),
                        RecipeType = Enum.Parse<RecipeType>(reader.GetString(2)),
                        TotalTimeMin = reader.GetInt32(3),
                        IsFreezable = reader.GetBoolean(4),
                        Rating = reader.GetInt32(5),
                        ImageLink = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                        IngrediantsAmount = GetRecipeIngrediants(dataSource, recipeName),
                    };
                    ret.Add(recipe);
                }

                return ret;
            }
        }
        private static Ingredient[] GetRecipeIngrediants(NpgsqlDataSource dataSource, string recipe)
        {
            string query =
                $"SELECT ingredient, amount, unit\r\nFROM recipeingredients\r\nWHERE Recipe = (@recipe);";

            using (NpgsqlCommand cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.Add(new("recipe", recipe));

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    List<Ingredient> ret = new List<Ingredient>();
                    while (reader.Read())
                    {
                        Ingredient ingrediant = new Ingredient()
                        {
                            Name = reader.GetString(0),
                            Amount = reader.GetDouble(1),
                            Unit = reader.GetString(2),
                        };
                        ret.Add(ingrediant);
                    }

                    return ret.ToArray();
                }
            }
        }
    }
}
