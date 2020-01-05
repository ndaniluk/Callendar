using System;
using Newtonsoft.Json;

namespace Callendar
{
    public class TakenAbsence
    {
        public int Id { get; set; }
        public int DaysCount { get; set; }
        public bool IsAccepted { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [JsonIgnore]
        public int AbsenceId { get; set; }
        public Absence Absence { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}