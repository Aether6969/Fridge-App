-- Drops the database and user created by the setup script.
-- Note that the order of droping is the reverse of the order of initialization.
--
-- psql -h localhost -d postgres -U username < drop_db.sql
--

-- Drop user created for this set of tables
DROP USER IF EXISTS g64_user;

-- Drop database created for this set of tables
DROP DATABASE IF EXISTS g64;