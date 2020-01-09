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
            var newTakenAbsence = new TakenAbsence
            {
                User = await _context.Users.Where(x => x.Id == userId).SingleOrDefaultAsync(),
                Absence = await _context.Absences
                    .Where(x => x.Name == absenceType)
                    .SingleOrDefaultAsync(),
                IsAccepted = false,
                StartDate = startDate,
                EndDate = endDate,
                DaysCount = (int) (endDate - startDate).TotalDays
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

            if (absence == null) return new NotFoundResult();

            absence.IsAccepted = true;
            _context.TakenAbsences.Update(absence);
            if (await _context.SaveChangesAsync() > 0) return new OkObjectResult(absence);

            return new NotFoundResult();
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
    }
}