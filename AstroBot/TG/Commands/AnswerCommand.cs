using Telegram.Bot;
using Telegram.Bot.Types;

namespace AstroBot.TG.Commands
{
    public class AnswerCommand : Command
    {
        public override string Name => "answer";

        public new string AnswerInfo => throw new System.NotImplementedException();

        public override void Execute(Message msg, TelegramBotClient client)
        {
            var chatId = msg.Chat.Id;
            var msgId = msg.MessageId;

            client.SendTextMessageAsync(chatId, "Not implemented yet", replyToMessageId: msgId);
        }
    }
}