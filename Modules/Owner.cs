using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;

namespace ALTDiscordBot.Modules
{
    public class Owner : ModuleBase<SocketCommandContext>
    {
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
                ConfigClass.CreateConfig(serverId);
            else
                embed.WithDescription($"Couldn't create a config for {serverId}. Either they already have a config, or I don't have access to that server.");

            await Helpers.SendLog(Context, embed);
        }

        [Command("config")]
        [RequireOwner]
        public async Task MasterConfig()
        {
            var config = ConfigClass.GetOrCreateConfig(Context.Guild.Id);
            var embed = Helpers.CreateEmbed(Context, $"Server ID: {config.ServerId}\n" +
                                                     $"Owner: <@{config.OwnerId}>");

            var modRole = "**Not set**";
            var adminRole = "**Not set**";
            if (config.ModRole != 0)
                modRole = $"**{Context.Guild.Roles.First(role => role.Id == config.ModRole).Name}**";

            if (config.AdminRole != 0)
                adminRole = $"**{Context.Guild.Roles.First(role => role.Id == config.AdminRole).Name}**";


            if (config.WelcomeChannel != 0)
                embed.AddField("Welcome/Leaving", "On:\n" +
                                                  $"- Channel: <#{config.WelcomeChannel}>\n" +
                                                  $"- WelcomeMsg: {config.WelcomeMessage}\n" +
                                                  $"- Colours: {config.WelcomeColour1} {config.WelcomeColour2} {config.WelcomeColour3}\n" +
                                                  $"- LeavingMsg: {config.LeavingMessage}");
            else
                embed.AddField("Welcome/Leaving", "Off");


            embed.AddField("Other", $"Server Logging Channel: <#{config.LoggingChannel}>\n" + $"Mod Role: {modRole}\n" + $"Admin Role: {adminRole}\n");

            embed.WithThumbnailUrl(Context.Guild.IconUrl);

            await Helpers.SendMessage(Context, embed);
        }

        [Command("setgame")]
        [RequireOwner]
        public async Task SetBotGame([Remainder] string game)
        {
            var client = Program._client;
            var config = ConfigClass.GetConfig(Context.Guild.Id);

            var embed = new EmbedBuilder();
            embed.WithDescription($"Set the bot's game to {game}");
            embed.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));
            embed.WithFooter(Utilities.GetFormattedLocaleMsg("CommandFooter", Context.User.Username));
            await client.SetGameAsync(game);

            await Helpers.SendLog(Context, embed);
        }

        [Command("status")]
        [RequireOwner]
        public async Task SetBotStatus(string status)
        {
            var config = ConfigClass.GetConfig(Context.Guild.Id);
            var embed = new EmbedBuilder().WithDescription($"Set the status to {status}.").WithFooter(Utilities.GetFormattedLocaleMsg("CommandFooter", Context.User.Username)).WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

            var client = Program._client;

            switch (status)
            {
                case "dnd":
                    await client.SetStatusAsync(UserStatus.DoNotDisturb);
                    break;
                case "idle":
                    await client.SetStatusAsync(UserStatus.Idle);
                    break;
                case "online":
                    await client.SetStatusAsync(UserStatus.Online);
                    break;
                case "offline":
                    await client.SetStatusAsync(UserStatus.Invisible);
                    break;
            }

            await Helpers.SendLog(Context, embed);
        }

        [Command("adminrole")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetServerAdminRole([Remainder] string roleName)
        {
            var config = ConfigClass.GetOrCreateConfig(Context.Guild.Id);
            var embed = new EmbedBuilder()
                .WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3))
                .WithFooter(Utilities.GetFormattedLocaleMsg("CommandFooter", Context.User.Username));
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == roleName);
            if (role == null)
            {
                embed.WithDescription($"The role `{roleName}` doesn't exist on this server. Remember that this command is cAsE sEnSiTiVe.");
            }
            else
            {
                embed.WithDescription($"Set the Administrator role to **{roleName}** for this server!");
                config.AdminRole = role.Id;
                ConfigClass.SaveConfig();
            }

            await Helpers.SendLog(Context, embed);
        }

        [Command("modrole")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetServerModRole([Remainder] string roleName)
        {
            var config = ConfigClass.GetOrCreateConfig(Context.Guild.Id);
            var embed = new EmbedBuilder()
                .WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3))
                .WithFooter(Utilities.GetFormattedLocaleMsg("CommandFooter", Context.User.Username));

            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == roleName);
            if (role == null)
            {
                embed.WithDescription(
                    $"The role `{roleName}` doesn't exist on this server. Remember that this command is cAsE sEnSiTiVe.");
            }
            else
            {
                embed.WithDescription($"Set the Moderator role to **{roleName}** for this server!");
                config.ModRole = role.Id;
                ConfigClass.SaveConfig();
            }

            await Helpers.SendLog(Context, embed);
        }
    }
}
