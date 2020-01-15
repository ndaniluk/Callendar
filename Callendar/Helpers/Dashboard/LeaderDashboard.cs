using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Callendar.Helpers
{
    public class LeaderDashboard : IUserDashboard
    {
        public async Task<User> GetDashboardInfo(CallendarDbContext context, Guid guid)
        {
            return await context.Users
                .Where(x => x.Id == guid)
                .Include(x => x.TakenAbsences).ThenInclude(x => x.Absence)
                .Include(x => x.Position)
                .Include(x => x.Team)
                .SingleOrDefaultAsync();
        }
    }
}