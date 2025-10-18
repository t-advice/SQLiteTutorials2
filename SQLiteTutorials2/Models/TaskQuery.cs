using System;
using System.Collections.Generic;

namespace SQLiteTutorials2.Models
{
    public class TaskQuery
    {
        public string? Search { get; set; }
        public StatusFilter Status { get; set; } = StatusFilter.All;
        public PriorityLevel? Priority { get; set; }
        public DueFilter Due { get; set; } = DueFilter.Any;
        public List<string>? Tags { get; set; } // matches any of provided tags (OR)
        public SortField SortBy { get; set; } = SortField.CreatedAt;
        public bool Desc { get; set; }
    }

    public enum StatusFilter { All, Active, Completed }
    public enum DueFilter { Any, Overdue, Today, ThisWeek, NoDueDate }
    public enum SortField { CreatedAt, DueDate, Priority, Title }
}