using System;
using System.Linq;
using System.Threading.Tasks;
using Callendar.Helpers.Employee;
using Microsoft.AspNetCore.Mvc;

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
        
//        //Adds new user to the same team leader is in
//        [HttpPost("{userId}/adminPanel/user")]
//        public async Task<ActionResult<User>> AddUser(Guid userId, [FromBody] User newUser)
//        {
//            var userHelper = new UsersHelper(_context);
//            if (await userHelper.IsGuidCorrect(userId) && await userHelper.IsLeader(userId))
//            {
//                if (!await userHelper.IsAlreadyRegistered(newUser.Email)) return new 
//                
//                _context.Users.Add(new User()
//                {
//
//                });
//            }
//        }

        [HttpDelete("{liderGuid}/adminPanel/{userGuid}")]
        public async Task<ActionResult<User>> DeleteUser(Guid liderGuid, Guid userGuid)
        {
            var user = await _context.Users.FindAsync(userGuid);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(Guid guid)
        {
            return _context.Users.Any(e => e.Id == guid);
        }
    }
}