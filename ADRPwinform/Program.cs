using AnimeDiscordRichPresence;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace ADRPwinform
{
    static class Program
    {
        static NotifyIcon notifyIcon = new NotifyIcon();
        static bool Visible = true;
        static ToolStripItem HideButton;
        static void Main(string[] args)
        {
            if (System.Diagnostics.Process.GetProcessesByName(Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1) return;

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
                HideButton = contextMenu.Items.Add("Show Console", null, (s, e) =>
                {
                    Visible = !Visible;
                    SetConsoleWindowVisibility(Visible);
                });

                contextMenu.Items.Add("Exit", null, (s, e) =>
                {
                    MainLogic.Stop();
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

            try
            {
                MainLogic.Run();
            }
            catch (Exception ex)
            {
                File.WriteAllText("Error.txt", ex.ToString());
                notifyIcon.Dispose();
                Application.Exit();
            }
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
                if (visible)
                {
                    if (HideButton != null)
                    {
                        HideButton.Text = "Hide Console";
                    }
                    ShowWindow(hWnd, 1);
                }
                else
                {
                    if (HideButton != null)
                    {
                        HideButton.Text = "Show Console";
                    }
                    ShowWindow(hWnd, 0);
                }
            }
        }
    }
}
