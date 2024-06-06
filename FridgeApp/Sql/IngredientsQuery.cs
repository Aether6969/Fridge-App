using Npgsql;
using System.Data.Common;
using System.Diagnostics;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace YourNamespace.Pages
{
    public class SearchModel
    {
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public List<string> FilteredItems { get; set; } = new List<string>();
        
        private readonly string connectionString = "Host=localhost;Port=5432;Database=g64;Username=g64_user;Password=g64_pwd_rule";

        public void OnSearch()
        {
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    FileInfo createTablesFile = new FileInfo("../Scripts/searchIngredients.sql");
                    string createTablesScript = createTablesFile.OpenText().ReadToEnd();
                    createTablesScript = createTablesScript.Replace("@name", "'%"+SearchTerm+"%'");
                    using (NpgsqlCommand searchCommand = new NpgsqlCommand(createTablesScript, connection))
                    {
                        using (NpgsqlDataReader reader = searchCommand.ExecuteReader())
                        {
                          while (reader.Read())
                            {
                                FilteredItems.Add(reader.GetString(0)); 
                            }
                        }
                    }
                }
            }
        }
    }
}

