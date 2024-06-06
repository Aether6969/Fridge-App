using Npgsql;
using System.Data.Common;
using System.Diagnostics;
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
                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand query = new NpgsqlCommand("SELECT * FROM users", conn))
                    using (NpgsqlDataReader reader = query.ExecuteReader())
                    {
                        success = reader.HasRows;
                        Console.WriteLine("Verified access to database.");
                    }
                }
            }
            catch (Exception)
            {
                // users table does not exist
            }

            if (!success)
            {
                Console.WriteLine("Failed to query table in database, loading all tables ...");
                Stopwatch sw = new Stopwatch();
                sw.Start();

                try
                {
                    using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                    {
                        // database tables not set up correctly, so setup tables
                        FileInfo createTablesFile = new FileInfo("../Scripts/create_tables.sql");
                        string createTablesScript = createTablesFile.OpenText().ReadToEnd();
                        NpgsqlCommand createTablesCmd = new NpgsqlCommand(createTablesScript, conn);
                        conn.Open();
                        createTablesCmd.ExecuteNonQuery();
                        conn.Close();

                        // Using COPY in sql to load tables form files is ... not very well
                        // designed, so jumping through the hoops to make it work.
                        FileInfo loadDbFile = new FileInfo("../Scripts/load_db.sql");
                        StringBuilder loadDbScript = new StringBuilder(loadDbFile.OpenText().ReadToEnd());
                        loadDbScript.Replace("\\", "");
                        // TODO: Update when recipe data is available
                        string currentDir = System.IO.Directory.GetCurrentDirectory() + "\\..\\Scripts\\";
                        loadDbScript.Replace("./", currentDir);
                        NpgsqlCommand loadDbCmd = new NpgsqlCommand(loadDbScript.ToString(), conn);
                        conn.Open();
                        loadDbCmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    sw.Stop();
                    Console.WriteLine("Database loaded in {0}", sw.Elapsed);
                }
                catch (Exception)
                {
                    Console.WriteLine($"Failed to load database {ConnectionString}");
                }
            }           
        }
    }
}
