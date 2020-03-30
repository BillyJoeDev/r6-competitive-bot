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
using System.Globalization;

namespace R6DiscordBot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        public static System.Drawing.Color HexToColor(string hexString)
        {
            if (hexString.IndexOf('x') != -1)
                hexString = hexString.Replace("0x", "");

            int r, g, b = 0;

            r = int.Parse(hexString.Substring(0, 2), NumberStyles.AllowHexSpecifier);
            g = int.Parse(hexString.Substring(2, 2), NumberStyles.AllowHexSpecifier);
            b = int.Parse(hexString.Substring(4, 2), NumberStyles.AllowHexSpecifier);

            return System.Drawing.Color.FromArgb(r, g, b);
        }

        [Command("checkplayer")]
        [Alias("cp")]
        public async Task checkplayer(string playername, string region)
        {
            var config = ConfigClass.GetOrCreateConfig(Context.Guild.Id, Context.Guild.OwnerId);
            var embedMessage = new EmbedBuilder();
            await Context.Message.DeleteAsync();

            PlayerDiscord root;
            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync("https://r6.apitab.com/discord?q=r6tab+" + playername + "+" + region + "+uplay&u=unix");
                root = JsonConvert.DeserializeObject<PlayerDiscord>(json);
            }

            SocketTextChannel channel = Helpers.GetChannelById(Context.Message.Channel.Id);
            if (root.Id == null)
            {
                var embedVerified = new EmbedBuilder();
                embedVerified.WithTitle("R6 Competitive | Error!");
                embedVerified.AddField("No Players", "We could not find any players with that name.");
                embedVerified.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

                await channel.SendMessageAsync("", false, embedVerified.Build());
                return;
            }

            var color = HexToColor(root.Discord.Color);
            var embedResult = new EmbedBuilder();
            embedResult.WithTitle($"R6 Competitive | {root.Name}");

            if (root.Banned)
                embedResult.AddField("Player BANNED", "The player has been banned.");

            embedResult.AddField("Player Level:", root.Level);
            embedResult.AddField("Player Elo:", root.Discord.Truemmr);
            embedResult.AddField("Player KD:", root.Discord.Kd);
            embedResult.AddField("Player Wins:", root.Discord.Wins);
            embedResult.AddField("Player Losses:", root.Discord.Losses);
            embedResult.AddField("Player WL Percentage:", root.Discord.Wl);
            embedResult.WithThumbnailUrl("https://cdn.tab.one/r6/images/ranks/?rank=" + root.Discord.Rank + "&champ=" + root.Discord.Champ);
            embedResult.WithFooter(root.Name, root.Avatar.ToString());
            embedResult.WithColor(new Color(color.R, color.B, color.G));

            await channel.SendMessageAsync("", false, embedResult.Build());
        }

        [Command("verify")]
        public async Task verify(string playerid)
        {
            var config = ConfigClass.GetOrCreateConfig(Context.Guild.Id, Context.Guild.OwnerId);
            var embedMessage = new EmbedBuilder();
            SocketGuildUser user = Context.Message.Author as SocketGuildUser;
            await Context.Message.DeleteAsync();

            UsersBase usercheck = UsersClass.GetUser(playerid);
            if (usercheck != null)
            {
                var embedVerified = new EmbedBuilder();
                embedVerified.WithTitle("R6 Competitive | Error!");
                embedVerified.AddField("Verification Error", "The player you have requested is already linked to another account.");
                embedVerified.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

                var dm2 = await Context.User.GetOrCreateDMChannelAsync();
                await dm2.SendMessageAsync("", false, embedVerified.Build());
                return;
            }

            UsersBase usercheck2 = UsersClass.GetUserID(user.Id);
            if (usercheck2 != null)
            {
                var embedVerified = new EmbedBuilder();
                embedVerified.WithTitle("R6 Competitive | Error!");
                embedVerified.AddField("Verification Error", "You are already verified to another account.");
                embedVerified.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

                var dm2 = await Context.User.GetOrCreateDMChannelAsync();
                await dm2.SendMessageAsync("", false, embedVerified.Build());
                return;
            }

            PlayerInfo root;
            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync("https://r6.apitab.com/player/" + playerid + "&u=unix");
                root = JsonConvert.DeserializeObject<PlayerInfo>(json);
            }

            if (root.Social.UplayUser == null)
            {
                var embedVerified = new EmbedBuilder();
                embedVerified.WithTitle("R6 Competitive | Error!");
                embedVerified.AddField("Verification Error", "This account you are trying to verify does not have a uplay account linked.");
                embedVerified.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

                var dm2 = await Context.User.GetOrCreateDMChannelAsync();
                await dm2.SendMessageAsync("", false, embedVerified.Build());
                return;
            }

            if (root.Social.DiscordId == null)
            {
                var embedVerified = new EmbedBuilder();
                embedVerified.WithTitle("R6 Competitive | Error!");
                embedVerified.AddField("Verification Error", "This account you are trying to verify does not have a discord account linked.");
                embedVerified.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

                var dm2 = await Context.User.GetOrCreateDMChannelAsync();
                await dm2.SendMessageAsync("", false, embedVerified.Build());
                return;
            }

            if (Convert.ToUInt64(root.Social.DiscordId) != user.Id)
            {
                var embedVerified = new EmbedBuilder();
                embedVerified.WithTitle("R6 Competitive | Error!");
                embedVerified.AddField("Verification Error", "This discord account dosen't match the one linked to the account.");
                embedVerified.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

                var dm2 = await Context.User.GetOrCreateDMChannelAsync();
                await dm2.SendMessageAsync("", false, embedVerified.Build());
                return;
            }

            if (Helpers.UserHasRole(Context, 692966707131580448))
            {
                var embedVerified = new EmbedBuilder();
                embedVerified.WithTitle("R6 Competitive | Error!");
                embedVerified.AddField("Verified", "You seem to already be verified.");
                embedVerified.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

                var dm2 = await Context.User.GetOrCreateDMChannelAsync();
                await dm2.SendMessageAsync("", false, embedVerified.Build());
                return;
            }

            string role = root.Ranked.Rankname;
            var rankToAdd = Context.Guild.Roles.FirstOrDefault(x => x.Name == role);
            if (rankToAdd == null)
            {
                var newrole = Context.Guild.Roles.FirstOrDefault(r => r.Name == "Unranked/Low Rank");
                await user.AddRoleAsync(newrole);
            } else
            {
                await user.AddRoleAsync(rankToAdd);
            }

            var removedrole = Context.Guild.Roles.FirstOrDefault(r => r.Name == "UMAD Unverified");
            var addedrole = Context.Guild.Roles.FirstOrDefault(r => r.Name == "UMAD Verified");
            await user.RemoveRoleAsync(removedrole);
            await user.AddRoleAsync(addedrole);

            UsersClass.CreateUser(user.Id, playerid);
            await user.ModifyAsync(u => { u.Nickname = root.Player.PName; });
        }

        [RequireOwner]
        [Command("ownerverify")]
        public async Task ownerverify(SocketUser player, string playerid)
        {
            var config = ConfigClass.GetOrCreateConfig(Context.Guild.Id, Context.Guild.OwnerId);
            var embedMessage = new EmbedBuilder();
            await Context.Message.DeleteAsync();

            UsersBase usercheck = UsersClass.GetUser(playerid);
            if (usercheck != null)
            {
                var embedVerified = new EmbedBuilder();
                embedVerified.WithTitle("R6 Competitive | Error!");
                embedVerified.AddField("Verification Error", "The player you have requested is already linked to another account.");
                embedVerified.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

                var dm2 = await Context.User.GetOrCreateDMChannelAsync();
                await dm2.SendMessageAsync("", false, embedVerified.Build());
                return;
            }

            UsersBase usercheck2 = UsersClass.GetUserID(player.Id);
            if (usercheck2 != null)
            {
                var embedVerified = new EmbedBuilder();
                embedVerified.WithTitle("R6 Competitive | Error!");
                embedVerified.AddField("Verification Error", "They are already verified to another account.");
                embedVerified.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));

                var dm2 = await Context.User.GetOrCreateDMChannelAsync();
                await dm2.SendMessageAsync("", false, embedVerified.Build());
                return;
            }

            PlayerInfo root;
            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync("https://r6.apitab.com/player/" + playerid + "&u=unix");
                root = JsonConvert.DeserializeObject<PlayerInfo>(json);
            }

            string role = root.Ranked.Rankname;
            var rankToAdd = Context.Guild.Roles.FirstOrDefault(x => x.Name == role);
            SocketGuildUser user = player as SocketGuildUser;
            if (rankToAdd == null)
            {
                var newrole = Context.Guild.Roles.FirstOrDefault(r => r.Name == "Unranked/Low Rank");
                await user.AddRoleAsync(newrole);
            }
            else
            {
                await user.AddRoleAsync(rankToAdd);
            }

            var removedrole = Context.Guild.Roles.FirstOrDefault(r => r.Name == "UMAD Unverified");
            var addedrole = Context.Guild.Roles.FirstOrDefault(r => r.Name == "UMAD Verified");
            await user.RemoveRoleAsync(removedrole);
            await user.AddRoleAsync(addedrole);

            UsersClass.CreateUser(user.Id, playerid);
            await user.ModifyAsync(u => { u.Nickname = root.Player.PName; });
        }
    }
}
