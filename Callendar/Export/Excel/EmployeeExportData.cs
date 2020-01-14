using System;

namespace Callendar.Export.Excel
{
    public class EmployeeExportData
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PositionName { get; set; }
        public int Points { get; set; }
        public int L4Days { get; set; }
        public int VacationDays { get; set; }
        public int MaxVacationDays { get; set; }
    }
}
