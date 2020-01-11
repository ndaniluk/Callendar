using System;
using System.Threading.Tasks;

namespace Callendar.Helpers
{
    public interface IUserDashboard
    {
        Task<User> GetDashboardInfo(CallendarDbContext context, Guid guid);
    }
}