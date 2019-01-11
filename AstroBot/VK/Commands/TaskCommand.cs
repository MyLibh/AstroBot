using System;
using System.Collections.Generic;

using VkNet;
using VkNet.Model;

using AstroBot.Util;
using AstroBot.DB;
using AstroBot.DB.Students;

namespace AstroBot.VK.Commands
{
    public class TaskCommand : Command
    {
        public new string AnswerOk => "Держи свое задание:\n";
        public new string AnswerInfo => "Если вы сдали предыдущую задачу, используйте /task для получения новой";
        public override string Name => "task";

        public override void Execute(Message msg, VkApi client)
        {
            try
            {
                if (DataBase.Students.Exist(Students.ExistOption.VKId, msg.UserId.ToString()))
                {
                    var tuple = DataBase.Tasks.GetTask(DB.Tasks.Tasks.IdType.VKId, msg.UserId.ToString());
                    client.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
                    {
                        RandomId = Environment.TickCount,
                        ForwardMessages = new List<long> { msg.Id.Value },
                        UserId = msg.UserId,
                        Message = AnswerOk + tuple.Item1
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

                Logger.Log(Logger.Module.VK, Logger.Type.Warning, $"{exception.Message}: {msg.Body} ({exception.Message})");
            }

            Logger.Log(Logger.Module.VK, Logger.Type.Info, $"{msg.UserId}: {msg.Body}");
        }
    }
}