using System;

namespace AnimeDiscordRichPresence
{
    static class DiscordActivity
    {
        static Discord.Discord discord;
        static Discord.ActivityManager activityManager;

        static void Init()
        {
            if (discord == null)
            {
                discord = new Discord.Discord(751435145315221615, (UInt64)Discord.CreateFlags.Default);
                activityManager = discord.GetActivityManager();
            }
        }
        public static void Set(GetAnimeName.Anime anime)
        {
            Init();

            var activity = new Discord.Activity
            {
                State = anime.website,
                Details = anime.name,
                Timestamps =
                    {
                        Start = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    },
                Instance = true,
            };

            Console.WriteLine("Try to set discord activity... (Name: {0})", anime.name);
            activityManager.UpdateActivity(activity, statusCallback);
        }
        static void statusCallback(Discord.Result result)
        {
            if (result == Discord.Result.Ok)
            {
                Console.WriteLine("Success!");
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }
        public static void Clear()
        {
            Console.WriteLine("Try to clear discord activity...");
            discord.Dispose();
            discord = null;
        }
        public static void Update()
        {
            if (discord != null)
            {
                discord.RunCallbacks();
            }
        }
    }
}
