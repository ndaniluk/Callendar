using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Callendar.Helpers.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Callendar.Controllers
{
    [Route("admin")]
    [ApiController]
    public class AdminPanelController : ControllerBase
    {
        private readonly CallendarDbContext _context;

        public AdminPanelController(CallendarDbContext context)
        {
            _context = context;
        }

        //Adds new user to the same team leader is in
        [HttpPost("{leaderId}/user")]
        public async Task<ActionResult<User>> AddUser(Guid leaderId, [FromBody] User newUser)
        {
            var userHelper = new UsersHelper(_context);
            if (!await userHelper.IsGuidCorrect(leaderId) || !await userHelper.IsLeader(leaderId))
                return new OkObjectResult("You don't have needed permissions to add new user");

            if (await userHelper.IsAlreadyRegistered(newUser.Email))
                return new OkObjectResult("User already registered");
            
            newUser.Password = userHelper.HashPassword(newUser.Password);
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return new OkObjectResult(newUser);
        }

        [HttpDelete("{leaderId}/adminPanel/{userId}")]
        public async Task<ActionResult<User>> DeleteUser(Guid leaderId, Guid userId)
        {
            var userHelper = new UsersHelper(_context);
            if (!await userHelper.IsGuidCorrect(leaderId) || !await userHelper.IsLeader(leaderId))
                return new NotFoundResult();

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        [HttpPut("{leaderId}/adminPanel/password/{userId}")]
        public async Task<ActionResult<User>> ChangePassword(Guid leaderId, Guid userId, [FromBody] string newPassword)
        {
            var userHelper = new UsersHelper(_context);
            if (!await userHelper.IsGuidCorrect(leaderId) || !await userHelper.IsGuidCorrect(userId) || !await userHelper.IsLeader(leaderId))
                return new NotFoundResult();

            var user = await _context.Users
                .Where(x => x.Id == userId)
                .SingleOrDefaultAsync();

            user.Password = userHelper.HashPassword(newPassword);

            return new OkObjectResult(user);
        }

        [HttpGet("{leaderId}/adminPanel/tasks")]
        public async Task<ActionResult<List<Task>>> GetTasksFromTeam(Guid leaderId)
        {
            var userHelper = new UsersHelper(_context);
            if (!await userHelper.IsGuidCorrect(leaderId) || !await userHelper.IsLeader(leaderId))
                return new NotFoundResult();

            var leader = await _context.Users
                .Where(x => x.Id == leaderId)
                .SingleOrDefaultAsync();
                
            return new OkObjectResult(_context.Tasks
                .Where(x => x.User.TeamId == leader.TeamId)
                .ToListAsync());
        }
    }
}