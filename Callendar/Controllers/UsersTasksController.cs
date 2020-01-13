using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Callendar.Models;
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

            if (user == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(user);
        }

        // GET: users/{guid}/telemarketerStatistics
        [HttpGet("{guid}/telemarketerStatistics")]
        public async Task<ActionResult<TelemarketerStatistics>> GetTelemarketerStatistics(Guid guid)
        {
            var user = await _context.Users
                .Where(x => x.Id == guid).SingleOrDefaultAsync();

            if (user == null)
            {
                return new NotFoundResult();
            }


            var tasks = await _context.Tasks
                .Where(x => x.UserId == guid)
                .Include(x => x.TaskCategory)
                .ToListAsync();

            int pointsToGet = 0;

            foreach (var task in tasks.Where(t => t.IsClosed == false))
                pointsToGet += task.TaskCategory.ScorePoints;

            TelemarketerStatistics telemarketerStatistics = new TelemarketerStatistics();

            telemarketerStatistics.PointsToGet = pointsToGet;
            telemarketerStatistics.TasksToDoCount = tasks.Where(x => x.IsClosed == false).Count();
            telemarketerStatistics.TasksDoneCount = tasks.Where(x => x.IsClosed).Count();
            telemarketerStatistics.Points = user.Points;

            return new OkObjectResult(telemarketerStatistics);
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
                .Where(x => x.TeamId == leader.TeamId && x.PositionId == 1)
                .ToListAsync();

            if (teamMembers == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(teamMembers);
        }

        // GET: users/{guid}/tasksCategories
        [HttpGet("{guid}/tasksCategories")]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasksCategories(Guid guid)
        {
            var tasksCategories = await _context.TaskCategories
                 .ToListAsync();

            if (tasksCategories == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(tasksCategories);
        }

        // GET: users/{guid}/tasks
        [HttpGet("{guid}/tasks")]
        public async Task<ActionResult<IEnumerable<Task>>> GetTelemarketerTasks(Guid guid)
        {
            var tasks = await _context.Tasks
                .Where(x => x.UserId == guid)
                .Include(x => x.TaskCategory)
                .ToListAsync();

            if (tasks == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(tasks);
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


            var task = await _context.Tasks
                .Where(x => x.Id == taskId)
                .Include(x => x.TaskCategory)
                .SingleOrDefaultAsync();

            if (task == null)
            {
                return new NotFoundResult();
            }

            task.IsClosed = true;
            user.Points += task.TaskCategory.ScorePoints;


            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return Ok();
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