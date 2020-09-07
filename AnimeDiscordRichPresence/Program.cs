namespace AnimeDiscordRichPresence
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!MainLogic.Init())
            {
                return;
            }
            UpdateChecker.Check();
            MainLogic.Run();
        }
    }
}
