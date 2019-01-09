using System;
using System.Collections.Generic;

using VkNet;
using VkNet.Model;

namespace AstroBot.VK.Commands
{
    public class StartCommand : Command
    {
        public override string Name => "start";

        public new string AnswerInfo => "Привет!\nЯ Астробот.\nК сожалению, мои возможности ограничены :(\n";

        public override void Execute(Message msg, VkApi client)
        {
            client.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
            {
                RandomId = Environment.TickCount,
                ForwardMessages = new List<long> { msg.Id.Value },
                UserId = msg.UserId,
                Message = AnswerInfo + ListCommand.AnswerInfo
            });
        }
    }
}