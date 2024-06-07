using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrigeCore.Server
{
    public static partial class Server
    {
        public static void SetupTables()
        {
            // check if the tables in the database exists and has values by running a simple query
            bool success = false;
            try
            {
                using (NpgsqlCommand query = dataSource.CreateCommand("SELECT * FROM users"))
                using (NpgsqlDataReader reader = query.ExecuteReader())
                {
                    success = reader.HasRows;
                    Console.WriteLine("Verified access to database.");
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
                    // database tables not set up correctly, so setup tables
                    FileInfo createTablesFile = new FileInfo("../Scripts/create_tables.sql");
                    string createTablesScript = createTablesFile.OpenText().ReadToEnd();
                    using (NpgsqlCommand createTablesCmd = dataSource.CreateCommand(createTablesScript))
                    {
                        createTablesCmd.ExecuteNonQuery();
                    }

                    // Using COPY in sql to load tables form files is ... not very well
                    // designed, so jumping through the hoops to make it work.
                    FileInfo loadDbFile = new FileInfo("../Scripts/load_db.sql");
                    StringBuilder loadDbScript = new StringBuilder(loadDbFile.OpenText().ReadToEnd());
                    loadDbScript.Replace("\\COPY", "COPY");
                    string dataDir = System.IO.Directory.GetCurrentDirectory() + "\\..\\ArlaRecipeScraper\\Data\\";
                    loadDbScript.Replace("../ArlaRecipeScraper/Data/", dataDir);
                    string scriptDir = System.IO.Directory.GetCurrentDirectory() + "\\..\\Scripts\\";
                    loadDbScript.Replace("./", scriptDir);
                    using (NpgsqlCommand loadDbCmd = dataSource.CreateCommand(loadDbScript.ToString()))
                    {
                        loadDbCmd.ExecuteNonQuery();
                    }

                    sw.Stop();
                    Console.WriteLine("Database loaded in {0}", sw.Elapsed);
                }
                catch (Exception)
                {
                    Console.WriteLine($"Failed to load database {connectionString}");
                }
            }
        }
    }
}
