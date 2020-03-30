using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using R6DiscordBot.Modules;

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
                await ctx.Channel.SendMessageAsync(msg, false, embed.Build());
        }

        public static IEnumerable<SocketRole> GetRole(SocketGuild guild, string roleName)
        {
            var role = guild.Roles.Where(x => x.Name == roleName);
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

        public static Dictionary<string,string> GetRankName(string playerid)
        {
            PlayerInfo root;
            using (var httpClient = new WebClient())
            {
                var json = httpClient.DownloadString("https://r6.apitab.com/player/" + playerid + "&u=unix");
                root = JsonConvert.DeserializeObject<PlayerInfo>(json);
            }

            Dictionary<string, string> pass = new Dictionary<string, string>()
            {
                { root.Ranked.Rankname, root.Player.PName }
            };

            return pass;
        }

        public static void RemoveRanks(SocketGuildUser user)
        {
            List<string> Ranks = new List<string>
            {
                "Unranked/Low Rank",
                "Gold 3",
                "Gold 2",
                "Gold 1",
                "Platinum 3",
                "Platinum 2",
                "Platinum 1",
                "Diamond",
                "Champion"
            };

            foreach (SocketRole role in user.Roles) {
                if (Ranks.Contains(role.Name))
                {
                    user.RemoveRoleAsync(role);
                }
            }
        }

        public static async Task UpdateRanks()
        {
            while (true)
            {
                await Task.Delay(900000);
                var guild = _client.GetGuild(692948711659012157);

                List<UsersBase> Users = new List<UsersBase>();
                Users.Clear();

                foreach (UsersBase user in UsersClass.GetAllUsers())
                    Users.Add(user);

                foreach (UsersBase user in Users)
                {
                    Console.WriteLine("Updating " + user.UplayID);
                    SocketGuildUser usersdiscord = guild.GetUser(user.DiscordID);

                    Dictionary<string,string> PlayerInfo = GetRankName(user.UplayID);
                    var targetRole = guild.Roles.FirstOrDefault(r => r.Name == PlayerInfo.Keys.ElementAt(0));
                    if (targetRole == null)
                    {
                        var role = guild.Roles.FirstOrDefault(r => r.Name == "Unranked/Low Rank");
                        RemoveRanks(usersdiscord);
                        await usersdiscord.AddRoleAsync(role);
                    }
                    else
                    {
                        if (!usersdiscord.Roles.Contains(targetRole))
                        {
                            RemoveRanks(usersdiscord);
                            await usersdiscord.AddRoleAsync(targetRole);
                        }
                    }

                    if (usersdiscord.Nickname != PlayerInfo.Values.ElementAt(0))
                    {
                        Console.WriteLine("Updating players name");
                        await usersdiscord.ModifyAsync(u => { u.Nickname = PlayerInfo.Values.ElementAt(0); });
                    }
                }                
            }
        }
    }
}
