using Npgsql;
using System.Data.Common;
using System.Text;

namespace FridgeApp
{
    public static class Sql
    {
        private static readonly string ConnectionString = "Host=localhost;Port=5432;Database=g64;Username=g64_user;Password=g64_pwd_rule";

        public static void SetupTables()
        {
            // check if the tables in the database exists and has values by running a simple query
            bool success = false;
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);

                var query = new NpgsqlCommand("SELECT * FROM users", conn);
                conn.Open();
                var reader = query.ExecuteReader();
                success = reader.HasRows;
                reader.Close();
                conn.Close();
            }
            catch (Exception)
            {
                // users table does not exist
            }

            if (!success)
            {
                Console.WriteLine("Failed to query table in database, loading all tables ...");

                try
                {
                    NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);

                    // database tables not set up correctly, so setup tables
                    FileInfo create_tables_file = new FileInfo("../Scripts/create_tables.sql");
                    string create_tables_script = create_tables_file.OpenText().ReadToEnd();
                    var create_tables_cmd = new NpgsqlCommand(create_tables_script, conn);
                    conn.Open();
                    create_tables_cmd.ExecuteNonQuery();
                    conn.Close();

                    FileInfo load_db_file = new FileInfo("../Scripts/load_db.sql");
                    StringBuilder load_db_script = new StringBuilder(load_db_file.OpenText().ReadToEnd());
                    load_db_script.Replace("\\", "");
                    string currentDir = System.IO.Directory.GetCurrentDirectory() + "\\..\\Scripts\\";
                    load_db_script.Replace("./", currentDir);
                    var load_db_cmd = new NpgsqlCommand(load_db_script.ToString(), conn);
                    conn.Open();
                    load_db_cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception)
                {
                    Console.WriteLine($"Failed to load database {ConnectionString}");
                }
            }           
        }
    }
}
