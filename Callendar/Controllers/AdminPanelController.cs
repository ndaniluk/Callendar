using System;
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
        [HttpPost("{userId}/user")]
        public async Task<ActionResult<User>> AddUser(Guid userId, [FromBody] User newUser)
        {
            var userHelper = new UsersHelper(_context);
            if (!await userHelper.IsGuidCorrect(userId) || !await userHelper.IsLeader(userId))
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
    }
}