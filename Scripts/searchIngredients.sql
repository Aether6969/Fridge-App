SELECT name AS results
FROM ingredients 
WHERE LOWER(name) LIKE LOWER(@name)


