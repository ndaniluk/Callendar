using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Callendar
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string Email { get; set; }
        public int Points { get; set; }
        public int VacationDaysLeft { get; set; }
        public string PhotoPath { get; set; }

        public ICollection<TakenAbsence> TakenAbsences { get; set; }

        [JsonIgnore]
        public int PositionId { get; set; }
        public Position Position { get; set; }

        public ICollection<Task> Tasks { get; set; }

        [JsonIgnore]
        public int TeamId { get; set; }
        public Team Team { get; set; }

        public int TasksCount => Tasks.Count;
        public int TasksToDoCount => Tasks.Where(x => x.IsClosed == false).Count();
        public int TasksDoneCount => Tasks.Where(x => x.IsClosed).Count();
        public int PointsToGet => CalculatePointsToGet();

        private int CalculatePointsToGet()
        {
            int points = 0;

            foreach (var task in Tasks)
                points += task.TaskCategory.ScorePoints;

            return points;
        }
    }
}