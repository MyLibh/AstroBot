namespace AstroBot.VK
{
    public static class Settings
    {
        public static string Name { get; } = System.IO.File.ReadAllLines(@"../../../config/VK/config.cfg")[1].Substring(5);
        public static string Token { get; } = System.IO.File.ReadAllLines(@"../../../config/VK/config.cfg")[2].Substring(6);
    }
}
