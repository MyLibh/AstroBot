using Telegram.Bot;
using Telegram.Bot.Types;

namespace AstroBot.TG.Commands
{
    public class ListCommand : Command
    {
        public static string List { get; } = 
            "/register - Зарегистрироваться\n"    +
            "/list - Вывести список команд\n" +
            "/task - Получить домашку\n" +
            "/answer - Проверить ответ\n" +
            "/solution - Сдать решениe";
        public override string Name => "list";

        public override void Execute(Message msg, TelegramBotClient client)
        {
            var chatId = msg.Chat.Id;
            var msgId = msg.MessageId;

            client.SendTextMessageAsync(chatId, ListCommand.List, replyToMessageId: msgId);
        }
    }
}