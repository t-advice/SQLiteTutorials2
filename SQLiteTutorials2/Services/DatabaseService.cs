using SQLite;
using SQLiteTutorials2.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTutorials2.Services
{
    public class DatabaseService // Service class for database operations
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<TaskItem>().Wait(); // Create or migrate table (adds new columns)

            // Helpful indexes for filters/sorting
            _ = _database.ExecuteAsync("CREATE INDEX IF NOT EXISTS idx_taskitem_iscompleted ON TaskItem(IsCompleted);");
            _ = _database.ExecuteAsync("CREATE INDEX IF NOT EXISTS idx_taskitem_priority ON TaskItem(Priority);");
            _ = _database.ExecuteAsync("CREATE INDEX IF NOT EXISTS idx_taskitem_duedate ON TaskItem(DueDate);");
            _ = _database.ExecuteAsync("CREATE INDEX IF NOT EXISTS idx_taskitem_createdat ON TaskItem(CreatedAt);");
        }

        public Task<List<TaskItem>> GetTasksAsync() =>
            _database.Table<TaskItem>().ToListAsync();

        public Task<int> SaveTaskAsync(TaskItem task) =>
            _database.InsertOrReplaceAsync(task);

        public Task<int> DeleteTaskAsync(TaskItem task) =>
            _database.DeleteAsync(task);

        public Task<List<TaskItem>> GetTasksAsync(TaskQuery query)
        {
            // Build dynamic SQL with parameters to support LIKE and ranges
            var sql = new StringBuilder("SELECT * FROM TaskItem WHERE 1=1");
            var args = new List<object>();

            // Search in Title or Tags
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                sql.Append(" AND (Title LIKE ? OR IFNULL(Tags,'') LIKE ?)");
                var like = $"%{query.Search.Trim()}%";
                args.Add(like);
                args.Add(like);
            }

            // Status
            switch (query.Status)
            {
                case StatusFilter.Active:
                    sql.Append(" AND IsCompleted = 0");
                    break;
                case StatusFilter.Completed:
                    sql.Append(" AND IsCompleted = 1");
                    break;
            }

            // Priority
            if (query.Priority.HasValue)
            {
                sql.Append(" AND Priority = ?");
                args.Add((int)query.Priority.Value);
            }

            // Tags OR matching: any tag contained in comma-separated Tags column
            if (query.Tags is { Count: > 0 })
            {
                sql.Append(" AND (");
                for (int i = 0; i < query.Tags.Count; i++)
                {
                    if (i > 0) sql.Append(" OR ");
                    sql.Append("IFNULL(Tags,'') LIKE ?");
                    args.Add($"%{query.Tags[i].Trim()}%");
                }
                sql.Append(")");
            }

            // Due date filters using simple date ranges
            var now = DateTime.Now;
            var todayStart = DateTime.Today;
            var todayEnd = todayStart.AddDays(1);
            var weekStart = todayStart.AddDays(-(int)todayStart.DayOfWeek); // Sunday-based
            var weekEnd = weekStart.AddDays(7);

            switch (query.Due)
            {
                case DueFilter.Overdue:
                    sql.Append(" AND DueDate IS NOT NULL AND DueDate < ?");
                    args.Add(todayStart);
                    break;
                case DueFilter.Today:
                    sql.Append(" AND DueDate IS NOT NULL AND DueDate >= ? AND DueDate < ?");
                    args.Add(todayStart);
                    args.Add(todayEnd);
                    break;
                case DueFilter.ThisWeek:
                    sql.Append(" AND DueDate IS NOT NULL AND DueDate >= ? AND DueDate < ?");
                    args.Add(weekStart);
                    args.Add(weekEnd);
                    break;
                case DueFilter.NoDueDate:
                    sql.Append(" AND DueDate IS NULL");
                    break;
            }

            // Sorting
            string orderBy = query.SortBy switch
            {
                SortField.DueDate => $"(DueDate IS NULL) ASC, DueDate {(query.Desc ? "DESC" : "ASC")}",
                SortField.Priority => $"Priority {(query.Desc ? "DESC" : "ASC")}",
                SortField.Title => $"Title {(query.Desc ? "DESC" : "ASC")}",
                _ => $"CreatedAt {(query.Desc ? "DESC" : "ASC")}"
            };
            sql.Append($" ORDER BY {orderBy}");

            return _database.QueryAsync<TaskItem>(sql.ToString(), args.ToArray());
        }
    }
}
