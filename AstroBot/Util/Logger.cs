using System;
using System.Text;
using System.IO;

namespace AstroBot.Util
{
    static class Logger
    {
        public enum Type
        {
            Info,
            Debug,
            Warning,
            Error,
            Log
        }

        public enum Module
        {
            TG,
            VK,
            Core
        }

        private static string filename = "../../../logs/ " + DateTime.Now.ToFileTimeUtc() + ".log";

        public static void Log(Module module, Type type, string message)
        {
            string str = "<" + DateTime.Now + ">" + "{" + module.ToString() + "}" + "[" + type.ToString() + "]" + "(" + message + ")" + "\r\n"; 

            UnicodeEncoding uniencoding = new UnicodeEncoding();

            byte[] result = uniencoding.GetBytes(str);
            using (FileStream SourceStream = File.OpenWrite(filename))
            {
                SourceStream.Seek(0, SeekOrigin.End);
                SourceStream.Write(result, 0, result.Length);
            }

            Console.Write(str);
        }
    }
}
