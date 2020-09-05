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
                if (title.Contains(animeWebsite.matchText))
                {
                    return new Anime("Anime Name", animeWebsite.website, "1");
                    //int pFrom = St.IndexOf("key : ") + "key : ".Length;
                    //int pTo = St.LastIndexOf(" - ");

                    //String result = St.Substring(pFrom, pTo - pFrom);
                }
            }
            return null;
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
