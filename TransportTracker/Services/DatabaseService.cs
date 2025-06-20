using SQLite;
using TransportTracker.Models;

namespace TransportTracker.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;
        public DatabaseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "TransportTracker.db3");
            _database = new SQLiteAsyncConnection(dbPath);
        }
        public async Task InitializeAsync()
        {
            await _database.CreateTableAsync<Route>();
            await _database.CreateTableAsync<Driver>();
            await _database.CreateTableAsync<Work>();
        }
        public SQLiteAsyncConnection GetConnection()
        {
            return _database;
        }
    }
}
