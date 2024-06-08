using FrigeCore.Structures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
﻿using Npgsql;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text;

using static FrigeCore.Server.Server;

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
            getfridge();
        }
        public void OnPost(){
            if (Request.Form.ContainsKey("submitIngredientSearch"))
            {
                SearchTerm = Request.Form["searchIngredient"].ToString();
                IngredientSearchResults = SearchIngrediants(SearchTerm).Select((x) => x.Name).ToList();

                // Create a new select element
                IngredientSearchContent = "<select>";

                // Loop through the filtered items and create option elements
                foreach(string ingredient in IngredientSearchResults)
                {
                    IngredientSearchContent += $"<option value='{ingredient}'>{ingredient}</option>";
                }

                IngredientSearchContent += "</select>";
            }
            if(Request.Form.ContainsKey("addIngredients")){
                AddTerm = Request.Form["selectedIngredient"].ToString();
                addIngredients(AddTerm);
            }                               
            getfridge();
        }
        public void getfridge(){
            UserIngredientSearchResults = GetFridgeIngrediants("Bombur").Select((x) => x.Name).ToList();
            UserIngredientSavedContent = string.Empty;

            // This is not the prettiest way to do this
            UserIngredientSavedContent += "<table class=\"center\">";
            int ingredientsPerRow = 3;
            for (int i = 0; i < UserIngredientSearchResults.Count; i++)
            {
                string ingredient = UserIngredientSearchResults[i];
                switch (i % ingredientsPerRow)
                {
                    case 0:
                        UserIngredientSavedContent += "<tr><td>" + ingredient +"</td>";
                        break;
                    case 1:
                        UserIngredientSavedContent += "<td>" + ingredient + "</td>";
                        break;
                    case 2:
                        UserIngredientSavedContent += "<td>" + ingredient  + "</td></tr>";
                        break;
                }
            }
            if (UserIngredientSearchResults.Count % ingredientsPerRow != 0)
            {
                UserIngredientSavedContent += "</tr>";
            }
            UserIngredientSavedContent += "</table>";
        }
    }
}
