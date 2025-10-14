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
    public class DatabaseService // Service class for database operations
    {
        private readonly SQLiteAsyncConnection _database; // SQLite connection
        public DatabaseService(string dbPath) // Constructor with database path
        {
            _database = new SQLiteAsyncConnection(dbPath); // Initialize connection
            _database.CreateTableAsync<TaskItem>().Wait(); // Create table if not exists
        }
        public Task<List<TaskItem>> GetTasksAsync() => _database.Table<TaskItem>().ToListAsync(); // Get all tasks
        public Task<int> SaveTaskAsync(TaskItem task) => _database.InsertOrReplaceAsync(task); // Insert or update task
        public Task<int> DeleteTaskAsync(TaskItem task) => _database.DeleteAsync(task); // Delete task
    }
}
