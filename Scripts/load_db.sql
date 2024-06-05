-- Load the contents of the tables in the database
-- from the outputs of the scraper and data preprocessor.
-- The relative path from the script folder to the output
-- of the scraper is ../ArlaRecipeScraper/Data
-- User and fridge data is manually constructed
--
-- Load and build in the same order as the tables were
-- initialized so the state is consistent
--
-- psql -h localhost -d g64 -U username < load_db.sql
--
-- Note the use of \COPY in place of COPY in the script to
-- be able to actually load data from the csv file.
--
BEGIN TRANSACTION; -- build ingredients and recipies
	-- TODO: Temp until we load from the correct source 
	\COPY ingredients FROM './tempIngredients.csv' DELIMITER ',' CSV HEADER;
	--\COPY ingredients FROM './tempIngredients.csv' DELIMITER ',' CSV HEADER;
	--\COPY ingredients FROM '../ArlaRecipeScraper/Data/Ingrediants.csv' 
	--DELIMITER ',' CSV HEADER;
	--\COPY recipes FROM '../ArlaRecipeScraper/Data/Recipes.csv' 
	--DELIMITER ',' CSV HEADER;
	--\COPY recipeIngredients FROM '../ArlaRecipeScraper/Data/RecipeIngredients.csv' 
	--DELIMITER ',' CSV HEADER;
COMMIT;

BEGIN TRANSACTION; -- load users, fridges and contents in fridges
	\COPY users FROM './users.csv' DELIMITER ',' CSV HEADER;
	\COPY fridges FROM './fridges.csv' DELIMITER ',' CSV HEADER;
	\COPY fridgeIngredients FROM './fridgeIngredients.csv' DELIMITER ',' CSV HEADER;
COMMIT;
