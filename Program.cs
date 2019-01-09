using Discord.WebSocket;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ALTDiscordBot
{
    class Program
    {
        public static DiscordSocketClient _client;
        public static CommandHandler _handler;

        static void Main(string[] args)
        {
            NotifyIcon tray = new NotifyIcon();
            Console.Title = "ALT Bot";
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Green;
            tray.Icon = new Icon("Resources\\ALTLogo.ico");
            tray.Text = "ALT Discord Bot";
            tray.Visible = true;

            new Program().StartASync().GetAwaiter().GetResult();
        }
        
        public async Task StartASync()
        {
            if (Config.bot.token == "" || Config.bot.token == null)
            {
                Console.WriteLine("Token Invalid");
                Console.ReadLine();
                return;
            }
                

            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });
            _client.Log += Log;
            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();
            await _client.SetStatusAsync(UserStatus.DoNotDisturb);
            await _client.SetGameAsync("Travi's Computer", $"https://twitch.tv/TravisButtsOfficial",StreamType.NotStreaming);
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
            await Task.Delay(-1);
        }

        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
        }
    }
}
