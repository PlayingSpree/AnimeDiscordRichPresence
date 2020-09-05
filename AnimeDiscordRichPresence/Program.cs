﻿using System;
using System.Diagnostics;
using System.Threading;

namespace AnimeDiscordRichPresence
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!Config.Load())
                return;

            Console.WriteLine("Anime Discord Rich Presence by PlayingSpree.");
            Console.WriteLine("Edit config.json to add new website.");
            Console.WriteLine("Scaning for anime every {0} miliseconds...", Config.program.ScanInterval);

            AnimeName.Anime lastAnime = null;

            while (true)
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
                DiscordActivity.Update();
                Thread.Sleep(Config.program.ScanInterval);
            }
        }
        public static void Log(string text)
        {
            Console.WriteLine(string.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss"), text));
        }
    }
}
