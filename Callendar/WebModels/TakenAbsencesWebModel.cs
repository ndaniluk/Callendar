using System;

namespace Callendar.WebModels
{
    public class TakenAbsencesWebModel
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string BackgroundColor { get; set; }
        public bool IsAccepted { get; set; }
    }
}