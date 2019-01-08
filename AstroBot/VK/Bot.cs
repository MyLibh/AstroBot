using System;
using System.Collections.Generic;

using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace AstroBot.VK
{
    public static class Bot
    {
        private static VkApi client = new VkApi();
        // private static List<Command> commandsList;
        // public static IReadOnlyList<Command> Commands { get => commandsList.AsReadOnly(); }

        public static void Start()
        {
            //if (client != null)
             //   return;

            //initCommandsList();

            client.Authorize(new ApiAuthParams { AccessToken = Settings.Token });
            Console.WriteLine(client.Token);
            var res = client.Groups.Get(new GroupsGetParams());

            Console.WriteLine(res.TotalCount);
            //initCallbacks();

            //client.StartReceiving();
        }

        public static VkApi Get()
        {
            return client;
        }

        private static void initCommandsList()
        {
            /*commandsList = new List<Command>();
            commandsList.Add(new RegisterCommand());
            commandsList.Add(new ListCommand());
            commandsList.Add(new AnswerCommand());
            commandsList.Add(new TaskCommand());
            commandsList.Add(new SolutionCommand());
            commandsList.Add(new StartCommand());*/
        }

        private static void initCallbacks()
        {
            //client.OnMessage += MessageController.Update;
        }

        public static void Stop()
        {
            //client.StopReceiving();
        }
    }
}
