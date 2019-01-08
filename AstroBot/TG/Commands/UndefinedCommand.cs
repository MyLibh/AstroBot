using Telegram.Bot;
using Telegram.Bot.Types;

namespace AstroBot.TG.Commands
{
    public class UndefinedCommand : Command
    {
        public override string Name => "undefined";

        public override void Execute(Message msg, TelegramBotClient client)
        {
            var chatId = msg.Chat.Id;
            var msgId = msg.MessageId;

            client.SendTextMessageAsync(chatId, "Увы, но я не умею отвечать на такой запрос:\n'" + msg.Text + "'", replyToMessageId: msgId);
        }
    }
}