using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AnimeDiscordRichPresence
{
    static class AnimeName
    {
        public static Anime GetAnime()
        {
            string title = null;

            Process[] processlist = Process.GetProcesses();

            foreach (Process process in processlist)
            {
                if (Config.animeMatch.processNames.Any(process.ProcessName.Contains))
                {
                    if (!string.IsNullOrEmpty(process.MainWindowTitle))
                    {
                        title = process.MainWindowTitle;
                        break;
                    }
                }
            }

            return processWindowTitle(title);
        }

        static Anime processWindowTitle(string title)
        {
            foreach (var animeWebsite in Config.animeMatch.animeWebsites)
            {
                if (animeWebsite.matchText.Any(title.Contains))
                {
                    string animeTitle = stringCutter(title, animeWebsite.matchAnimeNameStartText, animeWebsite.matchAnimeNameEndText);
                    string animeEpisode = stringCutter(title, animeWebsite.matchEpisodeStartText, animeWebsite.matchEpisodeEndText);

                    return new Anime(animeTitle, animeWebsite.website, animeEpisode);
                }
            }
            return null;
        }

        static string stringCutter(string text, List<string> start, List<string> end)
        {
            int startIndex = 0, endIndex = text.Length;
            if (!start.Any() && !end.Any())
            {
                return "";
            }
            if (start.Any())
            {
                foreach (var match in start)
                {
                    startIndex = text.IndexOf(match);
                    if (startIndex == -1)
                    {
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex += match.Length;
                        break;
                    }
                }
            }
            if (end.Any())
            {
                foreach (var match in end)
                {
                    endIndex = text.IndexOf(match);
                    if (endIndex == -1)
                    {
                        endIndex = text.Length;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return text[startIndex..endIndex].Trim();
        }

        public class Anime
        {
            public string name;
            public string website;
            public string episode;

            public Anime(string name, string website, string episode)
            {
                this.name = name;
                this.website = website;
                this.episode = episode;
            }
        }
    }
}
