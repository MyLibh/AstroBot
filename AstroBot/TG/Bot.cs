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

            Logger.Log(Logger.Module.TG, Logger.Type.Debug, "Starting bot...");

            initCommandsList();

            client = new TelegramBotClient(Config.Token);
            initCallbacks();

            client.StartReceiving();

            Logger.Log(Logger.Module.TG, Logger.Type.Debug, "Bot started");
        }

        public static TelegramBotClient Get()
        {
            return client;
        }

        private static void initCommandsList()
        {
            Logger.Log(Logger.Module.TG, Logger.Type.Debug, "\tInitializing commands list...");

            commandsList = new List<Command>();
            commandsList.Add(new RegisterCommand());
            commandsList.Add(new ListCommand());
            commandsList.Add(new AnswerCommand());
            commandsList.Add(new TaskCommand());
            commandsList.Add(new SolutionCommand());
            commandsList.Add(new StartCommand());

            Logger.Log(Logger.Module.TG, Logger.Type.Debug, "\tCommands list initialized");
        }

        private static void initCallbacks()
        {
            Logger.Log(Logger.Module.TG, Logger.Type.Debug, "\tInitializing callbacks...");

            client.OnMessage += MessageController.Update;

            Logger.Log(Logger.Module.TG, Logger.Type.Debug, "\tCallbacks initialized");
        }

        public static void Stop()
        {
            if (client == null)
                return;

            Logger.Log(Logger.Module.TG, Logger.Type.Debug, "Stopping bot...");

            client.StopReceiving();

            Logger.Log(Logger.Module.TG, Logger.Type.Debug, "Bot stopped");
        }
    }
}