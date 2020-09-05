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
                    public List<string> matchText = new List<string>() { "Anime-Sugoi" };
                    public List<string> matchAnimeNameStartText = new List<string>() { };
                    public List<string> matchAnimeNameEndText = new List<string>() { "ตอนที่" };
                    public List<string> matchEpisodeStartText = new List<string>() { "ตอนที่" };
                    public List<string> matchEpisodeEndText = new List<string>() { "ซับ", "แปล" };
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
