using Telegram.Bot;
using Telegram.Bot.Types;

namespace AstroBot.TG.Commands
{
    public class RegisterCommand : Command
    {
        public static string Answer { get; } = "Введите:\n <Фамилия> <Имя> <Класс>.\n Например,\n Иванов Иван 11X";
        public override string Name => "register";

        public override void Execute(Message msg, TelegramBotClient client)
        {
            var chatId = msg.Chat.Id;
            var msgId = msg.MessageId;

            client.SendTextMessageAsync(chatId, RegisterCommand.Answer, replyToMessageId: msgId);

            //Student student = new Student{ Id = 2, Surname = "Иванов", Name = "Иван", Class = "11X", TelegramId = msg.From.Id };
    
            //DataBase.AddStudent(student);
        }
    }
}