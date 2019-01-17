using System;
using System.Collections.Generic;
using System.Net;

using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;

using AstroBot.DB;
using AstroBot.DB.Students;
using AstroBot.Util;
using AstroBot.GD;

namespace AstroBot.VK.Commands
{
    public class SolutionCommand : Command
    {
        public new string AnswerOk => "Решение загружено. Можно получить новое задание /task";
        public new string AnswerError => "Увы, но я не смог открыть твои файлы";
        public new string AnswerInfo => "Чтобы сдать решение, введите:\n /solution <Фото/Документы>.";
        public string AnswerBadAnswer => "Вы не проверили ответ, используйте /answer";

        public override string Name => "solution";

        private string TMP_FILE_PATH = @"C:\swap\tmp.jpg";

        public override void Execute(Message msg, VkApi client)
        {
            try
            {
                if (DataBase.Students.Exist(Students.ExistOption.VKId, msg.UserId.ToString()))
                {
                    string str = msg.Body;
                    String[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (words.Length != 1)
                        throw new FormatException("Неверный ввод");

                    if (DataBase.Tasks.CanSaveSolution(DB.Tasks.Tasks.IdType.VKId, msg.UserId.ToString()))
                    {
                        int cnt = 0;
                        foreach (var attachment in msg.Attachments)
                            if (attachment.Type == typeof(Photo))
                            {
                                Photo photo = attachment.Instance as Photo;

                                // max size download
                                if(photo.Photo807 != null && downloadFile(photo.Photo807))
                                {
                                    GoogleDrive.Upload(TMP_FILE_PATH, "test");

                                    System.IO.File.Delete(TMP_FILE_PATH);

                                    cnt++;
                                }
                            }

                        if (msg.Attachments.Count == 0)
                            throw new ArgumentException("Вы ничего не прикрепили");
                        else if(cnt == msg.Attachments.Count)
                            send(client, msg, AnswerOk);   
                        else
                            send(client, msg, $"Я смог загрузить {cnt}/{msg.Attachments.Count} вложений");
                    }
                    else
                        send(client, msg, AnswerBadAnswer);
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

                Logger.Log(Logger.Module.VK, Logger.Type.Warning, $"{msg.UserId}: {msg.Body} ({exception.Message})");
            }  
        }

        private bool downloadFile(Uri uri)
        {
            try
            {
                using (WebClient webclient = new WebClient())
                {
                    webclient.DownloadFile(uri, TMP_FILE_PATH);
                }

                Logger.Log(Logger.Module.VK, Logger.Type.Debug, $"File '{uri}' downloaded");
            }
            catch (Exception exception)
            {
                Logger.Log(Logger.Module.VK, Logger.Type.Error, $"Downloading from '{uri}' failed ({exception.Message})");

                return false;
            }

            return true;
        }
    }
}