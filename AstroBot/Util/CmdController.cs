using System;

namespace AstroBot.Util
{
    class CmdController
    {
        public static void Execute()
        {
            while(true)
            {
                var str = Console.ReadLine();
                if(str == "help")
                {
                    Console.WriteLine("start - Запустить программу");
                    Console.WriteLine("exit  - Остановить программу");
                    Console.WriteLine("help  - Вывести хелп");
                }
                else if(str == "start")
                {
                    Logger.Log(Logger.Module.Core, Logger.Type.Info, "Starting...");

                    DB.DataBase.OpenConnection();

                    TG.Bot.Start();

                    Logger.Log(Logger.Module.Core, Logger.Type.Info, "Started");
                }
                else if(str == "exit" || str == "stop")
                {
                    Logger.Log(Logger.Module.Core, Logger.Type.Info, "Stopping...");

                    TG.Bot.Stop();

                    DB.DataBase.CloseConnection();

                    Logger.Log(Logger.Module.Core, Logger.Type.Info, "Stopped");
                    Console.ReadLine();
                    break;
                }
                else if(str == "showdb")
                {
                    DB.DataBase.Students.Print();
                }
                else
                {
                    Logger.Log(Logger.Module.Core, Logger.Type.Info, $"Undefined command: '{str}'");
                    Console.WriteLine();
                }
            }
        }
    }
}
