using System;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class Pinger : IDisposable
    {
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        private Timer timer;

        private Icon iconGreen;
        private Icon iconRed;
        private Ping ping;
        private long roundtrip = 0;

        public Pinger(Action<object, EventArgs> onExit)
        {
            ping = new Ping();

            // timer
            this.timer = new Timer();
            this.timer.Enabled = true;
            this.timer.Interval = 1000;
            this.timer.Tick += new EventHandler(this.timer_Tick);

            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();

            trayMenu.MenuItems.Add("Exit", (o, ea) => onExit(o, ea));

            iconGreen = new Icon(SystemIcons.Shield, 40, 40);
            iconRed = new Icon(SystemIcons.Error, 40, 40);

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon = new NotifyIcon();
            trayIcon.Icon = iconGreen;

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                var response = ping.Send("ya.ru", 300);

                if (roundtrip == 0)
                {
                    roundtrip = response.RoundtripTime;
                }
                else
                {
                    roundtrip += (response.RoundtripTime - roundtrip) / 3;
                }

                trayIcon.Text = $"{roundtrip.ToString()} ms";

                if (response.Status == IPStatus.Success)
                {
                    trayIcon.Icon = iconGreen;
                }
                else
                {
                    trayIcon.Icon = iconRed;
                }
            }
            catch (Exception ex)
            {
                trayIcon.Icon = iconRed;
                trayIcon.Text = ex.Message;
            }
        }

        public void Dispose()
        {
            // Release the icon resource.
            trayIcon.Dispose();
        }
    }
}
