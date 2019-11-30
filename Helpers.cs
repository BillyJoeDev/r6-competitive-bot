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
        public static readonly string _path = @"C:\\Users\\admin\\Pictures\\bot\\Resources\\Users.json";

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

        public static Users PlayerExsist(string id, bool playerid = true)
        {
            var jsonData = File.ReadAllText(_path);
            var usersList = JsonConvert.DeserializeObject<List<Users>>(jsonData) ?? new List<Users>();

            foreach (Users user in usersList)
            {
                if (playerid)
                {
                    if (user.PlayerID == id) return user;
                } else
                {
                    if (user.DiscordID == Convert.ToUInt64(id)) return user;
                }
            }

            return null;
        }

        public static string GetRank(int rank, int mmr)
        {
            string rankToReturn = "";

            if (rank == 0)
                rankToReturn = "Unranked";
            else if (mmr <= 1199)
                rankToReturn = "Copper 5";
            else if (mmr <= 1299)
                rankToReturn = "Copper 4";
            else if (mmr <= 1399)
                rankToReturn = "Copper 3";
            else if (mmr <= 1499)
                rankToReturn = "Copper 2";
            else if (mmr <= 1599)
                rankToReturn = "Copper 1";
            else if (mmr <= 1699)
                rankToReturn = "Bronze 5";
            else if (mmr <= 1799)
                rankToReturn = "Bronze 4";
            else if (mmr <= 1899)
                rankToReturn = "Bronze 3";
            else if (mmr <= 1999)
                rankToReturn = "Bronze 2";
            else if (mmr <= 2099)
                rankToReturn = "Bronze 1";
            else if (mmr <= 2199)
                rankToReturn = "Silver 5";
            else if (mmr <= 2299)
                rankToReturn = "Silver 4";
            else if (mmr <= 2399)
                rankToReturn = "Silver 3";
            else if (mmr <= 2499)
                rankToReturn = "Silver 2";
            else if (mmr <= 2599)
                rankToReturn = "Silver 1";
            else if (mmr <= 2599)
                rankToReturn = "Silver 1";
            else if (mmr <= 2799)
                rankToReturn = "Gold 3";
            else if (mmr <= 2999)
                rankToReturn = "Gold 2";
            else if (mmr <= 3199)
                rankToReturn = "Gold 1";
            else if (mmr <= 3599)
                rankToReturn = "Platinum 3";
            else if (mmr <= 3999)
                rankToReturn = "Platinum 2";
            else if (mmr <= 4399)
                rankToReturn = "Platinum 1";
            else if (mmr <= 4999)
                rankToReturn = "Diamond";
            else if (mmr >= 5000)
                rankToReturn = "Champion";

            return rankToReturn;
        }

        public static string GetRole(string playerName)
        {
            string role = "Unranked/Low Rank";
            RootPlayerSearch root;
            using (var httpClient = new WebClient())
            {
                var json = httpClient.DownloadString(" https://r6tab.com/api/search.php?platform=uplay&search=" + playerName);
                root = JsonConvert.DeserializeObject<RootPlayerSearch>(json);
            }

            int rank = Convert.ToInt32(root.results[0].p_currentrank);
            int mmr = Convert.ToInt32(root.results[0].p_currentmmr);
            string result = GetRank(rank, mmr);

            if (result.Contains("Gold")) role = "Gold";
            if (result.Contains("Platinum")) role = "Platinum";
            if (result.Contains("Diamond")) role = "Diamond";
            if (result.Contains("Champion")) role = "Champion";

            return role;
        }

        public static void RemoveRanks(SocketGuildUser user)
        {
            List<string> Ranks = new List<string>
            {
                "Unranked/Low Rank",
                "Gold",
                "Platinum",
                "Diamond"
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
                var guild = _client.GetGuild(596406512780574745);

                foreach (SocketGuildUser user in guild.Users)
                {
                    Users exsists = PlayerExsist(user.Id.ToString(), false);
                    if (exsists == null)
                        continue;
                    else
                    {
                        string rank = GetRole(exsists.PlayerName);
                        var targetRole = guild.Roles.FirstOrDefault(r => r.Name == rank);
                        if (!user.Roles.Contains(targetRole))
                        {
                            RemoveRanks(user);
                            await user.AddRoleAsync(targetRole);
                        }
                    }
                }
            }
        }


    }
}
