using System;

using Telegram.Bot;
using Telegram.Bot.Types;

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
            var msgId   = msg.MessageId;
            var student = new Student();

            try
            {
                string str = msg.Text;
                String[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (words.Length != 4 || words[3].Length != 3)
                    throw new ArgumentException("Invalid argument");

                student.Name    = words[1].Substring(0, Math.Min(words[1].Length, Student.NameMaxLength));
                student.Surname = words[2].Substring(0, Math.Min(words[2].Length, Student.SurnameMaxLength));
                student.Class   = words[3].Substring(0, Math.Min(words[3].Length, Student.ClassMaxLength));
                student.TGId    = msg.From.Username;

                if (DataBase.Students.Exist(Students.ExistOption.TGId, student.TGId) != 0)
                    throw new ArgumentException("Already registered");

                var id = DataBase.Students.Exist(Students.ExistOption.Surname, student.Surname);
                if (id != 0)
                    DataBase.Students.Update(Students.UpdateOption.TGId, id, student.TGId);
                else
                    DataBase.Students.Add(student);
            }
            catch(Exception e)
            {
                client.SendTextMessageAsync(msg.Chat.Id, AnswerError + "(" + e.Message + ")\n" + AnswerInfo, replyToMessageId: msgId);

                Logger.Log(Logger.Module.TG, Logger.Type.Warning, $"{msg.From.Username}: {msg.Text} ({e.Message})");

                return;
            }

            client.SendTextMessageAsync(msg.Chat.Id, AnswerOk, replyToMessageId: msgId);

            Logger.Log(Logger.Module.TG, Logger.Type.Info, $"{student.TGId}: {msg.Text}");
        }
    }
}