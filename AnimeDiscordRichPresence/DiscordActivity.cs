﻿using System;
using System.Text;

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
        public static void Set(AnimeName.Anime anime)
        {
            Init();
            Update();

            string stateString = string.IsNullOrEmpty(anime.episode) ? "" : string.Format("Episode {0} ", anime.episode);
            stateString += string.IsNullOrEmpty(anime.website) ? "" : string.Format("On {0}", anime.website);

            // Hack from string to byte[128] to fix UTF-8 problem
            var activity = new Discord.Activity
            {
                State = StringToByte(stateString),
                Details = StringToByte(anime.name),
                Timestamps =
                    {
                        Start = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    },
                Instance = true,
            };

            MainLogic.Log("Try to set discord activity...");
            try
            {
                activityManager.UpdateActivity(activity, StatusCallback);
            }
            catch
            {
                MainLogic.Log("Could not set activity. Discord error. (Maybe try opening discord...)");
            }
        }
        static byte[] StringToByte(string text)
        {
            byte[] array = Encoding.UTF8.GetBytes(text);
            Array.Resize(ref array, 128);
            return array;
        }
        static void StatusCallback(Discord.Result result)
        {
            if (result == Discord.Result.Ok)
            {
                MainLogic.Log("Set activity success!");
            }
            else
            {
                MainLogic.Log(string.Format("Set activity failed. (Code {0}: {1})", (int)result, result));
            }
        }
        public static void Clear()
        {
            MainLogic.Log("Disconnecting from discord...");
            if (discord != null)
            {
                try
                {
                    discord.Dispose();
                }
                catch
                {

                }
            }
            discord = null;
        }
        public static void Update()
        {
            if (discord != null)
            {
                try
                {
                    discord.RunCallbacks();
                }
                catch
                {
                    MainLogic.Log("Discord update error. (Maybe try opening discord...)");
                    Clear();
                }
            }
        }
    }
}
