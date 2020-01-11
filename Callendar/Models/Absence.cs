using System.Collections.Generic;
using Newtonsoft.Json;

namespace Callendar
{
    public class Absence
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsWork { get; set; }
        public double SalaryPercent { get; set; }
        public string RepresentingColor { get; set; }

        [JsonIgnore] public ICollection<TakenAbsence> TakenAbsences { get; set; }
    }
}