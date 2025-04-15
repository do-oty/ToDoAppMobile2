using System;

namespace ToDoApp.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Time { get; set; }
        public required string Image { get; set; }
        public required string Description { get; set; }
        public string Details { get; set; } = string.Empty;
    }
} 