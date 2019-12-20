using System;
using System.Threading.Tasks;
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
		public async Task<ActionResult<User>> GetUserDashboard(Guid guid)
		{
			//TODO
			return null;
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