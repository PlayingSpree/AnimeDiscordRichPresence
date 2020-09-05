using System;
using System.Collections.Generic;
using System.Text;

namespace AnimeDiscordRichPresence
{
    static class Config
    {
        public static ConfigFile.AnimeMatchConfig animeMatch;
        public static ConfigFile.ProgramConfig program;

        public static void Load()
        {
            animeMatch = new ConfigFile.AnimeMatchConfig();
            program = new ConfigFile.ProgramConfig();
        }

        public class ConfigFile
        {
            public class AnimeMatchConfig
            {
                public class AnimeWebsite
                {
                    public string website = "Anime Sugoi";
                    public string matchText = "Anime-Sugoi";
                    public string matchAnimeNameStartText = null;
                    public string matchAnimeNameEndText = "ตอนที่";
                    public string matchEpisodeStartText = "ตอนที่";
                    public string matchEpisodeEndText = "ซับ";
                }
                public List<string> processNames = new List<string>() { "chrome", "msedge" };
                public List<AnimeWebsite> animeWebsites = new List<AnimeWebsite>() { new AnimeWebsite() };
            }
            public class ProgramConfig
            {
                public int scanInterval = 3000;
            }
        }
    }
}
