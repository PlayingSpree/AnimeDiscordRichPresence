using System;
using System.Threading;

namespace AnimeDiscordRichPresence
{
    static class MainLogic
    {
        public static AnimeName.Anime lastAnime = null;
        static bool pause = false;
        static bool stop = false;
        public static void Stop() => stop = true;
        public static void Pause()
        {
            Log("Anime detection paused.");
            pause = true;
            if (lastAnime != null)
            {
                lastAnime = null;
                DiscordActivity.Clear();
            }
        }

        public static void Resume()
        {
            Log("Anime detection resumed.");
            pause = false;
        }

        public static bool IsPause => pause;

        public static bool Init()
        {
            if (!Config.Load())
                return false;

            Console.WriteLine("Anime Discord Rich Presence by PlayingSpree.");
            Console.WriteLine("Edit config.json to add new website.");
            return true;
        }
        public static void Run()
        {
            stop = false;
            pause = false;

            Console.WriteLine("Scaning for anime every {0} miliseconds...", Config.program.ScanInterval);
            while (!stop)
            {
                if (!pause)
                {
                    AnimeName.Anime anime = AnimeName.GetAnime();
                    if (anime == null)
                    {
                        if (lastAnime != null)
                        {
                            Log("No anime detected.");
                            DiscordActivity.Clear();
                        }
                    }
                    else
                    {
                        if (lastAnime == null)
                        {
                            Log("Anime detected.");
                            DiscordActivity.Set(anime);
                        }
                        else if (lastAnime.name != anime.name)
                        {
                            Log("New anime detected.");
                            DiscordActivity.Set(anime);
                        }
                        else if (lastAnime.episode != anime.episode)
                        {
                            Log("New episode detected.");
                            DiscordActivity.Set(anime);
                        }
                    }
                    lastAnime = anime;
                }

                int sleepTime = Config.program.ScanInterval;
                do
                {
                    DiscordActivity.Update();
                    if (sleepTime <= 0 || stop)
                    {
                        break;
                    }
                    else
                    {
                        Thread.Sleep(Math.Min(200, sleepTime));
                    }
                    sleepTime -= 200;
                }
                while (sleepTime > 0);
            }
            DiscordActivity.Clear();
        }

        public static void ReloadConfig()
        {
            if (Config.Load())
            {
                Log("Config reloaded.");
            }
            else
            {
                Log("Cannot reload config.");
            }
        }

        public static void ForceUpdate()
        {
            lastAnime = AnimeName.GetAnime();
            if (lastAnime != null)
            {
                Log("Force update anime to discord.");
                DiscordActivity.Set(lastAnime);
            }
            else
            {
                Log("Cannot force update. (No anime detected).");
            }
        }

        public static void ForceReconnect()
        {
            DiscordActivity.Clear();

            lastAnime = AnimeName.GetAnime();
            if (lastAnime != null)
            {
                Log("Force reconnect discord.");
                DiscordActivity.Set(lastAnime);
            }
            else
            {
                Log("Cannot force reconnect. (No anime detected).");
            }
        }

        public static void Log(string text)
        {
            Console.WriteLine(string.Format("[{0}] {1}", DateTime.Now.ToString(), text));
        }
    }
}