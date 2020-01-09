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
                .Where(x => x.Id == userId)
                .Where(x => x.Position.Name == "Leader")
                .SingleAsync();
            return user != null;
        }

        public async Task<bool> IsAlreadyRegistered(string email)
        {
            var user = await _context.Users
                .Where(x => x.Email == email)
                .SingleAsync();
            return user == null;
        }
        
        public byte[] HashPassword(string password)
        {
            var provider = new RNGCryptoServiceProvider();
            var salt = new byte[8];
            provider.GetBytes(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
            return pbkdf2.GetBytes(20);
        }
    }
}