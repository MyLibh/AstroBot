using System;
using System.Collections.Generic;

using VkNet;
using VkNet.Model;

namespace AstroBot.VK.Commands
{
    public class ListCommand : Command
    {
        public override string Name => "list";

        public new static string AnswerInfo => 
            "/register - Зарегистрироваться\n" +
            "/list - Вывести список команд\n" +
            "/task - Получить домашку\n" +
            "/answer - Проверить ответ\n" +
            "/solution - Сдать решениe";

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