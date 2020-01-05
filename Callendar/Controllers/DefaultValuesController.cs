using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Callendar.Controllers
{
    [Route("defaults")]
    [ApiController]
    public class DefaultValuesController
    {
        private readonly CallendarDbContext _context;

        public DefaultValuesController(CallendarDbContext context)
        {
            _context = context;
        }
        
        [HttpGet("addValues")]
        public async Task<ActionResult> AddValues()
        {
            _context.Users.Add(new User()
            {
                Id = Guid.Parse("1f99527f-239f-4dbf-62e3-08d7855770ea"),
                FirstName = "Norbert",
                LastName = "Daniluk",
                Password = "SUPERHASH5000",
                Email = "norbert@daniluk.dev",
                Points = 50,
                VacationDaysLeft = 21,
                PhotoPath = "C:/BrakZdjecia",
                PositionId = 1,
                TeamId = 1
            });

            _context.Absences.Add(new Absence()
            {
                Name = "sick",
                IsWork = true,
                SalaryPercent = 80,
                RepresentingColor = "#FA3847"
            });

            _context.Permissions.Add(new Position()
            {
                Name = "Marketer"
            });
            
            _context.Permissions.Add(new Position()
            {
                Name = "Accountant"
            });
            
            _context.Permissions.Add(new Position()
            {
                Name = "Leader"
            });

            _context.TakenAbsences.Add(new TakenAbsence()
            {
                AbsenceId = 1,
                StartDate = DateTime.Parse("2020-02-01"),
                EndDate = DateTime.Parse("2020-02-06"),
                DaysCount = 5,
                IsAccepted = false,
                UserId = Guid.Parse("1f99527f-239f-4dbf-62e3-08d7855770ea")
            });

            _context.TaskCategories.Add(new TaskCategory()
            {
                Name = "Call to 50 clients",
                Description = "Make 50 calls with random generated clients",
                ScorePoints = 50
            });

            _context.Tasks.Add(new Task()
            {
                IsClosed = false,
                TaskCategoryId = 1,
                UserId = Guid.Parse("1f99527f-239f-4dbf-62e3-08d7855770ea")
            });

            _context.Teams.Add(new Team()
            {
                Name = "Devs"
            });

            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}