using SQLite;
using SQLiteTutorials2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTutorials2.Services
{
    internal class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;
        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<TaskItem>().Wait();
        }
        public Task<List<TaskItem>> GetTaskItemAsync() => _database.Table<TaskItem>().ToListAsync();
        public Task<int> SaveTaskAsync(TaskItem task) => _database.InsertOrReplaceAsync(task);
        public Task<int> DeleteTaskAsync(TaskItem task) => _database.DeleteAsync(task);
    }
}
