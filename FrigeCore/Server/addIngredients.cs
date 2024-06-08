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
        public static void addIngredients(string AddTerm)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string existsIngredient = "SELECT * FROM fridgeIngredients WHERE ingredient = @ingredient AND Fridge = 3";
                existsIngredient = existsIngredient.Replace("@ingredient", "'" + AddTerm + "'");
                List<Ingredient> matches = new List<Ingredient>();
                using (NpgsqlCommand searchCommand = dataSource.CreateCommand(existsIngredient))
                {
                    using (NpgsqlDataReader reader = searchCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Ingredient ingredient = new Ingredient()
                            {
                                Name=reader.GetString(1),
                                Amount=reader.GetDouble(2),
                                Unit=reader.GetString(3), 
                            };
                            matches.Add(ingredient);
                        }
                    }
                }
                if(matches.Count()==0){
                    FileInfo addIngredient = new FileInfo("../Scripts/addIngredient.sql");
                    string addIngredientScript = addIngredient.OpenText().ReadToEnd();
                    addIngredientScript = addIngredientScript.Replace("@ingredient", "'" + AddTerm + "'");
                    addIngredientScript = addIngredientScript.Replace("@fridge", "3");
                    using (NpgsqlCommand command = new NpgsqlCommand(addIngredientScript, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            
            }
        }
    }
}
