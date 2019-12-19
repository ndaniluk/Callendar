using System;

namespace Callendar
{
    public class TakenAbsence
    {
        public int Id { get; set; }
        public int DaysCount { get; set; }
        public bool IsAccepted { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int AbsenceId { get; set; }
        public Absence Absence { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}