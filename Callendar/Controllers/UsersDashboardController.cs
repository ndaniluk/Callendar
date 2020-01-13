using System;
using System.Linq;
using System.Threading.Tasks;
using Callendar.Helpers.Dashboard;
using Callendar.Helpers.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Callendar.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersDashboardController : ControllerBase
    {
        private readonly CallendarDbContext _context;
        private readonly DashboardsJsonHelper _dashboardsJsonHelper;

        public UsersDashboardController(CallendarDbContext context)
        {
            _context = context;
            _dashboardsJsonHelper = new DashboardsJsonHelper(context);
        }

        //Returns basic information that will appear in dashboard
        [HttpGet("{userId}/dashboard")]
        public async Task<ActionResult<User>> GetUserDashboard(Guid userId)
        {
            var user = await _dashboardsJsonHelper.GetDashboard(userId);
            if (user == null) return new NotFoundResult();

            return new OkObjectResult(user);
        }

        //Adds new absence
        [HttpPost("{userId}/dashboard/absence/{startDate}/{endDate}/type/{absenceType}")]
        public async Task<ActionResult<TakenAbsence>> AddAbsence(Guid userId, DateTime startDate, DateTime endDate,
            string absenceType)
        {
            var usersHelper = new UsersHelper(_context);
            if (!await usersHelper.IsGuidCorrect(userId)) return new NotFoundResult();

            if (absenceType == "onDemand" && !await usersHelper.IsLimitOfOnDemand(userId))
            {
                return new OkObjectResult("On demand absences limit has been reached");
            }

            if (DateTime.Compare(startDate, endDate) > 0)
            {
                return new OkObjectResult("Ending date is earlier than starting date");
            }
            
            var user = await _context.Users.Where(x => x.Id == userId).SingleOrDefaultAsync();
            
            if (user.VacationDaysLeft <= 0) return new OkObjectResult("You have reached the limit of your absences");

            var newTakenAbsence = new TakenAbsence
            {
                User = await _context.Users.Where(x => x.Id == userId).SingleOrDefaultAsync(),
                Absence = await _context.Absences
                    .Where(x => x.Name == absenceType)
                    .SingleOrDefaultAsync(),
                IsAccepted = false,
                StartDate = startDate,
                EndDate = endDate,
                DaysCount = (int) (endDate - startDate).TotalDays + 1
            };

            _context.TakenAbsences.Add(newTakenAbsence);
            await _context.SaveChangesAsync();

            return new OkObjectResult(newTakenAbsence);
        }

        //Accepts an absence
        [HttpPut("{userId}/dashboard/absence/{absenceId}")]
        public async Task<ActionResult<TakenAbsence>> AcceptAbsence(Guid userId, int absenceId)
        {
            var usersHelper = new UsersHelper(_context);
            if (!await usersHelper.IsGuidCorrect(userId)) return new NotFoundResult();
            var absence = await _context.TakenAbsences
                .Where(x => x.Id == absenceId)
                .SingleAsync();

            var user = await _context.Users.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.VacationDaysLeft -= 1;
            _context.Users.Update(user);
            
            if (absence == null) return new NotFoundResult();

            absence.IsAccepted = true;
            _context.TakenAbsences.Update(absence);
            
            await _context.SaveChangesAsync();
            return new OkObjectResult(absence);
        }

        //Removes an absence
        [HttpDelete("{userId}/dashboard/absence/{absenceId}")]
        public async Task<ActionResult<TakenAbsence>> DeleteAbsence(Guid userId, int absenceId)
        {
            var usersHelper = new UsersHelper(_context);
            if (!await usersHelper.IsGuidCorrect(userId)) return new NotFoundResult();
            var absence = await _context.TakenAbsences
                .Where(x => x.Id == absenceId)
                .SingleAsync();

            if (absence == null) return new NotFoundResult();

            _context.TakenAbsences.Remove(absence);
            if (await _context.SaveChangesAsync() > 0) return new OkObjectResult(absence);

            return new NotFoundResult();
        }
        
        //Returns count of not accepted absences
        [HttpGet("{userId}/dashboard/absence/count")]
        public async Task<ActionResult<TakenAbsence>> GetNotAcceptedAbsencesCount(Guid userId)
        {
            var usersHelper = new UsersHelper(_context);
            if (!await usersHelper.IsGuidCorrect(userId)) return new NotFoundResult();
            var notAcceptedAbsencesCount = await _context.TakenAbsences
                .Where(x => x.IsAccepted == false)
                .CountAsync();

            return new OkObjectResult(notAcceptedAbsencesCount);
        }
    }
}