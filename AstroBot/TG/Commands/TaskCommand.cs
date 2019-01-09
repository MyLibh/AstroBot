﻿using Telegram.Bot;
using Telegram.Bot.Types;

using AstroBot.Util;

namespace AstroBot.TG.Commands
{
    public class TaskCommand : Command
    {
        public override string Name => "task";

        public new string AnswerInfo => "Not implemented yet";

        public override void Execute(Message msg, TelegramBotClient client)
        {
            var chatId = msg.Chat.Id;
            var msgId = msg.MessageId;

            Logger.Log(Logger.Module.TG, Logger.Type.Info, "'" + msg.From.Username + "'" + " > " + msg.Text);

            client.SendTextMessageAsync(chatId, AnswerInfo, replyToMessageId: msgId);
        }
    }
}