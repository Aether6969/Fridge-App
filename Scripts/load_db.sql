-- Load the contents of the tables in the database
-- from the outputs of the scraper and data preprocessor.
-- The relative path from the script folder to the output
-- of the scraper is ../ArlaRecipeScraper/Data
-- User and fridge data is manually constructed

-- Load and build in the same order as the tables were
-- initialized so the state is consistent

-- psql -h localhost -d dis24g64 -U mrtn -v path="'/home/bossman/Documents/Database (2024)/Project/Fridge-App/Scripts/users.csv'" < load_db.sql


-- TODO: Load from csv requires absolute path :rollseyes:
--BEGIN TRANSACTION; -- build ingredients and recipies
--load ingredients and recipes
--COPY ingredients FROM '../ArlaRecipeScraper/Data/Ingrediants.csv' 
--DELIMITER ',' CSV HEADER;

--COPY recipes FROM '../ArlaRecipeScraper/Data/Recipes.csv' 
--DELIMITER ',' CSV HEADER;

-- TODO: Do we have this???
--COPY recipeIngredients FROM '../ArlaRecipeScraper/Data/RecipeIngredients.csv' 
--DELIMITER ',' CSV HEADER;
--COMMIT;

--BEGIN TRANSACTION; -- load users, fridges and contents in fridges
	--users
	\COPY users FROM './users.csv' DELIMITER ',' CSV HEADER;
	--INSERT INTO users(name) VALUES ('Bilbo');
	--INSERT INTO users(name) VALUES ('Gandalf');
	--INSERT INTO users(name) VALUES ('Thorin');
	--INSERT INTO users(name) VALUES ('Bombur');

	--fridges
	\COPY fridges FROM './fridges.csv' DELIMITER ',' CSV HEADER;
	--INSERT INTO fridges(id,owner,name) VALUES (0,'Bilbo','Bagend');
	--INSERT INTO fridges(id,owner,name) VALUES (1,'Bilbo','Bree');
	--INSERT INTO fridges(id,owner,name) VALUES (2,'Gandalf','Rivendel');
	--INSERT INTO fridges(id,owner,name) VALUES (3,'Thorin','The Lonely Mountain');
	--INSERT INTO fridges(id,owner,name) VALUES (4,'Bombur','The Lonely Mountain');
	--INSERT INTO fridges(id,owner,name) VALUES (5,'Bombur','The Prancing Pony');	
--COMMIT;
