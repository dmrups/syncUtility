using System;
using System.Windows.Forms;
using WindowsFormsApp1;
using WindowsFormsApp1.Jobs;

namespace MyTrayApp
{
    public class SysTrayApp : Form
    {
        Pinger pinger;

        [STAThread]
        public static void Main()
        {
            Application.Run(new SysTrayApp());
        }

        public SysTrayApp()
        {
            pinger = new Pinger(OnExit);
            Scheduler.StartAsync().Wait();
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