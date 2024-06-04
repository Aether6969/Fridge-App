-- Drops all tables created by the setup script.
-- Note that the order of droping is the reverse of the order of initialization.
-- psql -h localhost -d dis24g64 -U sql_user_name -W < drop_db.sql
DROP TABLE IF EXISTS fridgeIngredients;
DROP TABLE IF EXISTS fridges;
DROP TABLE IF EXISTS users;
DROP TABLE IF EXISTS recipeIngredients;
DROP TABLE IF EXISTS recipes;
DROP TABLE IF EXISTS ingredients;

-- Drop user created for this set of tables
DROP USER IF EXISTS g64_user