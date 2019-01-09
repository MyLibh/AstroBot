using System;
using System.Collections.Generic;

using VkNet;
using VkNet.Model;

namespace AstroBot.VK.Commands
{
    public class SolutionCommand : Command
    {
        public override string Name => "solution";

        public override void Execute(Message msg, VkApi client)
        {
            client.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
            {
                RandomId = Environment.TickCount,
                ForwardMessages = new List<long> { msg.Id.Value },
                UserId = msg.UserId,
                Message = AnswerInfo
            });
        }
    }
}