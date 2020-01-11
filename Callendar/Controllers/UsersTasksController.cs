using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

        // GET: users/{guid}
        [HttpGet("{guid}")]
        public async Task<ActionResult<User>> GetUserInfo(Guid guid)
        {
            var user = await _context.Users
                .Where(x => x.Id == guid)
                .Include(x => x.FirstName)
                .Include(x => x.LastName)
                .Include(x => x.PhotoPath)
                .Include(x => x.Position.Name)
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(user);
        }

        // GET: users/{guid}/telemarketerStatistics
        [HttpGet("{guid}/telemarketerStatistics")]
        public async Task<ActionResult<User>> GetTelemarketerStatistics(Guid guid)
        {
            var user = await _context.Users
                .Where(x => x.Id == guid)
                .Include(x => x.Points)
                .Include(x => x.TasksToDoCount)
                .Include(x => x.TasksDoneCount)
                .Include(x => x.PointsToGet)
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(user);
        }

        // GET: users/{guid}/teamMembers
        [HttpGet("{guid}/teamMembers")]
        public async Task<ActionResult<IEnumerable<User>>> GetTeamMembers(Guid guid)
        {
            var leader = await _context.Users.Where(x => x.Id == guid).SingleOrDefaultAsync();

            if (leader == null)
            {
                return new NotFoundResult();
            }

            var teamMembers = await _context.Users
                .Where(x => x.TeamId == leader.TeamId && x.PositionId != 1)
                .Include(x => x.Id)
                .Include(x => x.FirstName)
                .Include(x => x.LastName)
                .ToListAsync();

            if (teamMembers == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(teamMembers);
        }

        // GET: users/{guid}/tasksCategories
        [HttpGet("{guid}/tasksCategories")]
        public async Task<ActionResult<IEnumerable<User>>> GetTasksCategories(Guid guid)
        {
           var tasksCategories = await _context.TaskCategories
                .Include(x => x.Id)
                .Include(x => x.Name)
                .Include(x => x.ScorePoints)
                .ToListAsync();

            if (tasksCategories == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(tasksCategories);
        }

        // PUT: users/{guid}/tasks/{taskId}
        [HttpPut("{guid}/tasks/{taskId}")]
        public async Task<ActionResult> PutTaskCompleting(Guid guid, int taskId)
        {
            var user = await _context.Users
                .Where(x => x.Id == guid)
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return new NotFoundResult();
            }


            var task = user.Tasks
                .Where(x => x.Id == taskId)
                .SingleOrDefault();

            if (task == null)
            {
                return new NotFoundResult();
            }

            task.IsClosed = true;


            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: users/{guid}/tasks/{taskCategoryId}/telemarketer/{telemarketerGuid}
        [HttpPost("{guid}/tasks/{taskCategoryId}/telemarketer/{telemarketerGuid}")]
        public async Task<ActionResult> PostTasksForTelemarketer(Guid guid, int taskCategoryId, Guid telemarketerGuid)
        {
            var user = await _context.Users
                .Where(x => x.Id == telemarketerGuid)
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return new NotFoundResult();
            }


            var taskCategory = await _context.Tasks
                .Where(x => x.Id == taskCategoryId)
                .SingleOrDefaultAsync();

            if (taskCategory == null)
            {
                return new NotFoundResult();
            }

            var task = new Task()
            {
                IsClosed = false,
                TaskCategoryId = taskCategoryId,
                UserId = telemarketerGuid
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}