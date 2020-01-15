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
                .SingleOrDefaultAsync();

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

        public string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            var hash = pbkdf2.GetBytes(20);
            var hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            var savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }

        public async Task<bool> VerifyPassword(string email, string password)
        {
            var actualPassword = await _context.Users
                .Where(x => x.Email == email)
                .Select(x => x.Password)
                .SingleOrDefaultAsync();
            
            var hashBytes = Convert.FromBase64String(actualPassword);

            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            var hash = pbkdf2.GetBytes(20);

            for (var i=0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }
        public async Task<bool> IsLimitOfOnDemand(Guid userId)
        {
            var count = await _context.Users
                .Where(x => x.Id == userId)
                .SelectMany(x => x.TakenAbsences)
                .CountAsync(x => x.Absence.Name == "Zadanie");

            return count < 4;
        }
    }
}