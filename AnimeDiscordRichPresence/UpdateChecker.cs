using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace AnimeDiscordRichPresence
{
    class UpdateChecker
    {
        // Release Version ============================
        public const string currentVersion = "v1.1.1";
        // ============================================
        public static void Check()
        {
            Console.WriteLine($"Checking update... (Current Version: {currentVersion})");
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "AnimeDiscordRichPresence");
                    var response = client.GetAsync("https://api.github.com/repos/PlayingSpree/AnimeDiscordRichPresence/releases/latest").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;
                        string responseString = responseContent.ReadAsStringAsync().Result;

                        using (JsonDocument document = JsonDocument.Parse(responseString))
                        {
                            string lastestVersion = document.RootElement.GetProperty("tag_name").GetString();
                            Console.Write($"Latest version: {lastestVersion} ");
                            if (string.Compare(currentVersion, lastestVersion) < 0)
                            {
                                Console.WriteLine();
                                Console.WriteLine("New version available! Get it at https://github.com/PlayingSpree/AnimeDiscordRichPresence/releases/latest");

                                if (Config.program.UpdateIgnoreAll || Config.program.UpdateIgnoreVersion == lastestVersion)
                                {
                                    return;
                                }

                                Console.WriteLine("Press [i] to ignore all update...");
                                Console.WriteLine("Press other key to ignore this update...");

                                if (Console.ReadKey().Key == ConsoleKey.I)
                                {
                                    Config.program.UpdateIgnoreAll = true;
                                }
                                Config.program.UpdateIgnoreVersion = lastestVersion;
                                Config.Save();
                            }
                            else
                            {
                                Console.Write("Up to date!");
                            }
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{(int)response.StatusCode} {response.ReasonPhrase} : {response.Content.ReadAsStringAsync().Result}");
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Update Check Error.");
                Console.WriteLine(e);
            }
        }
    }
}
