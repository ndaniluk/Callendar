using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Callendar.Helpers
{
    public interface IUserDashboard
    {
        Task<User> GetDashboardInfo(CallendarDbContext context, Guid guid);
    }
}