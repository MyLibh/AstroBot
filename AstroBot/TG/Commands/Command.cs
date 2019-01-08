using Telegram.Bot;
using Telegram.Bot.Types;

namespace AstroBot.TG.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }

        public abstract void Execute(Message msg, TelegramBotClient client);

        public bool Contains(string command)
        {
            return /* command.Contains(Settings.Name) && */ command.Contains(this.Name);
        }
    }
}