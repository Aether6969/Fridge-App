using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
﻿using Npgsql;
using System.Data.Common;
using System.Diagnostics;
using System.Text;

using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.IO;

namespace FridgeApp.Pages
{
    public class IndexModel : PageModel
    {
        public string SearchTerm { get; set; } = "";

        public List<string> IngredientSearchResults { get; set; } = new List<string>();

        public string IngredientSearchContent {get;set;} = "";
    
        private readonly string connectionString = "Host=localhost;Port=5432;Database=g64;Username=g64_user;Password=g64_pwd_rule";

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Response.ContentType = "text/html;charset=UTF-8";
            Request.ContentType = "text/html;charset=UTF-8";
        }
        public void OnPost(){
            Response.ContentType = "text/html;charset=UTF-8";
            Request.ContentType = "text/html;charset=UTF-8";
            if (Request.Form["submitIngredientSearch"] == "search")
                {
                    SearchTerm = Request.Form["searchIngredient"].ToString();
                    
                    if (!string.IsNullOrWhiteSpace(Request.Form["searchIngredient"])){
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
                                        IngredientSearchResults .Add(reader.GetString(0)); 
                                    }
                                }
                            }
                        }
                    }

                    // Create a new select element
                    IngredientSearchContent = "<select>";

                    // Loop through the filtered items and create option elements
                    foreach(string ingredient in IngredientSearchResults )
                    {
                        IngredientSearchContent += $"<option value='{ingredient}'>{ingredient}</option>";
                    }

                    IngredientSearchContent += "</select>";
                }
        }
    }
}