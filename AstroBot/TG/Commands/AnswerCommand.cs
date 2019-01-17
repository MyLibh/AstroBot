using System;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using AstroBot.DB;
using AstroBot.DB.Students;
using AstroBot.Util;

namespace AstroBot.TG.Commands
{
    public class AnswerCommand : Command
    {
        public new string AnswerOk => "Да, ответ правильный :)\n Теперь можете сдать ваше решение через /solution";
        public new string AnswerError => "Я тебя не понимаю!";
        public new string AnswerInfo => "Чтобы проверить последнее задание, введите:\n /answer <Ваш ответ>.\n Например,\n /answer 100";
        public string AnswerBadAnswer => "Увы, но ответ неправильный :(";

        public override string Name => "answer";
        public override void Execute(Message msg, TelegramBotClient client)
        {
            if (msg.Type != MessageType.Text)
                return;

            var chatId = msg.Chat.Id;
            var msgId = msg.MessageId;
            try
            {
                if (DataBase.Students.Exist(Students.ExistOption.TGId, msg.From.Id.ToString()))
                {
                    string str = msg.Text;
                    String[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (words.Length != 2)
                        throw new FormatException("Неверный ввод");

                    if (DataBase.Tasks.CheckAnswer(DB.Tasks.Tasks.IdType.TGId, msg.From.Id.ToString(), Convert.ToDouble(words[1])))
                        client.SendTextMessageAsync(chatId, AnswerOk, replyToMessageId: msgId);
                    else
                        client.SendTextMessageAsync(chatId, AnswerBadAnswer, replyToMessageId: msgId);
                }
                else
                {
                    client.SendTextMessageAsync(chatId, "Данный аккаунт незарегистрирован, используйте /register", replyToMessageId: msgId);

                    return;
                }
            }
            catch(ArgumentException exception)
            {
                client.SendTextMessageAsync(chatId, exception.Message, replyToMessageId: msgId);

                Logger.Log(Logger.Module.TG, Logger.Type.Warning, $"{msg.From.Username}: {msg.Text} ({exception.Message})");
            }
            catch(Exception exception)
            {
                client.SendTextMessageAsync(chatId, AnswerError + "(" + exception.Message + ")\n" + AnswerInfo, replyToMessageId: msgId);

                Logger.Log(Logger.Module.TG, Logger.Type.Warning, $"{msg.From.Username}: {msg.Text} ({exception.Message})");      
            }

            Logger.Log(Logger.Module.TG, Logger.Type.Info, $"{msg.From.Username}: {msg.Text}");
        }
    }
}