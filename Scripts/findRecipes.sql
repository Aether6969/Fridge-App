-- sort recipies by number of ingredients we have
WITH available_ingredients AS (
    SELECT fridgeingredients. ingredient
    FROM fridgeingredients
    INNER JOIN fridges ON fridges.id = fridgeingredients.fridge
    WHERE fridges.id = @fridge
), recipe_ingredients AS ( 
	SELECT recipes.name, recipeingredients.ingredient 
	FROM recipes INNER JOIN recipeingredients ON recipes.name = recipeingredients.recipe)
	
SELECT recipe_ingredients.name, COUNT(available_ingredients.ingredient) AS num_available, COUNT (recipe_ingredients.name) AS num_total, 100*COUNT(available_ingredients.ingredient) / COUNT (recipe_ingredients.name) AS ratio
FROM available_ingredients RIGHT OUTER JOIN recipe_ingredients ON recipe_ingredients.ingredient = available_ingredients.ingredient
GROUP BY recipe_ingredients.name
ORDER BY ratio DESC;
