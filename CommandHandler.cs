using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;

namespace ALTDiscordBot
{
    class CommandHandler
    {
        public static DiscordSocketClient _client;
        public static CommandService _service;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            await _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;

            if (msg == null) return;
            var context = new SocketCommandContext(_client, msg);
            if (context.User.IsBot) return;

            var config = ConfigClass.GetConfig(context.Guild.Id) ?? ConfigClass.CreateConfig(context.Guild.Id);
            var prefix = config.CommandPrefix ?? Config.bot.prefix;

            if (config.EmbedColour1 == 0 && config.EmbedColour2 == 0 && config.EmbedColour3 == 0)
            {
                config.EmbedColour1 = 112;
                config.EmbedColour2 = 0;
                config.EmbedColour3 = 251;
                ConfigClass.SaveConfig();
            }

            int argPos = 0;
            if (msg.HasStringPrefix(prefix, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _service.ExecuteAsync(context, argPos);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }
    }
}
