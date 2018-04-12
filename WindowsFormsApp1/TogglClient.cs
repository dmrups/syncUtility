using System;
using System.Collections.Generic;
using System.Linq;
using Toggl;
using Toggl.QueryObjects;
using Toggl.Services;
using WindowsFormsApp1.Entities;

namespace WindowsFormsApp1
{
    class TogglClient
    {
        private TimeEntryService timeSrv;
        private readonly int KdlId = 110749977;
        private readonly int VelanId = 110735396;

        public TogglClient()
        {
            var apiKey = "8e19e58fb9bf1971cd94b33978e861ad";
            timeSrv = new TimeEntryService(apiKey);

        }

        public IEnumerable<TaskReport> GetHours(DateTime start, DateTime end)
        {
            var prams = new TimeEntryParams();

            // there is an issue with the date ranges have
            // to step out of the range on the end.
            // To capture the end of the billing range day + 1
            prams.StartDate = start;
            prams.EndDate = end;

            var hours = timeSrv.List(prams)
                .Where(w => !string.IsNullOrEmpty(w.Description));

            var result = hours.GroupBy(x => x.Description).Select(x => new TaskReport
            {
                Description = x.Key,
                Date = DateTime.Parse(x.First().Start).Date,
                Duration = TimeSpan.FromSeconds(x.Sum(y => y.Duration ?? 0)),
                ProjectName = x.First().ProjectId == KdlId ? "KDL" : "VELAN",
                TaskId = x.First().TagNames.FirstOrDefault()
            });

            return result;
        }
    }
}
