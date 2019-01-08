using System;
 
namespace AstroBot
{
    class App
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            TG.Bot.Start();

            //Console.WriteLine("Starting VK bot");
            //VK.Bot.Start();
            //Console.WriteLine(VK.Settings.Token);
            //Console.WriteLine("VK bot started");

            Console.ReadLine();

            TG.Bot.Stop();
        }
    }
}
