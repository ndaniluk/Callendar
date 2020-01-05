using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Callendar.Helpers.Employee
{
    public class UsersHelper
    {
        private readonly CallendarDbContext _context;
        
        public UsersHelper(CallendarDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsGuidCorrect(Guid guid)
        {
            var user = await _context.Users.Where(x => x.Id == guid).SingleAsync();
            return user != null;
        }
    }
}