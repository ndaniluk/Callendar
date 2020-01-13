using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Callendar
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Points { get; set; }
        public int MaxVacationDays { get; set; }
        public int VacationDaysLeft { get; set; }
        public string PhotoPath { get; set; }

        public ICollection<TakenAbsence> TakenAbsences { get; set; }

        public int PositionId { get; set; }

        public Position Position { get; set; }

        public ICollection<Task> Tasks { get; set; }

        public int TeamId { get; set; }

        public Team Team { get; set; }
    }
}