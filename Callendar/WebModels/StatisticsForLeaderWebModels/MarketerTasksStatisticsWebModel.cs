using System.Collections.Generic;

namespace Callendar.WebModels.StatisticsForLeaderWebModels
{
    public class MarketerTasksStatisticsWebModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<TaskCategoryStatisticsWebModel> TaskCategoryStatistics { get; set; }
    }
}
