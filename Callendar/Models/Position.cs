using System.Collections.Generic;
using Newtonsoft.Json;

namespace Callendar
{
    public class Position
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore] public ICollection<User> Users { get; set; }
    }
}