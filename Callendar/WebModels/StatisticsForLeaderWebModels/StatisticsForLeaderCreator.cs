using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Callendar.WebModels.StatisticsForLeaderWebModels
{
    public class StatisticsForLeaderCreator
    {
        private readonly CallendarDbContext _context;
        private readonly int _teamId;
        public StatisticsForLeaderCreator(CallendarDbContext context, int teamId)
        {
            _context = context;
            _teamId = teamId;
        }

        public async Task<StatisticsForLeaderWebModel> GetStatisticsForLeader() =>
            new StatisticsForLeaderWebModel
            {
                MarketerTasksStatistics = await GetMarketerTasksStatistics()
            };

        private async Task<IEnumerable<MarketerTasksStatisticsWebModel>> GetMarketerTasksStatistics()
        {
            var marketers = await _context.Users
                .Include(x => x.Position)
                .Include(x => x.Tasks).ThenInclude(x => x.TaskCategory)
                .Where(x => x.Position.Name.Equals("Marketer") && x.TeamId.Equals(_teamId))
                .ToListAsync();

            var tasksCategories = await _context.TaskCategories.ToListAsync();

            return CreateListOfMarketerTasksStatistics(marketers, tasksCategories);
        }

        private IEnumerable<MarketerTasksStatisticsWebModel> CreateListOfMarketerTasksStatistics(IEnumerable<User> marketers, IEnumerable<TaskCategory> tasksCategories)
        {
            var marketersTasksStatistics = new List<MarketerTasksStatisticsWebModel>();

            foreach (var marketer in marketers)
            {
                var marketerTasksStatistics = new MarketerTasksStatisticsWebModel()
                {
                    FirstName = marketer.FirstName,
                    LastName = marketer.LastName,
                    TaskCategoryStatistics = GetTaskCategoryStatistics(marketer, tasksCategories)
                };

                marketersTasksStatistics.Add(marketerTasksStatistics);
            }

            return marketersTasksStatistics;
        }

        private IEnumerable<TaskCategoryStatisticsWebModel> GetTaskCategoryStatistics(User marketer, IEnumerable<TaskCategory> tasksCategories)
        {
            var tasksCategoriesStatistics = new List<TaskCategoryStatisticsWebModel>();

            foreach (var tasksCategory in tasksCategories)
            {
                var tasksCategoryStatistics = new TaskCategoryStatisticsWebModel()
                {
                    CategoryName = tasksCategory.Name,
                    TasksDone = marketer.Tasks.Count(x => x.IsClosed && x.TaskCategory.Equals(tasksCategory)),
                    TasksToDo = marketer.Tasks.Count(x => !x.IsClosed && x.TaskCategory.Equals(tasksCategory))
                };

                tasksCategoriesStatistics.Add(tasksCategoryStatistics);
            }

            return tasksCategoriesStatistics;
        }
    }
}
