using FrigeCore.Structures;
using Npgsql;

namespace FrigeCore.Server
{
    public static partial class Server
    {
        //TODO: move
        private static string connectionString = "Host=localhost;Port=5432;Database=g64;Username=g64_user;Password=g64_pwd_rule";
        private static NpgsqlDataSource dataSource = SetUpDatabaseSource();
        private static NpgsqlDataSource SetUpDatabaseSource()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CloseDataSource);

            return NpgsqlDataSource.Create(connectionString);
        }
        private static void CloseDataSource(object? sender, EventArgs e)
        {
            dataSource?.Dispose();
        }
    }
}
