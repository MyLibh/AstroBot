using VkNet.Model.GroupUpdate;

using AstroBot.Util;

namespace AstroBot.VK
{
    class MessageController
    {
        public static void Update(object sender, GroupUpdate e)
        { 
            var commands = Bot.Commands;
            var msg = e.Message;
            var client = Bot.Get();

            Logger.Log(Logger.Module.VK, Logger.Type.Debug, msg.UserId + " > " + msg.Body);

            foreach (var command in commands)
                if (command.Contains(msg.Body))
                {
                    command.Execute(msg, client);

                    return;
                }

            var undef_msg = new VK.Commands.UndefinedCommand();
            undef_msg.Execute(msg, client);
        }
    }
}
