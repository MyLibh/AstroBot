using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

using AstroBot.Util;

namespace AstroBot.GD
{
    static class GoogleDrive
    {
        private static readonly string CREDENTIALS_PATH = @"config/credentials.json";

        private static string[] Scopes = { DriveService.Scope.Drive };
        private static string ApplicationName = "Solutions Uploader";
        private static DriveService driveService;
        private static bool inited = false;

        public static void Init()
        {
            if (!inited)
            {
                Logger.Log(Logger.Module.Core, Logger.Type.Debug, "Starting Google authorization");

                UserCredential credential;

                using (var stream = new FileStream(CREDENTIALS_PATH, FileMode.Open, FileAccess.Read))
                {
                    string credPath = "token.json";
                    var secrets = GoogleClientSecrets.Load(stream);
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        secrets.Secrets,
                        Scopes,
                        secrets.Secrets.ClientId,
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;

                    Logger.Log(Logger.Module.Core, Logger.Type.Debug, $"\tCredential file saved to '{credPath}'");
                }

                driveService = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                inited = true;

                Logger.Log(Logger.Module.Core, Logger.Type.Debug, "Google authorization completed");
            }
        }

        public static void Upload(string source, string dest)
        {
            if (!inited)
                return;

            var body = new Google.Apis.Drive.v3.Data.File();
            body.Name = dest;
            body.MimeType = getMimeType(source);
            body.Parents = new List<string>() { createDirectory("sdfsd").Id };

            byte[] byteArray = File.ReadAllBytes(source);
            MemoryStream stream = new MemoryStream(byteArray);
            FilesResource.CreateMediaUpload request = driveService.Files.Create(body, stream, body.MimeType);

            if (request.Upload().Exception != null)
                Logger.Log(Logger.Module.Core, Logger.Type.Error, request.Upload().Exception.Message);
            else
                Logger.Log(Logger.Module.Core, Logger.Type.Debug, $"File '{Path.GetFileName(source)}' successfully uploaded");
        }

        private static string getMimeType(string filename)
        {
            switch (Path.GetExtension(filename))
            {
                case ".avi":  return "video/x-msvideo";
                case ".css":  return "text/css";
                case ".doc":  return "application/msword";
                case ".htm":
                case ".html": return "text/html";
                case ".bmp":  return "image/bmp"; 
                case ".gif":  return "image/gif"; 
                case ".jpeg": 
                case ".jpg":  return "image/jpeg"; 
                case ".docx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; 
                case ".pdf":  return "application/pdf"; 
                case ".ppt":  return "application/vnd.ms-powerpoint"; 
                case ".pptx": return "application/vnd.openxmlformats-officedocument.presentationml.presentation"; 
                case ".xls":  return "application/vnd.ms-excel"; 
                case ".xlsx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; 
                case ".txt":  return "text/plain"; 
                case ".zip":  return "application/zip"; 
                case ".rar":  return "application/x-rar-compressed"; 
                case ".mp3":  return "audio/mpeg"; 
                case ".mp4":  return "video/mp4"; 
                case ".png":  return "image/png";

                default:      return "application/octet-stream"; 
            }
    }

        private static Google.Apis.Drive.v3.Data.File createDirectory(string name)
        {
            var body = new Google.Apis.Drive.v3.Data.File();
            body.Name = name;
            body.MimeType = "application/vnd.google-apps.folder";
            body.Parents = new List<string>() { "root" };

            try
            {
                FilesResource.CreateRequest request = driveService.Files.Create(body);

                return request.Execute();
            }
            catch (Exception exception)
            {
                Logger.Log(Logger.Module.Core, Logger.Type.Error, $"Cannot create directory({exception.Message})");
            }

            return null;
        }
    }
}
