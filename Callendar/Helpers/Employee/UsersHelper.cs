using System;
using System.Linq;
using System.Security.Cryptography;
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

        public async Task<bool> IsGuidCorrect(Guid userId)
        {
            var user = await _context.Users
                .Where(x => x.Id == userId)
                .SingleAsync();

            return user != null;
        }

        public async Task<bool> IsLeader(Guid userId)
        {
            var user = await _context.Users
                .Include(x => x.Position)
                .Where(x => x.Id == userId && x.Position.Name == "Leader")
                .SingleOrDefaultAsync();

            return user != null;
        }

        public async Task<bool> IsAccountant(Guid userId)
        {
            var user = await _context.Users
                .Include(x => x.Position)
                .Where(x => x.Id == userId && x.Position.Name == "Accountant")
                .SingleOrDefaultAsync();

            return user != null;
        }

        public async Task<bool> IsAlreadyRegistered(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        public byte[] HashPassword(string password)
        {
            var provider = new RNGCryptoServiceProvider();
            var salt = new byte[8];
            provider.GetBytes(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
            return pbkdf2.GetBytes(20);
        }

        public async Task<bool> IsLimitOfOnDemand(Guid userId)
        {
            var count = await _context.Users
                .Where(x => x.Id == userId)
                .SelectMany(x => x.TakenAbsences)
                .CountAsync(x => x.Absence.Name == "onDemand");

            return count < 4;
        }
    }
}