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
            Error
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

            ConsoleColor color = Console.ForegroundColor;
            switch (type)
            {
                case Type.Debug:
                    color = ConsoleColor.DarkYellow;
                    break;

                case Type.Error:
                    color = ConsoleColor.Red;
                    break;

                case Type.Info:
                    color = ConsoleColor.Blue;
                    break;

                case Type.Warning:
                    color = ConsoleColor.DarkMagenta;
                    break;
            }

            var old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(str);

            Console.ForegroundColor = old;
        }
    }
}
