using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;

namespace R6DiscordBot.Modules
{
    public class Owner : ModuleBase<SocketCommandContext>
    {
        [Command("serverprefix")]
        [RequireOwner]
        public async Task SetGuildPrefix([Remainder] string prefix)
        {
            var config = ConfigClass.GetConfig(Context.Guild.Id) ?? ConfigClass.CreateConfig(Context.Guild.Id, Context.Guild.OwnerId);
            var embed = new EmbedBuilder();
            embed.WithDescription("Done.");
            embed.WithFooter(Utilities.GetFormattedLocaleMsg("CommandFooter", Context.User.Username));
            embed.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

            SocketTextChannel channel = Helpers.GetChannelById(config.TenManChannelID);
            await channel.SendMessageAsync("", false, embed);

            config.CommandPrefix = prefix;
            ConfigClass.SaveConfig();
        }

        [Command("createconfig")]
        [RequireOwner]
        public async Task CreateConfigIfOneDoesntExist(ulong serverId = 0)
        {
            if (serverId == 0) serverId = Context.Guild.Id;

            var g = Program._client.GetGuild(serverId);
            var embed = Helpers.CreateEmbed(Context, $"Created a config for the guild `{g.Name}! ({serverId})`");
            var targetConfig = ConfigClass.GetConfig(serverId);

            var serverIds = new List<ulong>();

            foreach (var server in Program._client.Guilds) serverIds.Add(server.Id);

            if (targetConfig == null && serverIds.Contains(serverId))
                ConfigClass.CreateConfig(serverId, g.OwnerId);
            else
                embed.WithDescription($"Couldn't create a config for {serverId}. Either they already have a config, or I don't have access to that server.");
        }
    }
}
