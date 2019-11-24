using System.Collections.Generic;

namespace Callendar
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public int Points { get; set; }
        public int VacationDaysLeft { get; set; }
        public string PhotoPath { get; set; }
        public bool IsLeader { get; set; }
        
        public IList<TakenAbsence> TakenAbsences { get; set; }

        public int PermissionId { get; set; }
        public Permission Permission { get; set; }

        public ICollection<Task> Tasks { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}