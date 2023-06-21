cd /d %~dp0 
@echo off

if not defined PGSQL set PGSQL=pgsql
if not defined PGDATABASE set PGDATABASE=tourplanner
if not defined PGPORT set PGPORT=5432
if not defined PGUSER set PGUSER=postgres

echo.
echo Importing from db_copy.sql...

"%PGSQL%\bin\psql" --port=%PGPORT% --dbname="%PGDATABASE%" --username="%PGUSER%" -v ON_ERROR_STOP=1 --echo-errors -v ECHO=all < db_copy.sql

set /p=Hit ENTER to continue...