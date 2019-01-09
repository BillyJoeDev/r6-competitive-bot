using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace ALTDiscordBot.Modules
{
    public class Admins : ModuleBase<SocketCommandContext>
    {

        [Command("wc")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task SetWelcomeChannel(SocketGuildChannel chnl)
        {
            var config = ConfigClass.GetConfig(Context.Guild.Id) ?? ConfigClass.CreateConfig(Context.Guild.Id);
            var embed = new EmbedBuilder().WithDescription($"Set this server's welcome channel to #{chnl}.").WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3)).WithFooter(Utilities.GetFormattedLocaleMsg("CommandFooter", Context.User.Username));
            config.WelcomeChannel = chnl.Id;
            ConfigClass.SaveConfig();

            await Helpers.SendLog(Context, embed);
        }

        [Command("ac")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task SetAnnouncementsChannel(SocketGuildChannel chnl)
        {
            var config = ConfigClass.GetConfig(Context.Guild.Id) ?? ConfigClass.CreateConfig(Context.Guild.Id);
            var embed = new EmbedBuilder().WithDescription($"Set this server's announcements channel to #{chnl}.").WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3)).WithFooter(Utilities.GetFormattedLocaleMsg("CommandFooter", Context.User.Username));
            config.AnnouncementsChannel = chnl.Id;
            ConfigClass.SaveConfig();

            await Helpers.SendLog(Context, embed);
        }

        [Command("dc")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task SetDevelopmentChannel(SocketGuildChannel chnl)
        {
            var config = ConfigClass.GetConfig(Context.Guild.Id) ?? ConfigClass.CreateConfig(Context.Guild.Id);
            var embed = new EmbedBuilder().WithDescription($"Set this server's development showcase channel to #{chnl}.").WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3)).WithFooter(Utilities.GetFormattedLocaleMsg("CommandFooter", Context.User.Username));
            config.AnnouncementsChannel = chnl.Id;
            ConfigClass.SaveConfig();

            await Helpers.SendLog(Context, embed);
        }

        [Command("lc")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task SetLoggingChannel(SocketGuildChannel chnl)
        {
            var config = ConfigClass.GetConfig(Context.Guild.Id) ?? ConfigClass.CreateConfig(Context.Guild.Id);
            var embed = new EmbedBuilder().WithDescription($"Set this server's logging channel to #{chnl}.").WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3)).WithFooter(Utilities.GetFormattedLocaleMsg("CommandFooter", Context.User.Username));
            config.LoggingChannel = chnl.Id;
            ConfigClass.SaveConfig();

            await Helpers.SendLog(Context, embed);
        }

        [Command("servername")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ModifyServerName([Remainder] string name)
        {
            var config = ConfigClass.GetOrCreateConfig(Context.Guild.Id);
            await Context.Guild.ModifyAsync(x => x.Name = name);
            var embed = new EmbedBuilder();
            embed.WithDescription($"Set this server's name to **{name}**!");
            embed.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));
            embed.WithFooter(Utilities.GetFormattedLocaleMsg("CommandFooter", Context.User.Username));

            await Helpers.SendLog(Context, embed);
        }


        [Command("rename")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetUsersNickname(SocketGuildUser user, [Remainder] string nick)
        {
            EmbedBuilder embed;
            embed = Helpers.CreateEmbed(Context, nick.Length >= 33 ? $"You can't have nicknames longer than 32 characters. The nickname you specified was {nick.Length} characters long." : $"Set <@{user.Id}>'s nickname on this server to **{nick}**!");
            await user.ModifyAsync(x => x.Nickname = nick);

            await Helpers.SendLog(Context, embed);
        }

        [Command("serverprefix")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetGuildPrefix([Remainder] string prefix)
        {
            var config = ConfigClass.GetConfig(Context.Guild.Id) ?? ConfigClass.CreateConfig(Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithDescription("Done.");
            embed.WithFooter(Utilities.GetFormattedLocaleMsg("CommandFooter", Context.User.Username));
            embed.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));
            config.CommandPrefix = prefix;
            ConfigClass.SaveConfig();

            await Helpers.SendLog(Context, embed);
        }
    }
}
