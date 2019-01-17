using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AstroBot.TG.Commands
{
    public class StartCommand : Command
    {
        public override string Name => "start";

        public new string AnswerInfo => "Привет!\nЯ Астробот.\nК сожалению, мои возможности ограничены :(\n";

        public override void Execute(Message msg, TelegramBotClient client)
        {
            if (msg.Type != MessageType.Text)
                return;

            var chatId = msg.Chat.Id;
            var msgId = msg.MessageId;

            client.SendTextMessageAsync(chatId, AnswerInfo + ListCommand.AnswerInfo, replyToMessageId: msgId);
        }
    }
}