using System;

namespace AstroBot.TG
{
    public static class Settings
    {
        // TODO: parse from config
        public static string Name { get; } = System.IO.File.ReadAllLines(@"../../../config/TG/config.cfg")[1].Substring(5);
        public static string Token { get; } = System.IO.File.ReadAllLines(@"../../../config/TG/config.cfg")[2].Substring(6);
    }
}