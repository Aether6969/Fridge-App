-- Creates the database, tables and the user for the app
-- Load tables using load_db.sql
-- Delete using drop_db.sql
--
-- psql -h localhost -d g64 -U username < create_db.sql
--

-- Clear existing tables if any
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
    link TEXT,
    recipeType VARCHAR(10),
    totalTimeMin INT,
    isFreezable BOOL,
    rating INT,
    imageLink TEXT,
    PRIMARY KEY (name)
);

CREATE TABLE recipeIngredients(
    recipe TEXT,
    ingredient TEXT,
    amount REAL,
    unit VARCHAR(7),
    PRIMARY KEY (recipe, ingredient),
    FOREIGN KEY(recipe) REFERENCES recipes(name),
    FOREIGN KEY(ingredient) REFERENCES ingredients(name)
);

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
    FOREIGN KEY(owner) REFERENCES users(name)
);

CREATE TABLE fridgeIngredients(
    fridge INT,
    ingredient TEXT,
    amount REAL,
    unit VARCHAR(7),
    PRIMARY KEY (fridge, ingredient),
    FOREIGN KEY(fridge) REFERENCES fridges(id),
    FOREIGN KEY(ingredient) REFERENCES ingredients(name)
);
