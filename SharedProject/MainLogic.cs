using System;
using System.Threading;

namespace AnimeDiscordRichPresence
{
    static class MainLogic
    {
        static bool stop = false;
        public static void Stop() => stop = true;

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
            AnimeName.Anime lastAnime = null;

            Console.WriteLine("Scaning for anime every {0} miliseconds...", Config.program.ScanInterval);
            while (!stop)
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

        public static void Log(string text)
        {
            Console.WriteLine(string.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss"), text));
        }
    }
}
