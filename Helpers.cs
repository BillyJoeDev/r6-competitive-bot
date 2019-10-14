using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace R6DiscordBot
{
    class Helpers
    {
        private static readonly DiscordSocketClient _client = Program._client;

        public static EmbedBuilder CreateEmbed(SocketCommandContext ctx, string desc)
        {
            var embed = new EmbedBuilder().WithDescription(desc).WithColor(0,255,0).WithFooter(Utilities.GetFormattedLocaleMsg("CommandFooter", ctx.User.Username));
            return embed;
        }

        public static async Task SendMessage(SocketCommandContext ctx, EmbedBuilder embed = null, string msg = "")
        {
            if (embed == null)
                await ctx.Channel.SendMessageAsync(msg);
            else
                await ctx.Channel.SendMessageAsync(msg, false, embed);
        }

        public static IEnumerable<SocketRole> GetRole(SocketCommandContext ctx, string roleName)
        {
            var role = ctx.Guild.Roles.Where(x => x.Name == roleName);
            return role;
        }

        public static SocketTextChannel GetChannelById(ulong channelId)
        {
            return Program._client.GetChannel(channelId) as SocketTextChannel;
        }

        internal static bool UserHasRole(SocketCommandContext ctx, ulong roleId)
        {
            var targetRole = ctx.Guild.Roles.FirstOrDefault(r => r.Id == roleId);
            var gUser = ctx.User as SocketGuildUser;

            return gUser.Roles.Contains(targetRole);
        }
    }
}
