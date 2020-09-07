using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace AnimeDiscordRichPresence
{
    static class Config
    {
        public static ConfigFile.AnimeMatchConfig animeMatch;
        public static ConfigFile.ProgramConfig program;

        public static bool Load()
        {
            ConfigFile file;
            try
            {
                string jsonString = File.ReadAllText("config.json");
                file = JsonSerializer.Deserialize<ConfigFile>(jsonString);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("config.json file not found.");

                try
                {
                    var options = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                        WriteIndented = true,
                    };
                    File.WriteAllText("config.json", JsonSerializer.Serialize(new ConfigFile(), options), Encoding.UTF8);
                    Console.WriteLine("Default config.json created.");

                    animeMatch = new ConfigFile.AnimeMatchConfig();
                    program = new ConfigFile.ProgramConfig();

                    Console.WriteLine("Press enter to continue with default config or other key to exit program...");

                    return Console.ReadKey().Key == ConsoleKey.Enter;
                }
                catch (Exception)
                {
                    Console.WriteLine("Error while creating config.json.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while reading config.json.");
                Console.WriteLine();
                Console.WriteLine(ex);
                Console.WriteLine();

                return false;
            }
            animeMatch = file.AnimeMatch;
            program = file.Program;
            return true;
        }

        public static bool Save()
        {
            try
            {
                ConfigFile file = new ConfigFile()
                {
                    AnimeMatch = animeMatch,
                    Program = program
                };
                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true,
                };
                File.WriteAllText("config.json", JsonSerializer.Serialize(file, options), Encoding.UTF8);
            }
            catch (Exception)
            {
                Console.WriteLine("Error while writing config.json.");
                return false;
            }
            return true;
        }

        public class ConfigFile
        {
            public AnimeMatchConfig AnimeMatch { get; set; } = new AnimeMatchConfig();
            public ProgramConfig Program { get; set; } = new ProgramConfig();
            public class AnimeMatchConfig
            {
                public class AnimeWebsite
                {
                    public string Website { get; set; } = "anime";
                    public List<string> MatchText { get; set; } = new List<string>() { "anime" };
                    public List<string> MatchAnimeNameStartText { get; set; } = new List<string>() { "Watch" };
                    public List<string> MatchAnimeNameEndText { get; set; } = new List<string>() { "Episode" };
                    public List<string> MatchEpisodeStartText { get; set; } = new List<string>() { "Episode" };
                    public List<string> MatchEpisodeEndText { get; set; } = new List<string>() { "English" };
                }
                public List<string> ProcessNames { get; set; } = new List<string>() { "chrome", "msedge" };
                public List<AnimeWebsite> AnimeWebsites { get; set; } = new List<AnimeWebsite>() { new AnimeWebsite() };
            }
            public class ProgramConfig
            {
                public int ScanInterval { get; set; } = 3000;
                public bool UpdateIgnoreAll { get; set; } = false;
                public string UpdateIgnoreVersion { get; set; } = "v0.0.0";
            }
        }
    }
}
