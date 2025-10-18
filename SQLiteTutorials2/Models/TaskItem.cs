using System;
using SQLite;

namespace SQLiteTutorials2.Models
{
    public class TaskItem // Model class for a task
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; }  // Task title

        [Indexed]
        public bool IsCompleted { get; set; } // completion status

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // creation timestamp

        [Indexed]
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium; // new: priority

        [Indexed]
        public DateTime? DueDate { get; set; } // new: due date (nullable)

        public string? Tags { get; set; } // new: comma-separated tags, e.g. "work,home,urgent"
    }

    public enum PriorityLevel
    {
        Low = 0,
        Medium = 1,
        High = 2
    }
}
