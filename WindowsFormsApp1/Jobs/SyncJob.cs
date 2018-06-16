using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Jobs
{
    internal class SyncJob : IJob
    {
        TogglClient toggl;
        GSheetClient sheet;

        public SyncJob()
        {
            toggl = new TogglClient();
            sheet = new GSheetClient();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            foreach (var projectName in toggl.Projects.Values)
            {
                try
                {
                    var lastDate = await sheet.GetLastDateAsync(projectName);

                    if (lastDate >= DateTime.Now.Date.Subtract(TimeSpan.FromDays(1)))
                    {
                        continue;
                    }

                    var intervalStart = lastDate.AddDays(1);
                    var intervalEnd = DateTime.Now.Date.Subtract(TimeSpan.FromDays(1)).AddHours(23).AddMinutes(59);

                    var hours = toggl.GetHours(intervalStart, intervalEnd).Where(x => x.ProjectName == projectName);

                    // row format: date / work item id / description / hours
                    var data = hours.Select(x => (IList<object>)new List<object>
                {
                    x.Date.ToString("dd.MM.yyyy"),
                    x.TaskId,
                    x.Description,
                    Math.Round(x.Duration.TotalHours, 1)
                });

                    await sheet.InsertRowsAsync(data.ToList(), projectName);
                }
                catch(Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
