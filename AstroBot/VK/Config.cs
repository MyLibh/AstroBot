﻿namespace AstroBot.VK
{
    public static class Config
    {
        private static readonly string CONFIG_PATH = @"../../../config/vk.cfg";

        public static string Name { get; } = System.IO.File.ReadAllLines(CONFIG_PATH)[1].Substring(5);
        public static string Token { get; } = System.IO.File.ReadAllLines(CONFIG_PATH)[2].Substring(6);
    }
}
