using System.Collections.Generic;

namespace Callendar
{
    public class TaskCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ScorePoints { get; set; }

        public ICollection<Task> Tasks { get; set; }
    }
}