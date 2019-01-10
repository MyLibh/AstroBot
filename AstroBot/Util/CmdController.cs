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
                if (str == "help")
                {
                    Console.WriteLine("start   - Запустить программу");
                    Console.WriteLine("exit    - Остановить программу");
                    Console.WriteLine("showdb  - Показать бд учеников");
                    Console.WriteLine("starttg - Запустить телеграм бота");
                    Console.WriteLine("startdb - Запустить базу данных");
                    Console.WriteLine("startvk - Запустить вк бота");
                    Console.WriteLine("help    - Вывести хелп");
                }
                else if (str == "start")
                {
                    Logger.Log(Logger.Module.Core, Logger.Type.Info, "Starting...");

                    DB.DataBase.OpenConnection();

                    TG.Bot.Start();
                    VK.Bot.Start();

                    Logger.Log(Logger.Module.Core, Logger.Type.Info, "Started");
                }
                else if (str == "starttg")
                {
                    Logger.Log(Logger.Module.Core, Logger.Type.Info, "Starting...");

                    DB.DataBase.OpenConnection();

                    TG.Bot.Start();

                    Logger.Log(Logger.Module.Core, Logger.Type.Info, "Started");
                }
                else if (str == "startvk")
                {
                    Logger.Log(Logger.Module.Core, Logger.Type.Info, "Starting...");

                    DB.DataBase.OpenConnection();

                    VK.Bot.Start();

                    Logger.Log(Logger.Module.Core, Logger.Type.Info, "Started");
                }
                else if (str == "startdb")
                {
                    Logger.Log(Logger.Module.Core, Logger.Type.Info, "Starting...");

                    DB.DataBase.OpenConnection();

                    Logger.Log(Logger.Module.Core, Logger.Type.Info, "Started");
                }
                else if (str == "exit" || str == "stop" || str == "close")
                {
                    Logger.Log(Logger.Module.Core, Logger.Type.Info, "Stopping...");

                    VK.Bot.Stop();
                    TG.Bot.Stop();

                    DB.DataBase.CloseConnection();

                    Logger.Log(Logger.Module.Core, Logger.Type.Info, "Stopped");

                    Console.ReadLine();
                    break;
                }
                else if (str == "showdb")
                {
                    if (DB.DataBase.IsOpen)
                        DB.DataBase.Students.Print();
                    else
                        Logger.Log(Logger.Module.Core, Logger.Type.Error, $"DB is not open");
                }
                else
                {
                    Logger.Log(Logger.Module.Core, Logger.Type.Warning, $"Undefined command: '{str}'");
                    Console.WriteLine();
                }
            }
        }
    }
}
