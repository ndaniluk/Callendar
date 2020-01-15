using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Callendar.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersCalendarController : ControllerBase
    {
        private readonly CallendarDbContext _context;

        public UsersCalendarController(CallendarDbContext context)
        {
            _context = context;
        }

        // GET: users/{guid}/calendar
        [HttpGet("{userId}/calendar")]
        public async Task<ActionResult<List<User>>> GetUserCalendar(Guid userId)
        {
            var user = await _context.Users
                .Where(x => x.Id == userId)
                .SingleAsync();

            if (user == null)
            {
                return new NotFoundResult();
            }

            var team = await _context.Users
                .Where(x => x.TeamId == user.TeamId)
                .Include(x => x.TakenAbsences).ThenInclude(x => x.Absence)
                .ToListAsync();

            return new OkObjectResult(team);
        }
    }
}