# Description TODO:
The project is split into 4 parts, Fridge-core where most of the actual buisnes logic is handled, Scripts where the database querys are, Fridge-app the UI (the actual webapp), and WebScraper where we scrape the recipes from Arla 

# Idear TODO:
The idear for our app was that the user would specify the ingrediants they have available and then we would show them a list of recipes ranked by how many of the needed ingrediants they have, the recipes are scraped from [Arla.dk](https://www.arla.dk/opskrifter/) and the idear is that this is an app we would make for Arla.

# How to use the app TODO:
To the left you will see your fridge where you can type in the name of an ingrediant and search for an ingredint (by pressing the serch button) and add it to your fridge via the add button, bellow where you add the ingrediants there is a list of ingrediants in your fridge.
Based on the ingrediants in your fridge if you press the serch button (to the right) to get the top 3 recipes you could make (based on available ingrediants).

# ER Diagram TODO:
![.ERDiagram.png](https://github.com/Aether6969/Fridge-App/blob/master/ERDiagram.png)
er diag description

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

TODO: ER diagram here or elsewhere in project?

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
