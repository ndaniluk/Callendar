using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

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
		public ActionResult<Guid> AuthorizeUser(User usersEmailAndPassword)
		{
			var users = _context.Users.Where(u =>
				u.Email.Equals(usersEmailAndPassword.Email) &&
				u.Password.Equals(usersEmailAndPassword.Password));

			return users.Any() ? (ActionResult<Guid>)Ok(users.First().Id) : (ActionResult<Guid>)Unauthorized();
		}
	}
}