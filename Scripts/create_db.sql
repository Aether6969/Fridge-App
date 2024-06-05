-- Creates the database and the user for the app
-- Create tables using create_tables.sql
-- Load tables using load_db.sql
-- Delete using drop_db.sql
--
-- psql -h localhost -d postgres -U username < create_db.sql
--
-- The create_db and create_tables scripts can't be in the same
-- file as we need to connect to an existing table with psql
-- and we can't change database within a script.
--
CREATE DATABASE g64 WITH 
    ENCODING = 'UTF8'
    CONNECTION LIMIT -1;

CREATE ROLE g64_user WITH
    LOGIN
    SUPERUSER
    CREATEDB
    CREATEROLE
    INHERIT
    NOREPLICATION
    BYPASSRLS
    PASSWORD 'g64_pwd_rule'
    CONNECTION LIMIT -1;