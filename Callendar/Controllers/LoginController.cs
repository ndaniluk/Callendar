using System;
using System.Linq;
using System.Threading.Tasks;
using Callendar.Helpers.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Callendar.Controllers
{
    [Route("login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly CallendarDbContext _context;

        public LoginController(CallendarDbContext context)
        {
            _context = context;
        }

        // POST: login
        [HttpPost]
        public async Task<ActionResult<Guid>> AuthorizeUser([FromBody] User userInfo)
        {
            var user = await _context.Users
                .Where(x => x.Email.Equals(userInfo.Email))
                .SingleOrDefaultAsync();

            var usersHelper = new UsersHelper(_context);

            if (await usersHelper.VerifyPassword(userInfo.Email, userInfo.Password))
            {
                return new OkObjectResult(user.Id);
            }
            return new ForbidResult();
        }
    }
}