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
        [HttpGet("{guid}/dashboard")]
        public async Task<ActionResult<User>> GetUserDashboard(Guid guid)
        {
            var user = await _dashboardsJsonHelper.GetDashboard(guid);
            if (user == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(user);
        }

        [HttpPost("{guid}/dashboard/absence/{startDate}/{endDate}/type/{absenceType}")]
        public async Task<ActionResult<TakenAbsence>> AddAbsence(Guid guid, DateTime startDate, DateTime endDate,
            string absenceType)
        {
            var usersHelper = new UsersHelper(_context);
            if (!await usersHelper.IsGuidCorrect(guid)) return new NotFoundResult();
            var newTakenAbsence = new TakenAbsence()
            {
                User = await _context.Users.Where(x => x.Id == guid).SingleOrDefaultAsync(),
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

        [HttpPut("{guid}/dashboard/absence/{absenceId}")]
        public async Task<ActionResult<TakenAbsence>> AcceptAbsence(Guid guid, int absenceId)
        {
            var usersHelper = new UsersHelper(_context);
            if (!await usersHelper.IsGuidCorrect(guid)) return new NotFoundResult();
            var absence = await _context.TakenAbsences
                .Where(x => x.Id == absenceId)
                .SingleAsync();
                
            if (absence == null) return new NotFoundResult();
                
            absence.IsAccepted = true;
            _context.TakenAbsences.Update(absence);
            if (await _context.SaveChangesAsync() > 0)
            {
                return new OkObjectResult(absence);
            }

            return new NotFoundResult();
        }

        [HttpDelete("{guid}/dashboard/absence/{absenceId}")]
        public async Task<ActionResult<TakenAbsence>> DeleteAbsence(Guid guid, int absenceId)
        {
            var usersHelper = new UsersHelper(_context);
            if (!await usersHelper.IsGuidCorrect(guid)) return new NotFoundResult();
            var absence = await _context.TakenAbsences
                .Where(x => x.Id == absenceId)
                .SingleAsync();
            
            if (absence == null) return new NotFoundResult();

            _context.TakenAbsences.Remove(absence);
            if (await _context.SaveChangesAsync() > 0)
            {
                return new OkObjectResult(absence);
            }

            return new NotFoundResult();
        }
    }
}