namespace AstroBot.VK
{
    public static class Settings
    {
        public static string Token { get; } = System.IO.File.ReadAllLines(@"../../../config/VK/config.cfg")[1].Substring(6);
    }
}
