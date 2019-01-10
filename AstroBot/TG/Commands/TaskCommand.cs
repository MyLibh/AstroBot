using System;

using Telegram.Bot;
using Telegram.Bot.Types;

using AstroBot.Util;
using AstroBot.DB;
using AstroBot.DB.Students;

namespace AstroBot.TG.Commands
{
    public class TaskCommand : Command
    {
        public new string AnswerOk => "Держи свое задание:\n";
        public new string AnswerInfo => "Если вы сдали предыдущую задачу, используйте /task для получения новой";
        public override string Name => "task";

        public override void Execute(Message msg, TelegramBotClient client)
        {
            var chatId = msg.Chat.Id;
            var msgId = msg.MessageId;
            try
            {
                if (DataBase.Students.Exist(Students.ExistOption.TGId, msg.From.Id.ToString()))
                {
                    var tuple = DataBase.Tasks.GetTask(DB.Tasks.Tasks.IdType.TGId, msg.From.Id.ToString());
                    client.SendTextMessageAsync(chatId, AnswerOk + tuple.Item1, replyToMessageId: msgId);
                }
                else
                {
                    client.SendTextMessageAsync(chatId, "Данный аккаунт незарегистрирован, используйте /register", replyToMessageId: msgId);

                    return;
                }
            }
            catch (ArgumentException exception)
            {
                client.SendTextMessageAsync(chatId, exception.Message, replyToMessageId: msgId);

                Logger.Log(Logger.Module.TG, Logger.Type.Warning, $"{msg.From.Username}: {msg.Text} ({exception.Message})");
            }
            catch (Exception exception)
            {
                client.SendTextMessageAsync(chatId, AnswerError + "(" + exception.Message + ")\n" + AnswerInfo, replyToMessageId: msgId);

                Logger.Log(Logger.Module.TG, Logger.Type.Warning, $"{msg.From.Username}: {msg.Text} ({exception.Message})");
            }

            Logger.Log(Logger.Module.TG, Logger.Type.Info, $"{msg.From.Username}: {msg.Text}");
        }
    }
}