-- search recipie by type and name (recipietypequery as set of all types default, rquery as empty string as default)
select name AS results
FROM recipes 
WHERE (recipes.recipetype IN @typelist) AND name LIKE '%@search%'

