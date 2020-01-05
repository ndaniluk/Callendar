using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Callendar.Helpers.Dashboard
{
    public class AccountantDashboard : IUserDashboard
    {
        public async Task<User> GetDashboardInfo(CallendarDbContext context, Guid guid)
        {
            return await context.Users
                .Where(x => x.Id == guid)
                .Include(x => x.TakenAbsences)
                .SingleOrDefaultAsync();
        }
    }
}