using VkNet;
using VkNet.Model;

namespace AstroBot.VK.Commands
{
    public abstract class Command
    {
        public static string AnswerOk { get; } = "Success";
        public static string AnswerError { get; } = "Failure"; 
        public static string AnswerInfo { get; } = "Not implemented yet";

        public abstract string Name { get; }

        public abstract void Execute(Message msg, VkApi client);

        public bool Contains(string command)
        {
            return /* command.Contains(Settings.Name) && */ command.Contains(this.Name);
        }
    }
}