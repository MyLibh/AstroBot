using System;
using System.Collections.Generic;

using VkNet;
using VkNet.Model;

namespace AstroBot.VK.Commands
{
    public abstract class Command
    {
        public static string AnswerOk { get; } = "Success";
        public static string AnswerError { get; } = "Failure"; 
        public static string AnswerInfo { get; } = "Not implemented yet";

        public abstract string Name { get; }

        public abstract void Execute(Message msg, VkApi client);

        public bool Contains(string command)
        {
            return /* command.Contains(Settings.Name) && */ command.Contains(this.Name);
        }

        protected void send(VkApi client, Message msg, string answer)
        {
            client.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
            {
                RandomId = Environment.TickCount,
                ForwardMessages = new List<long> { msg.Id.Value },
                UserId = msg.UserId,
                Message = answer
            });
        }
    }
}