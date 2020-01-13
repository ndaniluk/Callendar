using System;

namespace Callendar
{
    public class Task
    {
        public int Id { get; set; }
        public bool IsClosed { get; set; }

        public int TaskCategoryId { get; set; }

        public TaskCategory TaskCategory { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}