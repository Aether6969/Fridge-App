# DATABASE SETUP
The scripts to setup and delete the tables are placed in the _Scripts_ folder. Open a terminal (Linux) or a command prompt (Windows) in the _Scripts_ folder. The following command connects to PostgreSQL and creates a database called g64 and a user called g64_user:

    psql -h localhost -d postgres -U <database_username> < create_db.sql 

_database_username_ is the name of the user that you want to use to edit the database. The command assumes that the postgres database exists in the system.

When the app starts up, it will use the scripts _create_tables.sql_ and _load_db.sql_ to create and populate the tables in the database. These can be run manually:

    psql -h localhost -d g64 -U <database_username> < create_db.sql 
    psql -h localhost -d g64 -U <database_username> < load_db.sql 

In order to clean up the database after use, run the drop_db.sql script:

    psql -h localhost -d postgres -U <database_username> < drop_db.sql 

Depending on your setup, each call into psql may require entering the password for the database user.

TODO: Should the create and load scripts be merged?
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
