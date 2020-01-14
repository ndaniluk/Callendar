using System.Collections.Generic;

namespace Callendar.Export.Excel
{
    public class ExcelDocument
    {
        public  IEnumerable<EmployeeExportData> EmployeesData { get; set; }
    }
}
