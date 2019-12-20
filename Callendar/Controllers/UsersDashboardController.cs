using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Callendar.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Callendar.Controllers
{
	[Route("users")]
	[ApiController]
	public class UsersDashboardController : ControllerBase
	{
		private readonly CallendarDbContext _context;

		public UsersDashboardController(CallendarDbContext context)
		{
			_context = context;
		}

		// GET: users/{guid}/dashboard
		[HttpGet("{guid}/dashboard")]
		public async Task<ActionResult<IEnumerable<string>>> GetUserDashboard(Guid guid)
		{
			var user = await _context.Users.FindAsync(guid);
			user.Position = await _context.Permissions.FindAsync(user.PositionId);

			return user == null ? 
				(ActionResult<IEnumerable<string>>)NotFound() : 
				(ActionResult<IEnumerable<string>>)Ok(DashboardsJsonHelper.GetDashboard(user));
		}

		// POST: users/{guid}/dashboard/absencePeriod/{startDate}/{endDate}/absenceType/{absenceTypeId}
		[HttpPost("{guid}/dashboard/absencePeriod/{startDate}/{endDate}/absenceType/{absenceTypeId}")]
		public async Task<ActionResult<TakenAbsence>> PostAbsense(Guid guid, DateTime startDate, DateTime endDate, int absenceTypeId)
		{
			//TODO
			return null;
		}

		// PUT: users/{guid}/dashboard/absenceRequests/{absenceTypeId}/abesnceStatus/{isAccepted}
		[HttpPut("{guid}/dashboard/absenceRequests/{absenceTypeId}/abesnceStatus/{isAccepted}")]
		public async Task<ActionResult<TakenAbsence>> PutAbsenseAcceptance(Guid guid, int absenceTypeId, bool isAccepted)
		{
			//TODO
			return null;
		}
	}
}