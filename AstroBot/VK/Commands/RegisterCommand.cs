﻿using System;
using System.Collections.Generic;

using VkNet;
using VkNet.Model;

using AstroBot.DB;
using AstroBot.DB.Students;
using AstroBot.Util;

namespace AstroBot.VK.Commands
{
    public class RegisterCommand : Command
    {
        public new string AnswerOk => "Вы успешно зарегистрировались!";
        public new string AnswerError => "Упс, что-то пошло не так!";
        public new string AnswerInfo => "Введите:\n /register <Имя> <Фамилия> <Класс>.\n Например,\n /register Иванов Иван 11X";
        public override string Name => "register";

        public override void Execute(Message msg, VkApi client)
        {
            var student = new Student();

            try
            {
                string str = msg.Body;
                String[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (words.Length != 4 || words[3].Length != 3)
                    throw new ArgumentException("Invalid argument");

                student.Name    = words[1].Substring(0, Math.Min(words[1].Length, Student.NameMaxLength));
                student.Surname = words[2].Substring(0, Math.Min(words[2].Length, Student.SurnameMaxLength));
                student.Class   = words[3].Substring(0, Math.Min(words[3].Length, Student.ClassMaxLength));
                student.VKId    = msg.FromId.ToString();

                if (DataBase.Students.Exist(Students.ExistOption.VKId, student.VKId) != 0)
                    throw new ArgumentException("Already registered");

                var id = DataBase.Students.Exist(Students.ExistOption.Surname, student.Surname);
                if (id != 0)
                    DataBase.Students.Update(Students.UpdateOption.VKId, student.Surname, student.TGId);
                else
                    DataBase.Students.Add(student);
            }
            catch(Exception e)
            {
                client.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
                {
                    RandomId = Environment.TickCount,
                    ForwardMessages = new List<long> { msg.Id.Value },
                    UserId = msg.UserId,
                    Message = AnswerError + "(" + e.Message + ")\n" + AnswerInfo
                });

                Logger.Log(Logger.Module.TG, Logger.Type.Warning, $"{msg.FromId}: {msg.Text} ({e.Message})");

                return;
            }

            client.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
            {
                RandomId = Environment.TickCount,
                ForwardMessages = new List<long> { msg.Id.Value },
                UserId = msg.UserId,
                Message = AnswerOk
            });

            Logger.Log(Logger.Module.TG, Logger.Type.Info, $"{student.TGId}: {msg.Text}");
        }
    }
}