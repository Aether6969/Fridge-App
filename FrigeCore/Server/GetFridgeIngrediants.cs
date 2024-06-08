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
        public static List<Ingredient> GetFridgeIngrediants(string user)
        {
            string query = "SELECT ingredient,amount,unit FROM fridgeIngredients" +
                "\r\n\tinner join " +
                "\r\n\t(SELECT id FROM fridges" +
                "\r\n\t\tinner join users ON fridges.owner = users.name AND users.name = @username)" +
                "\r\n\t\tAS userFridge" +
                "\r\n\tON fridgeIngredients.fridge = userFridge.id;";
            using (NpgsqlCommand cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.Add(new("username", user));
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    List<Ingredient> UserIngredientSearchResults = new();
                    while (reader.Read())
                    {
                        Ingredient ingredient = new Ingredient()
                        {
                            
                            Name=reader.GetString(0),
                            Amount=reader.GetDouble(1),
                            Unit=reader.GetString(2), 
                        };

                        UserIngredientSearchResults.Add(ingredient);
                    }

                    return UserIngredientSearchResults;
                }
            }
        }
    }
}
