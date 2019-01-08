using System;
using System.Collections.Generic;

using Telegram.Bot;

using AstroBot.Util;
using AstroBot.TG.Commands;

namespace AstroBot.TG
{
    public static class Bot
    {
        private static TelegramBotClient client;
        private static List<Command> commandsList;
     
        public static IReadOnlyList<Command> Commands { get => commandsList.AsReadOnly(); }

        public static void Start()
        {
            if (client != null)
                return;

            Logger.Log(Logger.Module.TG, Logger.Type.Info, "Starting bot...");

            initCommandsList();

            client = new TelegramBotClient(Settings.Token);
            initCallbacks();

            client.StartReceiving();

            Logger.Log(Logger.Module.TG, Logger.Type.Info, "Bot started");
        }

        public static TelegramBotClient Get()
        {
            return client;
        }

        //public static Logger GetLogger()
        //{
        //    return logger;
        //}

        private static void initCommandsList()
        {
            commandsList = new List<Command>();
            commandsList.Add(new RegisterCommand());
            commandsList.Add(new ListCommand());
            commandsList.Add(new AnswerCommand());
            commandsList.Add(new TaskCommand());
            commandsList.Add(new SolutionCommand());
            commandsList.Add(new StartCommand());
        }

        private static void initCallbacks()
        {
            client.OnMessage += MessageController.Update;
        }

        public static void Stop()
        {
            Logger.Log(Logger.Module.TG, Logger.Type.Info, "Stopping bot...");
            client.StopReceiving();
            Logger.Log(Logger.Module.TG, Logger.Type.Info, "Bot stopped");
        }
    }
}