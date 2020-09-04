using System;
using System.Diagnostics;
using System.Threading;

namespace AnimeDiscordRichPresence
{
    class Program
    {
        static void Main(string[] args)
        {
            GetAnimeName.Init();
            GetAnimeName.Anime lastAnime = null;

            Console.WriteLine("Anime Discord Rich Presence by PlayingSpree.\nGLHF :)");
            while (true)
            {
                GetAnimeName.Anime anime = GetAnimeName.GetAnime();
                if (anime == null)
                {
                    if (lastAnime != null)
                    {
                        DiscordActivity.Clear();
                    }
                }
                else
                {
                    if (lastAnime == null)
                    {
                        DiscordActivity.Set(anime);
                    }
                    else if (lastAnime.name != anime.name)
                    {
                        DiscordActivity.Set(anime);
                    }
                }
                lastAnime = anime;
                DiscordActivity.Update();
                Thread.Sleep(1000);
            }
        }
    }
}
