using System;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using AstroBot.DB;
using AstroBot.DB.Students;
using AstroBot.Util;

namespace AstroBot.TG.Commands
{
    public class RegisterCommand : Command
    {
        public new string AnswerOk => "Вы успешно зарегистрировались!";
        public new string AnswerError => "Упс, что-то пошло не так!";
        public new string AnswerInfo => "Введите:\n /register <Имя> <Фамилия> <Класс>.\n Например,\n /register Иванов Иван 11X";
        public override string Name => "register";

        public override void Execute(Message msg, TelegramBotClient client)
        {
            if (msg.Type != MessageType.Text)
                return;

            var msgId   = msg.MessageId;
            var student = new Student();

            try
            {
                string str = msg.Text;
                String[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (words.Length != 4 || words[3].Length != 3)
                    throw new FormatException("Неправильный ввод");

                student.Name    = words[1].Substring(0, Math.Min(words[1].Length, Student.NameMaxLength));
                student.Surname = words[2].Substring(0, Math.Min(words[2].Length, Student.SurnameMaxLength));
                student.Class   = words[3].Substring(0, Math.Min(words[3].Length, Student.ClassMaxLength));
                student.TGId    = msg.From.Id.ToString();

                if (DataBase.Students.Exist(Students.ExistOption.TGId, student.TGId))
                    throw new ArgumentException("Вы уже зарегистрированны");

                if (DataBase.Students.Exist(Students.ExistOption.Surname, student.Surname))
                {
                    DataBase.Students.Update(Students.IdType.TGId, student.Surname, student.TGId);

                    client.SendTextMessageAsync(msg.Chat.Id, "Ваш профиль был обновлен", replyToMessageId: msgId);

                    Logger.Log(Logger.Module.TG, Logger.Type.Info, $"[UPDATE] {student.TGId}: {msg.Text}");
                }
                else
                {
                    DataBase.Students.Add(student);

                    client.SendTextMessageAsync(msg.Chat.Id, AnswerOk, replyToMessageId: msgId);

                    Logger.Log(Logger.Module.TG, Logger.Type.Info, $"{student.TGId}: {msg.Text}");
                }
            }
            catch(Exception exception)
            {
                client.SendTextMessageAsync(msg.Chat.Id, AnswerError + "(" + exception.Message + ")\n" + AnswerInfo, replyToMessageId: msgId);

                Logger.Log(Logger.Module.TG, Logger.Type.Warning, $"{msg.From.Username}: {msg.Text} ({exception.Message})");
            }
        }
    }
}