using Npgsql;
using System;
using System.Collections.Generic;
using Npgsql;
using FrigeCore.Structures;

namespace FrigeCore.Server
{
    public static partial class Server
    {
        public static List<Ingredient> SearchIngrediants(string ingrediant)
        {
            FileInfo createTablesFile = new FileInfo("../Scripts/searchIngredients.sql");
            string createTablesScript = createTablesFile.OpenText().ReadToEnd();
            createTablesScript = createTablesScript.Replace("@name", "'%" + ingrediant + "%'");
            using (NpgsqlCommand searchCommand = dataSource.CreateCommand(createTablesScript))
            {
                using (NpgsqlDataReader reader = searchCommand.ExecuteReader())
                {
                    List<Ingredient> IngredientSearchResults = new List<Ingredient>();

                    while (reader.Read())
                    {
                        Ingredient ingredient = new Ingredient()
                        {
                            Name = reader.GetString(0),
                            Amount = reader.GetDouble(1),
                            Unit = reader.GetString(2),
                        };
                        IngredientSearchResults.Add(ingredient);
                    }

                    return IngredientSearchResults;
                }
            }
        }
    }
}
