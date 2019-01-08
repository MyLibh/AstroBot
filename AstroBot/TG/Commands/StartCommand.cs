using Telegram.Bot;
using Telegram.Bot.Types;

namespace AstroBot.TG.Commands
{
    public class StartCommand : Command
    {
        public override string Name => "start";

        public override void Execute(Message msg, TelegramBotClient client)
        {
            var chatId = msg.Chat.Id;
            var msgId = msg.MessageId;

            client.SendTextMessageAsync(chatId, "Привет!\nЯ Астробот.\nК сожалению, мои возможности ограничены :(\n" + ListCommand.List, replyToMessageId: msgId);
        }
    }
}