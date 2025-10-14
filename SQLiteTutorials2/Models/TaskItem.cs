using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace SQLiteTutorials2.Models
{
    public class TaskItem // Model class for a task
    {
        [PrimaryKey, AutoIncrement] // SQLite attributes
        public int Id { get; set; }  // Primary key
        public string Title { get; set; }  // Task title
        public bool IsCompleted { get; set; } // New field for task completion status
        public DateTime CreatedAt { get; set; } // New field for creation timestamp

    }
}
