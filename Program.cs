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

namespace R6DiscordBot
{
    class Program
    {
        public static DiscordSocketClient _client;
        public static CommandHandler _handler;

        static void Main(string[] args)
        {
            NotifyIcon tray = new NotifyIcon();
            Console.Title = "R6 Competitive Discord Bot";
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Green;
            tray.Icon = new Icon("Resources\\R6Logo.ico");
            tray.Text = "R6 Competitive Discord Bot";
            tray.Visible = true;

            new Program().StartASync().GetAwaiter().GetResult();
        }

        public async Task StartASync()
        {
            string token = "NjMyNzE3MTg5ODg3MTY0NDE2.XaKCpQ.r9IbHhaOn82zVLBVcOqJElc8hXE";
            if (token == "" || token == null)
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
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await _client.SetStatusAsync(UserStatus.DoNotDisturb);
            await _client.SetGameAsync("R6 Competitive Bot by Travis Butts");
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
