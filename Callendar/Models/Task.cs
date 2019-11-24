namespace Callendar
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsClosed { get; set; }

        public int TaskCategoryId { get; set; }
        public TaskCategory TaskCategory { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }
    }
}