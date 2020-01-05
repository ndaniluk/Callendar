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

        [HttpPost("{guid}/dashboard/absencePeriod/{startDate}/{endDate}/absenceType/{absenceType}")]
        public async Task<ActionResult<TakenAbsence>> AddAbsence(Guid guid, DateTime startDate, DateTime endDate,
            string absenceType)
        {
            var usersHelper = new UsersHelper(_context);
            if (await usersHelper.IsGuidCorrect(guid))
            {
                return new OkObjectResult(_context.TakenAbsences.Add(new TakenAbsence()
                {
                    UserId = guid,
                    AbsenceId = await _context.Absences.Where(x => x.Name == absenceType).Select(x => x.Id).SingleOrDefaultAsync(),
                    IsAccepted = false,
                    StartDate = startDate,
                    EndDate = endDate,
                    DaysCount = (int) (endDate - startDate).TotalDays
                }));
            }
            return new NotFoundResult();
        }

        [HttpPut("{guid}/dashboard/absenceRequests/{absenceTypeId}/absenceStatus/{isAccepted}")]
        public async Task<ActionResult<TakenAbsence>> AcceptAbsence(Guid guid, int absenceTypeId,
            bool isAccepted)
        {
            //TODO
            return null;
        }
    }
}