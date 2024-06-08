-- Creates the database, tables and the user for the app
-- Load tables using load_db.sql
--
-- psql -h localhost -d g64 -U username < create_tables.sql
--

-- Clear existing tables and indices if any
DROP INDEX IF EXISTS recipesNameIndex;
DROP INDEX IF EXISTS recipeIngredientsNameIndex;
DROP TABLE IF EXISTS fridgeIngredients;
DROP TABLE IF EXISTS fridges;
DROP TABLE IF EXISTS users;
DROP TABLE IF EXISTS recipeIngredients;
DROP TABLE IF EXISTS recipes;
DROP TABLE IF EXISTS ingredients;

-- TODO: Do we actually need this?
CREATE TABLE ingredients(
    name TEXT,
    PRIMARY KEY (name)
);

CREATE TABLE recipes(
    name TEXT,
    link TEXT DEFAULT '' NOT NULL,
    recipeType VARCHAR(15),
    totalTimeMin INT,
    isFreezable BOOL,
    rating INT,
    imageLink TEXT DEFAULT '' NOT NULL,
    PRIMARY KEY (name)
);

CREATE INDEX recipesNameIndex
ON recipes(name);

CREATE TABLE recipeIngredients(
    id BIGSERIAL NOT NULL, -- because recipe,ingredient,amount,unit are not guarenteed to be unique
    recipe TEXT,
    ingredient TEXT,
    amount REAL,
    unit TEXT DEFAULT '' NOT NULL,
    PRIMARY KEY (id),
    FOREIGN KEY(recipe) REFERENCES recipes(name) ON DELETE CASCADE ON UPDATE CASCADE
    -- The ingredients are a mess in the dataset. Without a significant cleaning
    -- effort, a foreign key on ingredients is not feasible.
    --FOREIGN KEY(ingredient) REFERENCES ingredients(name) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE INDEX recipeIngredientsNameIndex
ON recipeIngredients(recipe);

CREATE TABLE users(
    name TEXT, -- user names must be unique
    --password TEXT,
    --TODO: add more user info
    PRIMARY KEY (name)
);

-- the fridge represents all ingredients at one specific location, 
-- e.g. home or a cabin in the woods, regardless of being in a fridge,
-- storage or freezer.
CREATE TABLE fridges(
    id INT,
    owner TEXT,
    name TEXT, -- display name
    PRIMARY KEY (id),
    FOREIGN KEY(owner) REFERENCES users(name) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE fridgeIngredients(
    fridge INT,
    ingredient TEXT,
    amount REAL,
    unit TEXT DEFAULT '' NOT NULL,
    PRIMARY KEY (fridge, ingredient),
    FOREIGN KEY(fridge) REFERENCES fridges(id) ON DELETE CASCADE ON UPDATE CASCADE
    -- The ingredients are a mess in the dataset. Without a significant cleaning
    -- effort, a foreign key on ingredients is not feasible.
    --FOREIGN KEY(ingredient) REFERENCES ingredients(name) ON DELETE CASCADE ON UPDATE CASCADE
);
