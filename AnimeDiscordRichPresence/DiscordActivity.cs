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
                Program.Log("Try to connect to discord...");
                discord = new Discord.Discord(751435145315221615, (UInt64)Discord.CreateFlags.Default);
                activityManager = discord.GetActivityManager();
            }
        }
        public static void Set(AnimeName.Anime anime)
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

            Program.Log("Try to set discord activity...");
            activityManager.UpdateActivity(activity, StatusCallback);
        }
        static void StatusCallback(Discord.Result result)
        {
            if (result == Discord.Result.Ok)
            {
                Program.Log("Success!");
            }
            else
            {
                Program.Log("Failed");
            }
        }
        public static void Clear()
        {
            Program.Log("Try to disconnect discord...");
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
