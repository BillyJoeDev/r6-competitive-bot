using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;
using R6DiscordBot.Modules;

namespace R6DiscordBot
{
    class CommandHandler
    {
        public static DiscordSocketClient _client;
        public static CommandService _service;
        private Random rand = new Random();
        private int reactions = 0;
        List<IUser> players = new List<IUser>();
        StringBuilder playersInQue = new StringBuilder();

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            await _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += MessageReceived;
            _client.ReactionAdded += OnReactionAdded;
            _client.ReactionRemoved += OnReactionRemoved;
        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
        {
            var chnl = channel as SocketGuildChannel;
            var config = ConfigClass.GetOrCreateConfig(chnl.Guild.Id, chnl.Guild.OwnerId);

            if (reaction.MessageId == config.TenManMessageID)
            {
                if (reaction.Emote.Name == config.TenManJoinQueEmote)
                {
                    reactions++;
                    var message = await channel.GetMessageAsync(config.TenManMessageID) as IUserMessage;

                    if (!reaction.User.Value.IsBot)
                    {
                        players.Add(reaction.User.Value);
                        playersInQue.Append(reaction.User.Value.Mention);
                    }

                    string msg = "**NONE**";
                    if (players.Count > 0)
                        msg = playersInQue.ToString();

                    await message.ModifyAsync(x =>
                    {
                        x.Content = "";
                        x.Embed = new EmbedBuilder()
                            .WithTitle("**R6 Bot | 10 Mans Queue**")
                            .WithDescription(string.Format("**Currently in Queue:** {0}", msg))
                            .AddField("JOIN/EXIT QUEUE:", config.TenManJoinQueEmote)
                            .WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3))
                            .Build();
                    });

                    if (reactions == 11)
                    {
                        Random rnd = new Random();
                        players = players.Select(i => new {value = i, rank = rnd.Next(players.Count())}).OrderBy(n => n.rank).Select(n => n.value).ToList();

                        await message.RemoveAllReactionsAsync();

                        string mode = "attackers";
                        StringBuilder attackerteam = new StringBuilder();
                        StringBuilder defenderteam = new StringBuilder();

                        foreach (IUser user in players)
                        {
                            if (user.IsBot) continue;

                            if (mode == "attackers") {
                                attackerteam.Append(user.Mention + "\n");
                                mode = "defenders";
                            } else if (mode == "defenders")
                            {
                                defenderteam.Append(user.Mention + "\n");
                                mode = "attackers";
                            }
                        }

                        var embed = new EmbedBuilder();
                        embed.WithTitle("**R6 Bot | 10 Man Teams**");
                        embed.AddField("Attacking Team:", attackerteam);
                        embed.AddField("Defending Team:", defenderteam);
                        embed.WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3));  
                        await channel.SendMessageAsync("", false, embed);

                        reactions = 0;
                        mode = "attackers";
                        players.Clear();

                        await channel.SendMessageAsync((config.CommandPrefix + "setuptenmans"), false);
                    }
                }
            }
        }

        private async Task OnReactionRemoved(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
        {
            var chnl = channel as SocketGuildChannel;
            var config = ConfigClass.GetOrCreateConfig(chnl.Guild.Id, chnl.Guild.OwnerId);
            if (reaction.MessageId == config.TenManMessageID)
            {
                if (reaction.Emote.Name == config.TenManJoinQueEmote)
                {
                    var message = await channel.GetMessageAsync(config.TenManMessageID) as IUserMessage;

                    reactions--;
                    if (!reaction.User.Value.IsBot)
                    {
                        players.Remove(reaction.User.Value);

                        int index = playersInQue.ToString().IndexOf(reaction.User.Value.Mention);
                        playersInQue.Remove(index, reaction.User.Value.Mention.Length);
                    }

                    string msg = "**NONE**";
                    if (players.Count > 0)
                        msg = playersInQue.ToString();

                    await message.ModifyAsync(x =>
                    {
                        x.Content = "";
                        x.Embed = new EmbedBuilder()
                            .WithTitle("**R6 Bot | 10 Mans Queue**")
                            .WithDescription(string.Format("**Currently in Queue:** {0}", msg))
                            .AddField("JOIN/EXIT QUEUE:", config.TenManJoinQueEmote)
                            .WithColor(new Color(config.EmbedColour1, config.EmbedColour2, config.EmbedColour3))
                            .Build();
                    });
                }
            }
        }

        private async Task MessageReceived(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null) return;
            
            var context = new SocketCommandContext(_client, msg);
            var config = ConfigClass.GetConfig(context.Guild.Id) ?? ConfigClass.CreateConfig(context.Guild.Id, context.Guild.OwnerId);
            var prefix = config.CommandPrefix ?? "!";

            if (config.EmbedColour1 == 0 && config.EmbedColour2 == 0 && config.EmbedColour3 == 0)
            {
                config.EmbedColour1 = 112;
                config.EmbedColour2 = 0;
                config.EmbedColour3 = 251;
                ConfigClass.SaveConfig();
            }

            int argPos = 0;
            if (msg.HasStringPrefix(prefix, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _service.ExecuteAsync(context, argPos);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }
    }
}
