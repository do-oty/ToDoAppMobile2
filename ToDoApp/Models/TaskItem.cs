namespace ToDoApp.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Time { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
    }
}