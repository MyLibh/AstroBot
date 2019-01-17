﻿using System;

using AstroBot.Util;

namespace AstroBot
{
    class App
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;

            CmdController.Execute();
        }
    }
}
