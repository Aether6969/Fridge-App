-- Many recipies contain multiple subrecipes each of which
-- with their own list of ingredients, e.g. drømmekage consists
-- of a cake part and a cover part. In this case some ingredients,
-- e.g. butter and milk in drømmekage, can exist in both parts. 
-- This script tries to compute the sum of these ingredients
-- and gather them in one tuple.

-- multiples and summedMultiples are temporary tables to
-- store results. They are dropped at the start and end
-- of the run.
DROP TABLE IF EXISTS summedMultiples;
DROP TABLE IF EXISTS multiples;

CREATE TABLE multiples AS
	SELECT DISTINCT ri1.id,ri1.recipe,ri1.ingredient,ri1.amount,ri1.unit
		FROM recipeIngredients ri1, recipeIngredients ri2
	WHERE ri1.recipe = ri2.recipe AND ri1.ingredient = ri2.ingredient
		AND NOT ri1.id = ri2.id
	ORDER BY ri1.recipe,ri1.ingredient ASC;

CREATE TABLE summedMultiples AS
	SELECT recipe,ingredient,SUM(amount) AS amount,unit
		FROM multiples
	GROUP BY recipe,ingredient,unit;

DELETE FROM recipeIngredients ri
	USING summedMultiples sm
	WHERE sm.recipe = ri.recipe 
		AND sm.ingredient = ri.ingredient;

INSERT INTO recipeIngredients(recipe,ingredient,amount,unit)
	SELECT * FROM summedMultiples;

DROP TABLE IF EXISTS summedMultiples;
DROP TABLE IF EXISTS multiples;

-- enable this output to see two recipes with problems
--SELECT * FROM recipeIngredients where recipe='3 slags laks' or recipe='Drømmekage fra Brovst';