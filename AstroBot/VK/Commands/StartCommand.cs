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
            send(client, msg, AnswerInfo + ListCommand.AnswerInfo);
        }
    }
}