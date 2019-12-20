using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
		[HttpGet("{guid}/calendar")]
		public async Task<ActionResult<User>> GetUserCalendar(Guid guid)
		{
			//TODO
			return null;
		}
	}
}