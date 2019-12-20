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
	public class UsersController : ControllerBase
	{
		private readonly CallendarDbContext _context;

		public UsersController(CallendarDbContext context)
		{
			_context = context;
		}

		// GET: users
		[HttpGet]
		public async Task<ActionResult<IEnumerable<User>>> GetUsers()
		{
			return await _context.Users.ToListAsync();
		}

		// GET: users/{guid}
		[HttpGet("{guid}")]
		public async Task<ActionResult<User>> GetUser(Guid guid)
		{
			var user = await _context.Users.FindAsync(guid);

			if (user == null)
			{
				return NotFound();
			}

			return user;
		}

		// PUT: users/{guguid}
		[HttpPut("{guid}")]
		public async Task<IActionResult> PutUser(Guid guid, User user)
		{
			if (guid != user.Id)
			{
				return BadRequest();
			}

			_context.Entry(user).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UserExists(guid))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: users
		[HttpPost]
		public async Task<ActionResult<User>> PostUser(User user)
		{
			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetUser", new { id = user.Id }, user);
		}

		// DELETE: users/{guid}
		[HttpDelete("{guid}")]
		public async Task<ActionResult<User>> DeleteUser(Guid guid)
		{
			var user = await _context.Users.FindAsync(guid);
			if (user == null)
			{
				return NotFound();
			}

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
