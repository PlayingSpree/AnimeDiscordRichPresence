using AnimeDiscordRichPresence;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace ADRPwinform
{
    static class Program
    {
        static NotifyIcon notifyIcon = new NotifyIcon();
        static bool Visible = true;
        static bool stop = false;
        static void Main(string[] args)
        {
            Thread notifyThread = new Thread(
            delegate ()
            {
                notifyIcon.DoubleClick += (s, e) =>
                {
                    Visible = !Visible;
                    SetConsoleWindowVisibility(Visible);
                };
                notifyIcon.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                notifyIcon.Visible = true;
                notifyIcon.Text = "AnimeDiscordRichPresence";

                var contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("Exit", null, (s, e) => {
                    stop = true;
                    notifyIcon.Dispose();
                    Application.Exit(); 
                });
                notifyIcon.ContextMenuStrip = contextMenu;

                Application.Run();
            });
            MainLogic.Init();

            notifyThread.Start();

            Visible = false;
            SetConsoleWindowVisibility(Visible);

            MainLogic.Run(ref stop);
        }

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public static void SetConsoleWindowVisibility(bool visible)
        {
            IntPtr hWnd = FindWindow(null, Console.Title);
            if (hWnd != IntPtr.Zero)
            {
                if (visible) ShowWindow(hWnd, 1); //1 = SW_SHOWNORMAL           
                else ShowWindow(hWnd, 0); //0 = SW_HIDE               
            }
        }
    }
}
