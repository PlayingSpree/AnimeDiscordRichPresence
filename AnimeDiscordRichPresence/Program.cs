namespace AnimeDiscordRichPresence
{
    class Program
    {
        static void Main(string[] args)
        {
            MainLogic.Init();
            bool stop = false;
            MainLogic.Run(ref stop);
        }
    }
}
