using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Callendar.Helpers.Employee;
using Callendar.Models;
using Microsoft.AspNetCore.Mvc;
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
                .Include(x => x.Position)
                .SingleOrDefaultAsync();

            if (user == null) return new NotFoundResult();

            return new OkObjectResult(user);
        }

        // GET: users/{guid}/telemarketerStatistics
        [HttpGet("{guid}/telemarketerStatistics")]
        public async Task<ActionResult<TelemarketerStatistics>> GetTelemarketerStatistics(Guid guid)
        {
            var user = await _context.Users
                .Where(x => x.Id == guid).SingleOrDefaultAsync();

            if (user == null) return new NotFoundResult();

            var tasks = await _context.Tasks
                .Where(x => x.UserId == guid)
                .Include(x => x.TaskCategory)
                .ToListAsync();

            var pointsToGet = tasks.Where(t => t.IsClosed == false).Sum(task => task.TaskCategory.ScorePoints);

            var telemarketerStatistics = new TelemarketerStatistics
            {
                PointsToGet = pointsToGet,
                TasksToDoCount = tasks.Count(x => x.IsClosed == false),
                TasksDoneCount = tasks.Count(x => x.IsClosed),
                Points = user.Points
            };

            return new OkObjectResult(telemarketerStatistics);
        }
        
        // GET: users/{guid}/teamMembers
        [HttpGet("{guid}/teamMembers")]
        public async Task<ActionResult<IEnumerable<User>>> GetTeamMembers(Guid userId)
        {
            var leader = await _context.Users.Where(x => x.Id == userId).SingleOrDefaultAsync();

            if (leader == null) return new NotFoundResult();

            var teamMembers = await _context.Users
                .Include(x => x.Position)
                .Where(x => x.TeamId == leader.TeamId)
                .Where(x => x.Id == userId)
                .Where(x => x.Position.Name == "Marketer")
                .ToListAsync();
            
            if (teamMembers == null) return new NotFoundResult();

            return new OkObjectResult(teamMembers);
        }

        // GET: users/{guid}/tasksCategories
        [HttpGet("{guid}/tasksCategories")]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasksCategories(Guid guid)
        {
            var tasksCategories = await _context.TaskCategories.ToListAsync();

            if(tasksCategories == null) return new NotFoundResult();
            return new OkObjectResult(tasksCategories);
        }

		// GET: users/{userId}/tasks
		[HttpGet("{userId}/tasks")]
        public async Task<ActionResult<IEnumerable<Task>>> GetTelemarketerTasks(Guid userId)
        {
            var userHelper = new UsersHelper(_context);
            if (!await userHelper.IsGuidCorrect(userId)) return new NotFoundResult();

            var tasks = await _context.Tasks
                .Where(x => x.UserId == userId)
                .Include(x => x.TaskCategory)
                .ToListAsync();

            return tasks == null
                ? new OkObjectResult("No tasks assigned")
                : new OkObjectResult(tasks);
        }

        // PUT: users/{guid}/tasks/{taskId}
        [HttpPut("{guid}/tasks/{taskId}")]
        public async Task<ActionResult> PutTaskCompleting(Guid guid, int taskId)
        {
            var user = await _context.Users
                .Where(x => x.Id == guid)
                .SingleOrDefaultAsync();

            if (user == null) return new NotFoundResult();

            var task = await _context.Tasks
                .Where(x => x.Id == taskId)
                .Include(x => x.TaskCategory)
                .SingleOrDefaultAsync();

            if (task == null) return new NotFoundResult();

            task.IsClosed = true;
            user.Points += task.TaskCategory.ScorePoints;

            _context.Entry(task).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return new OkObjectResult(task);
        }

		// POST: users/{userId}/tasks/{taskCategoryId}/telemarketer/{telemarketerGuid}
		[HttpPost("{userId}/tasks/{taskCategoryId}/telemarketer/{telemarketerGuid}")]
        public async Task<ActionResult> PostTasksForTelemarketer(Guid userId, int taskCategoryId, Guid telemarketerGuid)
        {
            var userHelper = new UsersHelper(_context);
            if (!await userHelper.IsGuidCorrect(userId)) return new NotFoundResult();

            var user = await _context.Users
                .Where(x => x.Id == telemarketerGuid)
                .SingleOrDefaultAsync();

            if (user == null) return new NotFoundResult();

            var taskCategory = await _context.TaskCategories
                .Where(x => x.Id == taskCategoryId)
                .SingleOrDefaultAsync();

            if (taskCategory == null) return new NotFoundResult();

            var task = new Task
            {
                IsClosed = false,
                TaskCategoryId = taskCategoryId,
                UserId = telemarketerGuid
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return new OkObjectResult(task);
        }
    }
}