using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Callendar.Export.Excel
{
    public class ExcelDocumentCreator
    {
        private readonly CallendarDbContext _context;
        public ExcelDocumentCreator(CallendarDbContext context)
        {
            _context = context;
        }

        public async Task<ExcelDocument> GetExcelDocument() =>
            new ExcelDocument
            {
                EmployeesData = await GetEmployeesData()
            };

        private async Task<IEnumerable<EmployeeExportData>> GetEmployeesData()
        {
            var employees = await _context.Users
                .Include(x => x.Position)
                .Include(x => x.TakenAbsences).ThenInclude(x => x.Absence)
                .ToListAsync();

            return CreateListOfEmployeeExportData(employees);
        }

        private IEnumerable<EmployeeExportData> CreateListOfEmployeeExportData(IEnumerable<User> employees)
        {
            var employeesExportData = new List<EmployeeExportData>();

            foreach (var employee in employees)
            {
                var employeeExportData = new EmployeeExportData()
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Points = employee.Points,
                    PositionName = employee.Position.Name,
                    L4Days = CountTakenVacationThisMonth(employee.TakenAbsences, "Chorobowy"),
                    MaxVacationDays = employee.MaxVacationDays,
                    VacationDays = CountTakenVacationThisMonth(employee.TakenAbsences, "Wypoczynkowy")
                };

                employeesExportData.Add(employeeExportData);
            }

            return employeesExportData;
        }

        private int CountTakenVacationThisMonth(IEnumerable<TakenAbsence> employeeTakenAbsences, string vacationName)
        {
            var vacationAbsences = employeeTakenAbsences.Where(x => x.Absence.Name.Equals(vacationName));

            return vacationAbsences.Sum(x => x.DaysCount);
        }
    }
}
