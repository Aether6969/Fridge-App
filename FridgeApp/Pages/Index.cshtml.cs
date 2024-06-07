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
        //ingredient search
        public string IngredientSearchContent {get;set;}= "<select>"+ "</select>";
        public string SearchTerm {get;set;}= "";
        public List<string> IngredientSearchResults {get;set;}= new List<string>();
        //saved ingredients
        string AddTerm {get;set;}= "";
        public List<string> SavedIngredients {get;set;}= new List<string>();
        public string IngredientSavedContent {get;set;}= "<select>"+ "</select>";

        private readonly string connectionString = "Host=localhost;Port=5432;Database=g64;Username=g64_user;Password=g64_pwd_rule";

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<string> UserIngredientSearchResults { get; set; } = new List<string>();
        public string UserIngredientSavedContent { get; set; } = string.Empty;
        public void OnGet()
        {
            string query = $"SELECT ingredient,amount,unit FROM fridgeIngredients" +
                "\r\n\tinner join " +
                "\r\n\t(SELECT id FROM fridges" +
                "\r\n\t\tinner join users ON fridges.owner = users.name AND users.name = @username)" +
                "\r\n\t\tAS userFridge" +
                "\r\n\tON fridgeIngredients.fridge = userFridge.id;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new("username", "Bilbo"));
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        UserIngredientSearchResults = new List<string>();
                        while (reader.Read())
                        {
                            UserIngredientSearchResults.Add(reader.GetString(0));
                        }
                    }
                }
            }
            UserIngredientSavedContent = string.Empty;
            foreach(string ingredient in  UserIngredientSearchResults)
            {
                UserIngredientSavedContent += "<p>" + ingredient + "</p>";
            }
        }
        public void OnPost(){
            if (Request.Form.ContainsKey("submitIngredientSearch"))
            {
                SearchTerm = Request.Form["searchIngredient"].ToString();
                IngredientSearchResults =new List<string>();
                
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
                                IngredientSearchResults.Add(reader.GetString(0)); 
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
            if(Request.Form.ContainsKey("addIngredients")){
                AddTerm = Request.Form["selectedIngredient"].ToString();
                Console.WriteLine("akdfh "+AddTerm);
                // Create a new select element
                IngredientSavedContent = "<select>";

                IngredientSavedContent += $"<option value='{AddTerm}'>{AddTerm}</option>";

                IngredientSavedContent += "</select>";
            }
        }
    }
}
