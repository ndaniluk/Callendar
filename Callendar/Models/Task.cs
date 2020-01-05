using System;
using Newtonsoft.Json;

namespace Callendar
{
    public class Task
    {
        public int Id { get; set; }
        public bool IsClosed { get; set; }

        [JsonIgnore]
        public int TaskCategoryId { get; set; }
        public TaskCategory TaskCategory { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}