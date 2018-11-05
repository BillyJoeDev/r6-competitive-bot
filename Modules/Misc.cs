using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace ALTDiscordBot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        static Random rnd = new Random();
        List<string> travisjokeslist = new List<string>(new string[] { "you have a weird beard", "your mexican", "go hop the border again" });

        [Command("help")]
        public async Task HelpCommand()
        {
            var config = ConfigClass.GetOrCreateConfig(Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithTitle("ALT Bot | Commands");
            embed.AddField(config.CommandPrefix + "serverinfo", "Get current discord servers information.");
            embed.AddField(config.CommandPrefix + "google [what to google]", "Get google search link to anything.");
            embed.AddField(config.CommandPrefix + "travisjoke", "Get a good joke to laugh to.");
            embed.AddField(config.CommandPrefix + "ping", "Get your current ping.");
            embed.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

            var dm = await Context.User.GetOrCreateDMChannelAsync();
            await dm.SendMessageAsync("", false, embed);

            if (Helpers.UserHasRole(Context, config.ModRole) || Helpers.UserHasRole(Context, config.AdminRole) || Context.User.Id == config.OwnerId)
            {
                var embedmod = new EmbedBuilder();
                embedmod.WithTitle("ALT Bot | Mod Commands");
                embedmod.AddField(config.CommandPrefix + "announce [message]", "Announce message to announcment channel, for the whole server to see.");
                embedmod.AddField(config.CommandPrefix + "clear [amount]", "Clear # amount of lines of chat.");
                embedmod.AddField(config.CommandPrefix + "kick [@player] [reason]", "Kick a user from the discord server.");
                embedmod.AddField(config.CommandPrefix + "ban [@player] [reason]", "Ban a user from the discord server.");
                embedmod.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

                var dm1 = await Context.User.GetOrCreateDMChannelAsync();
                await dm1.SendMessageAsync("", false, embedmod);
            }

            if (Helpers.UserHasRole(Context, config.AdminRole) || Context.User.Id == config.OwnerId)
            {
                var embedadmin = new EmbedBuilder();
                embedadmin.WithTitle("ALT Bot | Admin Commands");
                embedadmin.AddField(config.CommandPrefix + "wc [#channel]", "Set welcome channel, this chanel will be used for welcoming new users, etc.");
                embedadmin.AddField(config.CommandPrefix + "ac [#channel]", "Set announcements channel, this chanel will be used for announcing messages, etc.");
                embedadmin.AddField(config.CommandPrefix + "lc [#channel]", "Set logging channel, this chanel will be used for logging messages sent by bot.");
                embedadmin.AddField(config.CommandPrefix + "servername [servername]", "Sets the discord servers name.");
                embedadmin.AddField(config.CommandPrefix + "rename [@player] [new name]", "Rename a user to change how they are displayed to other users.");
                embedadmin.AddField(config.CommandPrefix + "serverprefix [prefix]", "Change the bots prefix for how its called by other users.");
                embedadmin.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

                var dm2 = await Context.User.GetOrCreateDMChannelAsync();
                await dm2.SendMessageAsync("", false, embedadmin);
            }

            if (Context.User.Id == config.OwnerId)
            {
                var embedowner = new EmbedBuilder();
                embedowner.WithTitle("ALT Bot | Owner Commands");
                embedowner.AddField(config.CommandPrefix + "createconfig", "Create discord servers config if it dosen't exsist.");
                embedowner.AddField(config.CommandPrefix + "config", "Display the discord servers config and its details.");
                embedowner.AddField(config.CommandPrefix + "setgame [game name]", "Set bots current game that its playing.");
                embedowner.AddField(config.CommandPrefix + "status [dnd-idle-online-offline]", "Sets the discord bots status.");
                embedowner.AddField(config.CommandPrefix + "modrole [role name]", "Set the mod role of the server.");
                embedowner.AddField(config.CommandPrefix + "adminrole [role name]", "Set the admin role of the server.");
                embedowner.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

                var dm3 = await Context.User.GetOrCreateDMChannelAsync();
                await dm3.SendMessageAsync("", false, embedowner);
            }
        }

        [Command("serverinfo")]
        [Alias("sinfo")]
        public async Task ServerInformationCommand()
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Server Information");
            embed.AddField("Name", Context.Guild.Name);
            embed.AddField("Created", Context.Guild.CreatedAt.UtcDateTime);
            embed.AddField("Users", Context.Guild.Users.Count); ;
            embed.AddField("Region", Context.Guild.VoiceRegionId);
            embed.WithThumbnailUrl(Context.Guild.IconUrl);
            embed.WithFooter("Requested by " + Context.User.Username);
            embed.WithColor(new Color(0, 255, 0));

            await ReplyAsync("", false, embed);
        }

        [Command("google")]
        public async Task Google([Remainder] string search)
        {
            search = search.Replace(' ', '+');
            var searchUrl = $"https://google.com/search?q={search}";
            var embed = new EmbedBuilder();
            embed.WithDescription(searchUrl);
            embed.WithFooter("Google Search by " + Context.User.Username);
            embed.WithColor(new Color(0,255,0));
            embed.WithThumbnailUrl(
                "https://upload.wikimedia.org/wikipedia/commons/thumb/5/53/Google_%22G%22_Logo.svg/2000px-Google_%22G%22_Logo.svg.png");

            await ReplyAsync("", false, embed);
        }

        [Command("travisjoke")]
        public async Task travisjoke()
        {
            int r = rnd.Next(travisjokeslist.Count);
            await ReplyAsync(Context.User.Mention + ", " + (string)travisjokeslist[r]);
        }

        [Command("ping")]
        public async Task ping()
        {
            await ReplyAsync(Context.User.Mention + ", your ping is: " + Context.Client.Latency);
        }

        public static string ConvertBoolean(bool boolean)
        {
            return boolean ? "**On**" : "**Off**";
        }
    }
}
