DROP VIEW IF EXISTS summedMultiples;
DROP VIEW IF EXISTS multiples;

CREATE VIEW multiples AS
	SELECT DISTINCT ri1.id,ri1.recipe,ri1.ingredient,ri1.amount,ri1.unit
		FROM recipeIngredients ri1, recipeIngredients ri2
	WHERE ri1.recipe = ri2.recipe AND ri1.ingredient = ri2.ingredient
		AND NOT ri1.id = ri2.id
	ORDER BY ri1.recipe,ri1.ingredient ASC;

CREATE VIEW summedMultiples AS
	SELECT recipe,ingredient,SUM(amount) AS amount,unit
		FROM multiples
	GROUP BY recipe,ingredient,unit;

INSERT INTO recipeIngredients(recipe,ingredient,amount,unit)
	SELECT recipe,ingredient,amount,unit FROM summedMultiples;

DELETE FROM recipeIngredients ri
	USING summedMultiples sm
	WHERE sm.recipe = ri.recipe 
		AND sm.ingredient = ri.ingredient 
		AND NOT sm.amount = ri.amount;

DROP VIEW IF EXISTS summedMultiples;
DROP VIEW IF EXISTS multiples;
SELECT * FROM recipeIngredients ORDER BY recipe ASC;