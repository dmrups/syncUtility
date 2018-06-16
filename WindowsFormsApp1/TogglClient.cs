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

        public TogglClient()
        {
            var apiKey = "8e19e58fb9bf1971cd94b33978e861ad";
            timeSrv = new TimeEntryService(apiKey);
        }

        public Dictionary<long, string> Projects { get; } = new Dictionary<long, string> { { 110749977, "KDL" }, { 110735396, "VELAN" } };

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

            var result = hours.GroupBy(x => DateTime.Parse(x.Start).Date)
                .SelectMany(x => x.GroupBy(y => y.Description).Select(z => new TaskReport
                {
                    Description = z.Key,
                    Date = DateTime.Parse(z.First().Start).Date,
                    Duration = TimeSpan.FromSeconds(z.Sum(a => a.Duration ?? 0)),
                    ProjectName = Projects[z.First().ProjectId.Value],
                    TaskId = z.First().TagNames.FirstOrDefault()
                }));

            return result;
        }
    }
}
