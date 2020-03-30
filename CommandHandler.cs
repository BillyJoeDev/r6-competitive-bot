using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;
using R6DiscordBot.Modules;
using System.Net.Http;
using Newtonsoft.Json;

namespace R6DiscordBot
{
    class CommandHandler
    {
        public static DiscordSocketClient _client;
        public static CommandService _service;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            _client.MessageReceived += MessageReceived;
        }

        private async Task MessageReceived(SocketMessage s)
        {
            SocketUserMessage message = s as SocketUserMessage;

            if (message == null)
                return;

            SocketCommandContext context = new SocketCommandContext(_client, message);
            if (!context.IsPrivate)
            {
                await HandleCommands(message);
            } else
            {
                if (message.Author.IsBot) return;

                await HandleDMS(message,context);
            }
        }

        private async Task HandleDMS(SocketUserMessage s, SocketCommandContext context)
        {
            if (s == null) return;

            var embed = new EmbedBuilder();
            embed.WithTitle("**R6 Competitive | Error!**");
            embed.AddField("No Functionality", "Please use the discord to interact with me!");
            embed.WithColor(new Color(112, 0, 251));

            var dm = await s.Author.GetOrCreateDMChannelAsync();
            await dm.SendMessageAsync("", false, embed.Build());
        }

        private async Task HandleCommands(SocketUserMessage msg)
        {
            if (msg == null) return;
            
            var context = new SocketCommandContext(_client, msg);
            var config = ConfigClass.GetConfig(context.Guild.Id) ?? ConfigClass.CreateConfig(context.Guild.Id, context.Guild.OwnerId);
            var prefix = config.CommandPrefix ?? "!";

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
                var result = await _service.ExecuteAsync(context, argPos, null);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }
    }
}
