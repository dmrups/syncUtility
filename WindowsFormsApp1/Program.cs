using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace MyTrayApp
{
    public class SysTrayApp : Form
    {
        Pinger pinger;
        TogglClient toggl;
        GSheetClient sheet;

        [STAThread]
        public static void Main()
        {
            Application.Run(new SysTrayApp());
        }

        public SysTrayApp()
        {
            //pinger = new Pinger(OnExit);
            toggl = new TogglClient();
            sheet = new GSheetClient();

            //var hours = toggl.GetHours(new DateTime(2018,4,1), DateTime.Now.Date);

            //var kdlHours = hours.Where(x => x.ProjectName == "KDL");
            //var velanHours = hours.Where(x => x.ProjectName == "VELAN");

            //// row format: date / work item id / description / hours
            //var kdlData = kdlHours.Select(x => (IList<object>)new List<object>
            //{
            //    x.Date.ToString("dd.MM.yyyy"),
            //    x.TaskId,
            //    x.Description,
            //    Math.Round(x.Duration.TotalHours, 1)
            //});

            //var velanData = velanHours.Select(x => (IList<object>)new List<object>
            //{
            //    x.Date.ToString("dd.MM.yyyy"),
            //    x.TaskId,
            //    x.Description,
            //    Math.Round(x.Duration.TotalHours, 1)
            //});

            //sheet.InsertRows(kdlData.ToList(), "KDL");
            //sheet.InsertRows(velanData.ToList(), "VELAN");
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                pinger.Dispose();
            }

            base.Dispose(isDisposing);
        }
    }
}