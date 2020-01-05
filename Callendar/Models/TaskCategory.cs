using System.Collections.Generic;
using Newtonsoft.Json;

namespace Callendar
{
    public class TaskCategory
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ScorePoints { get; set; }

        [JsonIgnore]
        public ICollection<Task> Tasks { get; set; }
    }
}