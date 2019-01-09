using System;

using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace AstroBot.TG
{
    public static class MessageController
    {
        public static void Update(object sender, MessageEventArgs e)
        {
            var commands = Bot.Commands;
            var msg      = e.Message;
            var client   = Bot.Get();

            if (msg.Type != MessageType.Text)
                return;

            Console.WriteLine(e.Message.From.Username + " > " + e.Message.Text);

            foreach (var command in commands)
                if (command.Contains(msg.Text))
                {
                    command.Execute(msg, client);

                    return;
                }

            var undef_msg = new TG.Commands.UndefinedCommand();
            undef_msg.Execute(msg, client);
        }
    }
}
