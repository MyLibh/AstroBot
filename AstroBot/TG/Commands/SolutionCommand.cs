using System;

using Telegram.Bot;
using Telegram.Bot.Types;

using AstroBot.TG;
using AstroBot.DB;
using AstroBot.DB.Students;
using AstroBot.Util;
using AstroBot.GD;

using System.Net;

namespace AstroBot.TG.Commands
{
    public class SolutionCommand : Command
    {
        public new string AnswerOk => "Решение загружено. Можно получить новое задание /task";
        public new string AnswerError => "Увы, но я не смог открыть твои файлы";
        public new string AnswerInfo => "Чтобы сдать решение, введите:\n /solution <Фото/Документы>.";
        public string AnswerBadAnswer => "Вы не проверили ответ, используйте /answer";

        public override string Name => "solution";

        public override void Execute(Message msg, TelegramBotClient client)
        {
            var chatId = msg.Chat.Id;
            var msgId = msg.MessageId;
            try
            {
                if (DataBase.Students.Exist(Students.ExistOption.TGId, msg.From.Id.ToString()))
                {
                    string str = msg.Text;
                    String[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (words.Length != 2)
                        throw new FormatException("Неверный ввод");

                    if (DataBase.Tasks.CanSaveSolution(DB.Tasks.Tasks.IdType.TGId, msg.From.Id.ToString()))
                    {
                        foreach(var photo in msg.Photo)
                        {
                            downloadFile(client, photo.FileId);
                            //GoogleDrive.Upload();
                            //delete
                        }
                        client.SendTextMessageAsync(chatId, AnswerOk, replyToMessageId: msgId);
                    }
                    else
                        client.SendTextMessageAsync(chatId, AnswerBadAnswer, replyToMessageId: msgId);
                }
                else
                {
                    client.SendTextMessageAsync(chatId, "Данный аккаунт незарегистрирован, используйте /register", replyToMessageId: msgId);

                    return;
                }
            }
            catch (ArgumentException exception)
            {
                client.SendTextMessageAsync(chatId, exception.Message, replyToMessageId: msgId);

                Logger.Log(Logger.Module.TG, Logger.Type.Warning, $"{msg.From.Username}: {msg.Text} ({exception.Message})");
            }
            catch (Exception exception)
            {
                client.SendTextMessageAsync(chatId, AnswerError + "(" + exception.Message + ")\n" + AnswerInfo, replyToMessageId: msgId);

                Logger.Log(Logger.Module.TG, Logger.Type.Warning, $"{msg.From.Username}: {msg.Text} ({exception.Message})");
            }

            Logger.Log(Logger.Module.TG, Logger.Type.Info, $"{msg.From.Username}: {msg.Text}");
        }

        private static async void downloadFile(TelegramBotClient client, string fileId)
        {
            try
            {
                var file = await client.GetFileAsync(fileId);
                var download_url = @"https://api.telegram.org/file/" + Config.Token + "/" + file.FilePath;
                using (WebClient webclient = new WebClient())
                {
                    webclient.DownloadFile(new Uri(download_url), @"D:\swap\tmp.png");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error downloading: " + ex.Message);
            }
        }
    }
}