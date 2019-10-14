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

        [Command("verify")]
        public async Task verify(string region, string playerid)
        {
            var config = ConfigClass.GetOrCreateConfig(Context.Guild.Id, Context.Guild.OwnerId);
            var embedMessage = new EmbedBuilder();
            await Context.Message.DeleteAsync();

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
                    embed.AddField("Region","We could not verify that region, please make sure its a proper region.");
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

            SocketGuildUser user = Context.Message.Author as SocketGuildUser;
            var rankToAdd = Context.Guild.Roles.FirstOrDefault(x => x.Name == role);
            var regionToAdd = Context.Guild.Roles.FirstOrDefault(x => x.Name == region.ToUpper());

            await user.AddRoleAsync(rankToAdd);
            await user.AddRoleAsync(regionToAdd);
            await user.ModifyAsync(u => { u.Nickname = name; });
        }
    }
}
