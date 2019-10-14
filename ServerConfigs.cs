using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using Discord.WebSocket;

namespace R6DiscordBot
{
    public class ConfigBase
    {
        public ulong ServerId { get; set; }
        public ulong OwnerId { get; set; }
        public string CommandPrefix { get; set; }
        public int EmbedColour1 { get; set; }
        public int EmbedColour2 { get; set; }
        public int EmbedColour3 { get; set; }
        public ulong TenManChannelID { get; set; }
        public ulong TenManMessageID { get; set; }
        public string TenManJoinQueEmote { get; set; }
    }

    public static class ConfigClass
    {
        private static readonly List<ConfigBase> Config = new List<ConfigBase>();
        private static readonly string filePath = "Resources/Configs.json";

        static ConfigClass()
        {
            try
            {
                var jsonText = File.ReadAllText(filePath);
                Config = JsonConvert.DeserializeObject<List<ConfigBase>>(jsonText);
            }
            catch (Exception)
            {
                SaveConfig();
            }
        }

        public static ConfigBase GetConfig(ulong id)
        {
            var result = Config.FirstOrDefault(x => x.ServerId == id);

            var config = result;
            return config;
        }

        public static ConfigBase GetOrCreateConfig(ulong id, ulong ownerid)
        {
            var result = Config.FirstOrDefault(x => x.ServerId == id);

            var config = result ?? CreateConfig(id,ownerid);
            return config;
        }

        public static void SaveConfig()
        {
            var json = JsonConvert.SerializeObject(Config, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static ConfigBase CreateConfig(ulong id, ulong ownerid)
        {
            var newconfig = new ConfigBase
            {
                ServerId = id,
                OwnerId = ownerid,
                CommandPrefix = "!",
                EmbedColour1 = 112,
                EmbedColour2 = 0,
                EmbedColour3 = 251,
                TenManChannelID = 0,
                TenManMessageID = 0,
                TenManJoinQueEmote = ""
            };

            Config.Add(newconfig);
            SaveConfig();
            return newconfig;
        }
    }
}
