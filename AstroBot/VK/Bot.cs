using System;
using System.Collections.Generic;

using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.GroupUpdate;
using VkNet.Model.RequestParams;

using AstroBot.VK.Commands;
using AstroBot.Util;
using VkNet.Exception;
using System.Threading.Tasks;

namespace AstroBot.VK
{
    public static class Bot
    {
        private static VkApi client;
        private static VkObject group;
        private static LongPollServerResponse lpSettings { get; set; }
        private static List<Command> commandsList;
        public static IReadOnlyList<Command> Commands { get => commandsList.AsReadOnly(); }

        private static event EventHandler<GroupUpdate> onMessage;

        public static void Start()
        {
            if (client != null)
                return;

            Logger.Log(Logger.Module.VK, Logger.Type.Info, "Starting bot...");

            initCommandsList();
            
            client = new VkApi();
            client.Authorize(new ApiAuthParams { AccessToken = Settings.Token });

            group = client.Utils.ResolveScreenName(Settings.Name);
            lpSettings = client.Groups.GetLongPollServer((ulong)group.Id.Value);

            // ServicePointManager.DefaultConnectionLimit = 20;

            initCallbacks();
            startLongPoolEventHandler();

            Logger.Log(Logger.Module.TG, Logger.Type.Info, "Bot started");
        }

        public static VkApi Get()
        {
            return client;
        }

        private static void initCommandsList()
        {
            Logger.Log(Logger.Module.VK, Logger.Type.Info, "Initializing commands list...");

            commandsList = new List<Command>();
            commandsList.Add(new RegisterCommand());
            commandsList.Add(new ListCommand());
            commandsList.Add(new AnswerCommand());
            commandsList.Add(new TaskCommand());
            commandsList.Add(new SolutionCommand());
            commandsList.Add(new StartCommand());

            Logger.Log(Logger.Module.VK, Logger.Type.Info, "Commands list initialized");
        }

        private static void processLongPollEvents(BotsLongPollHistoryResponse pollResponse)
        {
            foreach (GroupUpdate update in pollResponse.Updates)
                if (update.Type == GroupUpdateType.MessageNew)
                    onMessage?.Invoke(null, update);
        }

        private static void initCallbacks()
        {
            Logger.Log(Logger.Module.VK, Logger.Type.Info, "Initializing callbacks...");

            onMessage += MessageController.Update;

            Logger.Log(Logger.Module.VK, Logger.Type.Info, "Callbacks initialized");
        }

        private static async void startLongPoolEventHandler()
        {
            Logger.Log(Logger.Module.VK, Logger.Type.Info, "Starting LongPool Event Handler");

            while (true)
            {
                try
                {
                    BotsLongPollHistoryResponse longPollResponse = await client.Groups.GetBotsLongPollHistoryAsync(
                        new BotsLongPollHistoryParams
                        {
                            Key = lpSettings.Key,
                            Server = lpSettings.Server,
                            Ts = lpSettings.Ts,
                            Wait = 20
                        }).ContinueWith(CheckLongPollResponseForErrorsAndHandle).ConfigureAwait(false); ;

                    if (longPollResponse == default(BotsLongPollHistoryResponse))
                        continue;

                    processLongPollEvents(longPollResponse);

                    lpSettings.Ts = longPollResponse.Ts;
                }
                catch (Exception exception)
                {
                    Logger.Log(Logger.Module.VK, Logger.Type.Error, exception.Message);
                }
            }
        }

        private static T CheckLongPollResponseForErrorsAndHandle<T>(Task<T> task)
        {
            if (task.IsFaulted)
            {
                if (task.Exception is AggregateException ae)
                {
                    foreach (Exception exception in ae.InnerExceptions)
                    {
                        if (exception is LongPollOutdateException lpoex)
                        {
                            lpSettings.Ts = lpoex.Ts;

                            return default(T);
                        }
                        else if (exception is LongPollKeyExpiredException)
                        {
                            lpSettings = client.Groups.GetLongPollServer((ulong)group.Id.Value);

                            return default(T);
                        }
                        else if (exception is LongPollInfoLostException)
                        {
                            lpSettings = client.Groups.GetLongPollServer((ulong)group.Id.Value);

                            return default(T);
                        }
                        else
                        {
                            Logger.Log(Logger.Module.VK, Logger.Type.Error, exception.Message);

                            throw exception;
                        }
                    }
                }

                Logger.Log(Logger.Module.VK, Logger.Type.Error, task.Exception.Message);
                throw task.Exception;
            }
            else if (task.IsCanceled)
            {
                Logger.Log(Logger.Module.VK, Logger.Type.Info, "task.IsCanceled, possibly timeout reached");

                return default(T);
            }
            else
            {
                try
                {
                    return task.Result;
                }
                catch (Exception exсeption)
                {
                    Logger.Log(Logger.Module.VK, Logger.Type.Error, exсeption.Message);
                    throw;
                }
            }
        }

        public static void Stop()
        {
            if (client == null)
                return;

            Logger.Log(Logger.Module.VK, Logger.Type.Info, "Stopping bot...");

            client.Dispose();

            Logger.Log(Logger.Module.VK, Logger.Type.Info, "Stopped");
        }
    }
}
