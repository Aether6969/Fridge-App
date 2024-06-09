# Description
The project is split into 4 parts:

* _FridgeCore_ where most of the actual buisness logic is handled. 
* _Scripts_ where the database querys are.
* _Fridge-App_ the UI (the actual webapp).
* _WebScraper_ where we scrape the recipes from Arla.

# Idea
The idea for our app was that the user would specify the ingredients they have available and then we would show them a list of recipes ranked by how many of the needed ingredients they have. The recipes are scraped from [Arla.dk](https://www.arla.dk/opskrifter/) and the idea is that this is an app we would make for Arla.

# How to use the app
To the left you will see your fridge where you can add ingredients and see your list of ingredients. Type in the name of an ingredient and search for an ingredient (by pressing the search button). The ingredient can be added it to your fridge via the add button. The list of ingredients in your fridge is shown below the add button.

To the right you can press the search button to get the top 3 recipes that you could make, based on and ranked by the available ingredients in your fridge. This contains a link to the full recipe on arla.dk.

# ER Diagram
![.ERDiagram.png](https://github.com/Aether6969/Fridge-App/blob/master/ERDiagram.png)

As can be seen, the ER diagram consists of three "islands" that are isolated from each other. Originally, we had planned to let both fridgeIngredients and recipeIngredients have a relation to ingredients, but in order to make that work, we needed to put in significant post scraping work to do a propper mapping. As an example, _peber_, _friskkværnet peber_ and _sort peber_ are equivalent, but _peberfrugt_ is not. More exotic ingredients, like _rester_, also needed to be considered. In the end, we decided to cut the dependencies and create a manually cleaned ingredient table.

The left part concerns the user: The user, the fridge(s) of the user (e.g. one at home and another in a cabin) and the ingredients in the fridge. Note that the app does not support changing user or adding more than one fridge. The user can add ingredients in the app. The right side is the data from Arla. This data is static in the sense that it can only be modified when running the web scraper and manually imported.

The tables are set up for cascading deletes and updates, and an index has been added on recipeIngredients.recipe to facilitate searches.

The main query of the app is the search that matches the ingredients of the user (in fridgeIngredients) with the recipe ingredients (in recipeIngredients), see _Scripts/findRecipes.sql_, in order to find the recipes where the user has the most of the ingredients needed to make the recipe.

# DATABASE SETUP
The scripts to setup and delete the tables are placed in the _Scripts_ folder. Open a terminal (Linux) or a command prompt (Windows) in the _Scripts_ folder. The following command connects to PostgreSQL and creates a database called _g64_ and a user called _g64_user_:

    psql -h localhost -d postgres -U <database_username> < create_db.sql 

_database_username_ is the name of the user that you want to use to edit the database. The command assumes that the postgres database exists in the system.

When the app starts up, it will use the scripts _create_tables.sql_ and _load_db.sql_ to create and populate the tables in the database. These can be run manually:

    psql -h localhost -d g64 -U <database_username> < create_tables.sql 
    psql -h localhost -d g64 -U <database_username> < load_db.sql 

In order to clean up the database after use, run the drop_db.sql script:

    psql -h localhost -d postgres -U <database_username> < drop_db.sql 

This script can fail if something is still connected to the database, but it will tell you of any failure. In this case, open pgAdmin and delete the database and user manually.
Depending on your setup, each call into psql may require entering the password for the database user.

## Troubleshooting
Some operating systems and some setups may differ. Typical issues:

1) The streaming operator < does not work. Use -f instead.
2) The tables are created but nothing is loaded into them. This is typically a rights issue: The database script does not have access to the folders containing the csv files. Please grant access to _/Scripts_ and _/Fridge-App/ArlaRecipeScraper/Data_.

# COMPILE AND RUN

## In Visual Studio 2022 with ASP.NET and NpgSql installed

1) Open FridgeApp.sln
2) Change run configuration from https to http (this will avoid installing certificates)
3) Press F5 to start the app (or Ctrl+F5 to run without debugger)
4) Wait a bit and Fridge App should open in your default browser

If ASP.NET is not installed, open the Visual Studio Installer, press _Modify_, check the _ASP.NET and web development_ checkbox and press _Modify_ to start installation.
NpgSql PostgreSQL can be installed through the extension manager.

## In Visual Studio Code with C# Dev Kit installed

1) Open folder containing FridgeApp.sln (e.g by using Ctrl+K Ctrl+O)
2) Press F5 to start the app (or Ctrl+F5 to run without debugger)
3) Wait a bit and Fridge App should open in your default browser

If the C# Dev Kit is not installed, open Visual Studio Code, press Ctrl+Shift+X, search for and select _C# Dev Kit_.

## Troubleshooting
If the app does not open in your browser, a debug console should be opened by the dot net framework, which should contain a line like _Now listening on: http://localhost:5291_ . Open the url at the end in your browser.
