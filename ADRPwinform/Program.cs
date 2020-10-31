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
        static ToolStripItem AboutItem;
        static ToolStripItem PauseButton;
        static void Main(string[] args)
        {
            if (System.Diagnostics.Process.GetProcessesByName(Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1) return;

            if (!MainLogic.Init())
            {
                return;
            }
            UpdateChecker.Check();

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
                contextMenu.Opening += new System.ComponentModel.CancelEventHandler((object sender, System.ComponentModel.CancelEventArgs e) =>
                {
                    if (MainLogic.IsPause)
                    {
                        AboutItem.Text = "Anime Detection Paused.";
                    }
                    else if (MainLogic.lastAnime == null)
                    {
                        AboutItem.Text = "No Anime Detected.";
                    }
                    else
                    {
                        AboutItem.Text = MainLogic.lastAnime.name;
                        AboutItem.Text += string.IsNullOrEmpty(MainLogic.lastAnime.episode) ? "" : $" Episode {MainLogic.lastAnime.episode}";
                        AboutItem.Text += string.IsNullOrEmpty(MainLogic.lastAnime.website) ? "" : $" On {MainLogic.lastAnime.website}";
                    }
                });

                contextMenu.Items.Add($"AnimeDiscordRichPresence {UpdateChecker.currentVersion}").Enabled = false;

                AboutItem = contextMenu.Items.Add("No Anime Detected.");
                AboutItem.Enabled = false;

                contextMenu.Items.Add(new ToolStripSeparator());

                ToolStripMenuItem optionsMenuItem = (ToolStripMenuItem)contextMenu.Items.Add("Options");

                PauseButton = optionsMenuItem.DropDownItems.Add("Pause", null, (s, e) =>
                {
                    if (MainLogic.IsPause)
                    {
                        PauseButton.Text = "Pause";
                        MainLogic.Resume();
                    }
                    else
                    {
                        PauseButton.Text = "Resume";
                        MainLogic.Pause();
                    }
                });

                optionsMenuItem.DropDownItems.Add(new ToolStripSeparator());

                optionsMenuItem.DropDownItems.Add("Open Config", null, (s, e) =>
                {
                    MainLogic.Log("Opening config.json...");
                    if (!File.Exists("config.json"))
                    {
                        MainLogic.Log("config.json not found.");
                        MessageBox.Show("config.json not found.", "ADRP");
                    }
                    try
                    {
                        var startInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/c \"config.json\"")
                        {
                            UseShellExecute = true,
                            WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
                        };
                        System.Diagnostics.Process.Start(startInfo);
                    }
                    catch (Exception ex)
                    {
                        MainLogic.Log("Cannot open config.json.");
                        MessageBox.Show("Cannot open config.json.", "ADRP");
                        Console.WriteLine(ex);
                    }
                });

                optionsMenuItem.DropDownItems.Add("Reload Config", null, (s, e) =>
                {
                    MainLogic.ReloadConfig();
                });

                optionsMenuItem.DropDownItems.Add(new ToolStripSeparator());

                optionsMenuItem.DropDownItems.Add("Force Update", null, (s, e) =>
                {
                    MainLogic.ForceUpdate();
                });

                optionsMenuItem.DropDownItems.Add("Force Reconnect", null, (s, e) =>
                {
                    MainLogic.ForceReconnect();
                });

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
                Console.WriteLine("Uncaught Error Detected.");
                Console.WriteLine(ex);
                SetConsoleWindowVisibility(true);

                Console.ReadKey();
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
