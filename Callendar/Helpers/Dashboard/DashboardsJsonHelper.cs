using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Callendar.Helpers.Dashboard
{
    public class DashboardsJsonHelper
    {
        private enum EPosition
        {
            Telemarketer,
            Accountant,
            Leader
        }

        private readonly Dictionary<string, EPosition> _positions = new Dictionary<string, EPosition>
        {
            {"Marketer", EPosition.Telemarketer},
            {"Accountant", EPosition.Accountant},
            {"Leader", EPosition.Leader}
        };

        private readonly CallendarDbContext _context;

        public DashboardsJsonHelper(CallendarDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetDashboard(Guid guid)
        {
            var userPosition = await _context.Users
                .Where(x => x.Id == guid)
                .Select(x => x.Position.Name)
                .SingleOrDefaultAsync();
            
            IUserDashboard dashboard;

            if (_positions.TryGetValue(userPosition, out var position))
                switch (position)
                {
                    case EPosition.Telemarketer:
                        dashboard = new TelemarketerDashboard();
                        break;
                    case EPosition.Accountant:
                        dashboard = new AccountantDashboard();
                        break;
                    case EPosition.Leader:
                        dashboard = new LeaderDashboard();
                        break;
                    default:
                        return null;
                }
            else
                return null;

            return await dashboard.GetDashboardInfo(_context, guid);
        }
    }
}