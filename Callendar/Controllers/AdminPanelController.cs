using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Callendar.Controllers
{
    [Route("users")]
    [ApiController]
    public class AdminPanelController : ControllerBase
    {
        private readonly CallendarDbContext _context;

        public AdminPanelController(CallendarDbContext context)
        {
            _context = context;
        }

        // GET: users/{liderGuid}/adminPanel
        [HttpGet("{liderGuid}/adminPanel")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: users/{liderGuid}/adminPanel/{userGuid}
        [HttpGet("{liderGuid}/adminPanel/{userGuid}")]
        public async Task<ActionResult<User>> GetUser(Guid liderGuid, Guid userGuid)
        {
            var user = await _context.Users.FindAsync(userGuid);

            if (user == null) return NotFound();

            return user;
        }

        // PUT: users/{liderGuid}/adminPanel/{userGuid}
        [HttpPut("{liderGuid}/adminPanel/{userGuid}")]
        public async Task<IActionResult> PutUser(Guid liderGuid, Guid userGuid, User user)
        {
            if (userGuid != user.Id) return BadRequest();

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(userGuid))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // POST: users/{liderGuid}/adminPanel
        [HttpPost("{liderGuid}/adminPanel")]
        public async Task<ActionResult<User>> PostUser(Guid liderGuid, User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new {id = user.Id}, user);
        }

        // DELETE: users/{liderGuid}/adminPanel/{userGuid}
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