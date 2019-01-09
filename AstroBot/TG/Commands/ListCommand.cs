using Telegram.Bot;
using Telegram.Bot.Types;

namespace AstroBot.TG.Commands
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

        public override void Execute(Message msg, TelegramBotClient client)
        {
            var chatId = msg.Chat.Id;
            var msgId = msg.MessageId;

            client.SendTextMessageAsync(chatId, AnswerInfo, replyToMessageId: msgId);
        }
    }
}