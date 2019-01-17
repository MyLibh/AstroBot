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
                    send(client, msg, AnswerOk + tuple.Item1);
                }
                else
                { 
                    send(client, msg, "Данный аккаунт незарегистрирован, используйте /register");
                }

                Logger.Log(Logger.Module.VK, Logger.Type.Info, $"{msg.UserId}: {msg.Body}");
            }
            catch (ArgumentException exception)
            {
                send(client, msg, exception.Message);

                Logger.Log(Logger.Module.VK, Logger.Type.Warning, $"{msg.UserId}: {msg.Body} ({exception.Message})");
            }
            catch (Exception exception)
            {
                send(client, msg, AnswerError + "(" + exception.Message + ")\n" + AnswerInfo);

                Logger.Log(Logger.Module.VK, Logger.Type.Warning, $"{exception.Message}: {msg.Body} ({exception.Message})");
            }    
        }
    }
}