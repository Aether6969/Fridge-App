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
                                Name=reader.GetString(0),
                                Link="",
                                RecipeType=RecipeType.Unknown,
                                TotalTimeMin=0,
                                IsFreezable=false,
                                Rating=0,
                                ImageLink="",
                                IngrediantsAmount= new Ingredient[0],
                            };
                            matches.Add(Tuple.Create(recipe, (int)reader.GetDouble(3)));
                        }
                    }
                }
            }
            return matches;
        }
    }
}
