using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading;

namespace R6DiscordBot
{
    class Utilities
    {
        private static Dictionary<string, string> alerts;

        static Utilities()
        {
            string json = File.ReadAllText("SystemLang/alerts.json");
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            alerts = data.ToObject<Dictionary<string, string>>();
        }


        public static string GetLocaleMsg(string key)
        {
            if (alerts.ContainsKey(key)) return alerts[key];
            return "";
        }

        public static string GetFormattedLocaleMsg(string key, params object[] parameter)
        {
            return alerts.ContainsKey(key) ? string.Format(alerts[key], parameter) : "";
        }

        public static string GetFormattedLocaleMsg(string key, object parameter)
        {
            return alerts.ContainsKey(key) ? string.Format(alerts[key], parameter) : "";
        }

        public static string InlineMsg(string msg) => $"`{msg}`";

        public static string CodeBlock(string msg) => $"```{msg}```";

        public static string Italic(string msg) => $"*{msg}*";

        public static string Bold(string msg) => $"**{msg}**";

        public static string BoldItalic(string msg) => $"***{msg}***";

        public static string Underline(string msg) => $"__{msg}__";

        public static string Strikethrough(string msg) => $"~~{msg}~~";
    }
}
