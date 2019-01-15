using System;
using System.Collections.Generic;

using VkNet;
using VkNet.Model;

using AstroBot.Util;
using AstroBot.DB;
using AstroBot.DB.Students;

namespace AstroBot.VK.Commands
{
    public class AnswerCommand : Command
    {
        public new string AnswerOk => "Да, ответ правильный :)\n Теперь можете сдать ваше решение через /solution";
        public new string AnswerError => "Я тебя не понимаю!";
        public new string AnswerInfo => "Чтобы проверить последнее задание, введите:\n /answer <Ваш ответ>.\n Например,\n /answer 100";

        public string AnswerBadAnswer => "Увы, но ответ неправильный :(";

        public override string Name => "answer";

        public override void Execute(Message msg, VkApi client)
        {
            try
            {
                if (DataBase.Students.Exist(Students.ExistOption.VKId, msg.UserId.ToString()))
                {
                    string str = msg.Body;
                    String[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (words.Length != 2)
                        throw new FormatException("Неверный ввод");

                    if (DataBase.Tasks.CheckAnswer(DB.Tasks.Tasks.IdType.VKId, msg.UserId.ToString(), Convert.ToDouble(words[1])))
                        client.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
                        {
                            RandomId = Environment.TickCount,
                            ForwardMessages = new List<long> { msg.Id.Value },
                            UserId = msg.UserId,
                            Message = AnswerOk
                        });
                    else
                        client.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
                        {
                            RandomId = Environment.TickCount,
                            ForwardMessages = new List<long> { msg.Id.Value },
                            UserId = msg.UserId,
                            Message = AnswerBadAnswer
                        });
                }
                else
                {
                    client.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
                    {
                        RandomId = Environment.TickCount,
                        ForwardMessages = new List<long> { msg.Id.Value },
                        UserId = msg.UserId,
                        Message = "Данный аккаунт незарегистрирован, используйте /register"
                    });

                    return;
                }
            }
            catch (ArgumentException exception)
            {
                client.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
                {
                    RandomId = Environment.TickCount,
                    ForwardMessages = new List<long> { msg.Id.Value },
                    UserId = msg.UserId,
                    Message = exception.Message
                });

                Logger.Log(Logger.Module.VK, Logger.Type.Warning, $"{msg.UserId}: {msg.Body} ({exception.Message})");
            }
            catch (Exception exception)
            {
                client.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
                {
                    RandomId = Environment.TickCount,
                    ForwardMessages = new List<long> { msg.Id.Value },
                    UserId = msg.UserId,
                    Message = AnswerError + "(" + exception.Message + ")\n" + AnswerInfo
                });

                Logger.Log(Logger.Module.VK, Logger.Type.Warning, $"{msg.UserId}: {msg.Body} ({exception.Message})");
            }

            Logger.Log(Logger.Module.VK, Logger.Type.Info, $"{msg.UserId}: {msg.Body}");
        }
    }
}