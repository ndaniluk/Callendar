using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Callendar.Controllers
{
	[Route("users")]
	[ApiController]
	public class UsersTasksController : ControllerBase
	{
		private readonly CallendarDbContext _context;

		public UsersTasksController(CallendarDbContext context)
		{
			_context = context;
		}

		// GET: users/{guid}/tasks
		[HttpGet("{guid}/tasks")]
		public async Task<ActionResult<IEnumerable<Task>>> GetUserTasks(Guid guid)
		{
			//TODO
			return null;
		}

		// PUT: users/{guid}/tasks/{taskId}
		[HttpPut("{guid}/tasks/{taskId}")]
		public async Task<ActionResult> PutTaskCompleting(Guid guid, int taskId)
		{
			//TODO
			return null;
		}

		// POST: users/{guid}/tasks/{taskCategoryId}/telemarketer/{telemarketerGuid}
		[HttpPost("{guid}/tasks/{taskCategoryId}/telemarketer/{telemarketerGuid}")]
		public async Task<ActionResult> PostTasksForTelemarketer(Guid guid, int taskCategoryId, Guid telemarketerGuid)
		{
			//TODO
			return null;
		}
	}
}