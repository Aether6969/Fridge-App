-- Creates the tables and creates the user used by the app
-- Load tables using load_db.sql
-- Delete using drop_db.sql
-- psql -h localhost -d dis24g64 -U psql_user_name -W < create_db.sql
CREATE ROLE g64_user WITH
    LOGIN
    SUPERUSER
    CREATEDB
    CREATEROLE
    INHERIT
    NOREPLICATION
    BYPASSRLS
    CONNECTION LIMIT -1;

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

--CREATE TABLE fridges(
--    name TEXT,
    --amount REAL,
    --unit VARCHAR(7),
 --   PRIMARY KEY (name)--,
    --FOREIGN KEY(recipe) REFERENCES recipes(name),
    --FOREIGN KEY(ingredient) REFERENCES ingredients(name)
--);

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