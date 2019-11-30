using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Net.NetworkInformation;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Json;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Timers;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;

namespace R6DiscordBot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [RequireOwner]
        [Command("setchannel")]
        public async Task setchannel(SocketGuildChannel chnl)
        {
            var config = ConfigClass.GetOrCreateConfig(Context.Guild.Id, Context.Guild.OwnerId);
            await Context.Message.DeleteAsync();
            config.TenManChannelID = chnl.Id;
            ConfigClass.SaveConfig();
        }

        [RequireOwner]
        [Command("setemote")]
        public async Task setemote(string joinqueueemote)
        {
            var config = ConfigClass.GetOrCreateConfig(Context.Guild.Id, Context.Guild.OwnerId);
            await Context.Message.DeleteAsync();
            config.TenManJoinQueEmote = joinqueueemote;
            ConfigClass.SaveConfig();
        }

        [RequireOwner]
        [Command("setuptenmans")]
        public async Task setuptenmans()
        {
            var config = ConfigClass.GetOrCreateConfig(Context.Guild.Id,Context.Guild.OwnerId);
            var embedMessage = new EmbedBuilder();
            await Context.Message.DeleteAsync();

            embedMessage.WithTitle("**R6 Bot | 10 Mans Queue**");
            embedMessage.WithDescription("**Currently in Queue: NONE**");
            embedMessage.AddField("JOIN/EXIT QUEUE:", config.TenManJoinQueEmote);
            embedMessage.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

            SocketTextChannel channel = Helpers.GetChannelById(config.TenManChannelID);
            var message = await channel.SendMessageAsync("", false, embedMessage);
            await message.AddReactionAsync(new Emoji(config.TenManJoinQueEmote));

            config.TenManMessageID = message.Id;
            ConfigClass.SaveConfig();
        }

        [Command("checkplayer")]
        [Alias("cp")]
        public async Task checkplayer(string playername)
        {
            var config = ConfigClass.GetOrCreateConfig(Context.Guild.Id, Context.Guild.OwnerId);
            var embedMessage = new EmbedBuilder();
            await Context.Message.DeleteAsync();

            RootPlayerSearch root;
            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync("https://r6tab.com/api/search.php?platform=uplay&search=" + playername);
                root = JsonConvert.DeserializeObject<RootPlayerSearch>(json);
            }

            SocketTextChannel channel = Helpers.GetChannelById(Context.Message.Channel.Id);
            if (root.results.Count == 0)
            {
                var embedVerified = new EmbedBuilder();
                embedVerified.WithTitle("R6 Competitive | Error!");
                embedVerified.AddField("No Players", "We could not find any players with that name.");
                embedVerified.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

                await channel.SendMessageAsync("", false, embedVerified);
                return;
            }

            var embedResult = new EmbedBuilder();
            embedResult.WithTitle($"R6 Competitive | {root.results[0].p_name}");
            embedResult.AddField("Player Level:", root.results[0].p_level);
            embedResult.AddField("Player Rank:", Helpers.GetRank(Convert.ToInt32(root.results[0].p_currentrank), Convert.ToInt32(root.results[0].p_currentmmr)));
            embedResult.AddField("Player Elo:", root.results[0].p_currentmmr);
            embedResult.AddField("Player KD:", GetKD(root.results[0].kd));
            embedResult.AddField("Player Platform:", root.results[0].p_platform);
            embedResult.WithThumbnailUrl(string.Format("https://r6tab.com/images/pngranks/{0}.png", Convert.ToInt32(root.results[0].p_currentrank)));
            embedResult.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));
            embedResult.WithFooter(Context.User.Username, Context.User.GetAvatarUrl());

            await channel.SendMessageAsync("", false, embedResult);
        }

        public string GetKD(string kd)
        {
            string actualKD = "";

            actualKD = string.Format("{0,0:N2}", Int32.Parse(kd) / 100.0);
            return actualKD;
        }

        [Command("verify")]
        public async Task verify(string region, string playerid)
        {
            var config = ConfigClass.GetOrCreateConfig(Context.Guild.Id, Context.Guild.OwnerId);
            var embedMessage = new EmbedBuilder();
            await Context.Message.DeleteAsync();

            Users exsist = Helpers.PlayerExsist(playerid);
            if (exsist != null)
            {
                var embedVerified = new EmbedBuilder();
                embedVerified.WithTitle("R6 Competitive | Error!");
                embedVerified.AddField("Verification Error", "There seems to be a user already verified with this player id!");
                embedVerified.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

                var dm2 = await Context.User.GetOrCreateDMChannelAsync();
                await dm2.SendMessageAsync("", false, embedVerified);
                return;
            }

            RootPlayerInfo root;
            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync("https://r6tab.com/api/player.php?p_id=" + playerid);
                root = JsonConvert.DeserializeObject<RootPlayerInfo>(json);
            }

            if (Helpers.UserHasRole(Context, 633415554354380833) || Helpers.UserHasRole(Context, 633415704082645034) || Helpers.UserHasRole(Context, 633415881443115058))
            {
                var embedVerified = new EmbedBuilder();
                embedVerified.WithTitle("R6 Competitive | Error!");
                embedVerified.AddField("Verified", "You seem to already be verified.");
                embedVerified.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

                var dm2 = await Context.User.GetOrCreateDMChannelAsync();
                await dm2.SendMessageAsync("", false, embedVerified);
                return;
            }

            int rank = 0;
            switch (region.ToLower())
            {
                case "na":
                    rank = root.p_NA_rank;
                    break;
                case "eu":
                    rank = root.p_EU_rank;
                    break;
                case "as":
                    rank = root.p_AS_rank;
                    break;
                default:
                    var embed = new EmbedBuilder();
                    embed.WithTitle("R6 Competitive | Error!");
                    embed.AddField("Region","We could not verify that region, please make sure its a proper region.");
                    embed.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

                    var dm1 = await Context.User.GetOrCreateDMChannelAsync();
                    await dm1.SendMessageAsync("", false, embed);
                    return;
            }

            string role = Helpers.GetRole(root.p_name);
            SocketGuildUser user = Context.Message.Author as SocketGuildUser;
            var rankToAdd = Context.Guild.Roles.FirstOrDefault(x => x.Name == role);
            var regionToAdd = Context.Guild.Roles.FirstOrDefault(x => x.Name == region.ToUpper());

            AddUser(root.p_name, playerid, user.Id);
            await user.AddRoleAsync(rankToAdd);
            await user.AddRoleAsync(regionToAdd);
            await user.ModifyAsync(u => { u.Nickname = root.p_name; });
        }

        private void AddUser(string name, string pid, ulong id)
        {
            // Read existing json data
            var jsonData = File.ReadAllText(Helpers._path);

            // De-serialize to object or create new list
            var usersList = JsonConvert.DeserializeObject<List<Users>>(jsonData) ?? new List<Users>();

            // Add any new employees
            usersList.Add(new Users()
            {
                PlayerName = name,
                PlayerID = pid,
                DiscordID = id
            });

            // Update json data string
            jsonData = JsonConvert.SerializeObject(usersList,Formatting.Indented);
            System.IO.File.WriteAllText(Helpers._path, jsonData);
        }

        [RequireOwner]
        [Command("verifyplayer")]
        public async Task verifyplayer(SocketUser player, string region, string playerid)
        {
            var config = ConfigClass.GetOrCreateConfig(Context.Guild.Id, Context.Guild.OwnerId);
            var embedMessage = new EmbedBuilder();
            await Context.Message.DeleteAsync();

            Users exsist = Helpers.PlayerExsist(playerid);
            if (exsist != null)
            {
                var embedVerified = new EmbedBuilder();
                embedVerified.WithTitle("R6 Competitive | Error!");
                embedVerified.AddField("Verification Error", "There seems to be a user already verified with this player id!");
                embedVerified.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

                var dm2 = await Context.User.GetOrCreateDMChannelAsync();
                await dm2.SendMessageAsync("", false, embedVerified);
                return;
            }

            RootPlayerInfo root;
            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync("https://r6tab.com/api/player.php?p_id=" + playerid);
                root = JsonConvert.DeserializeObject<RootPlayerInfo>(json);
            }

            int rank = 0;
            switch (region.ToLower())
            {
                case "na":
                    rank = root.p_NA_rank;
                    break;
                case "eu":
                    rank = root.p_EU_rank;
                    break;
                case "as":
                    rank = root.p_AS_rank;
                    break;
                default:
                    var embed = new EmbedBuilder();
                    embed.WithTitle("R6 Competitive | Error!");
                    embed.AddField("Region", "We could not verify that region, please make sure its a proper region.");
                    embed.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

                    var dm1 = await Context.User.GetOrCreateDMChannelAsync();
                    await dm1.SendMessageAsync("", false, embed);
                    return;
            }

            string role = "Unranked/Low Rank";
            if (rank == 14 || rank == 15 || rank == 16) role = "Gold";
            if (rank == 17 || rank == 18 || rank == 19) role = "Platinum";
            if (rank == 20) role = "Diamond";

            string name = root.p_name;
            SocketGuildUser user = player as SocketGuildUser;
            var rankToAdd = Context.Guild.Roles.FirstOrDefault(x => x.Name == role);
            var regionToAdd = Context.Guild.Roles.FirstOrDefault(x => x.Name == region.ToUpper());

            AddUser(root.p_name, playerid, user.Id);
            await user.AddRoleAsync(rankToAdd);
            await user.AddRoleAsync(regionToAdd);
            await user.ModifyAsync(u => { u.Nickname = name; });
        }

        [Command("joinrequest")]
        public async Task joinrequest()
        {
            var config = ConfigClass.GetOrCreateConfig(Context.Guild.Id, Context.Guild.OwnerId);
            var embedMessage = new EmbedBuilder();
            await Context.Message.DeleteAsync();

            var embedResult = new EmbedBuilder();
            embedResult.WithTitle($"R6 Competitive | Join Request");
            embedResult.WithDescription($"{Context.User.Mention} has requested to join a game. Follow the reactions below to handle the player.");
            embedResult.WithThumbnailUrl(Context.User.GetAvatarUrl());
            embedResult.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

            SocketTextChannel channel = Helpers.GetChannelById(Context.Message.Channel.Id);
            await channel.SendMessageAsync("", false, embedResult);
        }
    }
}
