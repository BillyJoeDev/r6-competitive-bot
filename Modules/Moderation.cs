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
    public class Moderation : ModuleBase<SocketCommandContext>
    {
        [Command("announce")]
        public async Task Announcement([Remainder]string message)
        {
            var config = ConfigClass.GetOrCreateConfig(Context.Guild.Id);
            if (Helpers.UserHasRole(Context, config.ModRole) || Helpers.UserHasRole(Context, config.AdminRole) || config.OwnerId == Context.User.Id)
            {
                var embed = new EmbedBuilder();
                embed.WithDescription(message);
                embed.WithColor(new Color(0, 255, 0));
                embed.WithFooter("Posted by " + Context.User.Username);

                await Context.Message.DeleteAsync();

                SocketTextChannel channel = Helpers.GetChannelById(config.AnnouncementsChannel);
                await channel.SendMessageAsync("", false, embed);
            }
        }

        [Command("clear")]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task ClearMessages(int amount)
        {
            var config = ConfigClass.GetOrCreateConfig(Context.Guild.Id);
            if (Helpers.UserHasRole(Context, config.ModRole) || Helpers.UserHasRole(Context, config.AdminRole) || Context.User.Id == config.OwnerId)
            {
                if (amount < 1)
                {
                    await ReplyAsync("You cannot delete less than 1 message.");
                }
                else
                {
                    var messages = await Context.Channel.GetMessagesAsync(amount).Flatten();
                    await Context.Channel.DeleteMessagesAsync(messages);
                    var msg = await ReplyAsync($"Deleted {amount} messages.");
                    await Task.Delay(1000);
                    await msg.DeleteAsync();
                }
            }
            else
            {
                var e = Helpers.CreateEmbed(Context,Utilities.GetFormattedLocaleMsg("NotEnoughPermission", Context.User.Username));
                await Helpers.SendMessage(Context, e);
            }
        }

        [Command("kick")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task KickUser(SocketGuildUser user, [Remainder] string reason = "")
        {
            var config = ConfigClass.GetOrCreateConfig(Context.Guild.Id);
            if (Helpers.UserHasRole(Context, config.ModRole) || Helpers.UserHasRole(Context, config.AdminRole) || config.OwnerId == Context.User.Id)
            {
                await user.KickAsync(reason);
                var embed = Helpers.CreateEmbed(Context,Utilities.GetFormattedLocaleMsg("KickUserMsg", user.Mention, Context.User.Mention));
                await Helpers.SendLog(Context, embed);
            }
            else
            {
                var e = Helpers.CreateEmbed(Context,Utilities.GetFormattedLocaleMsg("NotEnoughPermission", Context.User.Username));
                await Helpers.SendMessage(Context, e);
            }
        }

        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task BanUser(SocketGuildUser user, [Remainder] string reason = "Banned by a moderator.")
        {
            var config = ConfigClass.GetOrCreateConfig(Context.Guild.Id);
            if (Helpers.UserHasRole(Context, config.ModRole) || Helpers.UserHasRole(Context, config.AdminRole) || config.OwnerId == Context.User.Id)
            {
                await Context.Guild.AddBanAsync(user, 7, reason);
                var embed = new EmbedBuilder();
                embed.WithDescription(Utilities.GetFormattedLocaleMsg("BanText", user.Mention, Context.User.Mention));
                embed.WithFooter(Utilities.GetFormattedLocaleMsg("CommandFooter", Context.User.Username));
                embed.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));
                await Helpers.SendLog(Context, embed);
            }
            else
            {
                var e = Helpers.CreateEmbed(Context,Utilities.GetFormattedLocaleMsg("NotEnoughPermission", Context.User.Username));
                await Helpers.SendMessage(Context, e);
            }
        }

        [Command("unban")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task UnBanUser(IUser user)
        {
            var config = ConfigClass.GetOrCreateConfig(Context.Guild.Id);
            if (Helpers.UserHasRole(Context, config.ModRole) || Helpers.UserHasRole(Context, config.AdminRole) || config.OwnerId == Context.User.Id)
            {
                await Context.Guild.RemoveBanAsync(user);
                var embed = new EmbedBuilder();
                embed.WithDescription(Utilities.GetFormattedLocaleMsg("UnbanText", user.Mention, Context.User.Mention));
                embed.WithFooter(Utilities.GetFormattedLocaleMsg("CommandFooter", Context.User.Username));
                embed.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));
                await Helpers.SendLog(Context, embed);
            }
            else
            {
                var e = Helpers.CreateEmbed(Context, Utilities.GetFormattedLocaleMsg("NotEnoughPermission", Context.User.Username));
                await Helpers.SendMessage(Context, e);
            }
        }
    }
}
