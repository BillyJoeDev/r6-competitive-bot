using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;

namespace ALTDiscordBot
{
    public class ConfigBase
    {
        public ulong ServerId { get; set; }
        public ulong OwnerId { get; set; }
        public string CommandPrefix { get; set; }
        public ulong WelcomeChannel { get; set; }
        public ulong AnnouncementsChannel { get; set; }
        public ulong LoggingChannel { get; set; }
        public ulong DevShowcaseChannel { get; set; }
        public string WelcomeMessage { get; set; }
        public string LeavingMessage { get; set; }
        public int WelcomeColour1 { get; set; }
        public int WelcomeColour2 { get; set; }
        public int WelcomeColour3 { get; set; }
        public int EmbedColour1 { get; set; }
        public int EmbedColour2 { get; set; }
        public int EmbedColour3 { get; set; }
        public bool Antilink { get; set; }
        public bool Verified { get; set; }
        public ulong ModRole { get; set; }
        public ulong AdminRole { get; set; }
        public List<ulong> AntilinkIgnoredChannels { get; set; }
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

        public static ConfigBase GetOrCreateConfig(ulong id)
        {
            var result = Config.FirstOrDefault(x => x.ServerId == id);

            var config = result ?? CreateConfig(id);
            return config;
        }

        public static void SaveConfig()
        {
            var json = JsonConvert.SerializeObject(Config, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static ConfigBase CreateConfig(ulong id)
        {
            var newconfig = new ConfigBase
            {
                ServerId = id,
                OwnerId = 0,
                CommandPrefix = "!",
                WelcomeChannel = 0,
                AnnouncementsChannel = 0,
                LoggingChannel = 0,
                DevShowcaseChannel = 0,
                WelcomeMessage = string.Empty,
                LeavingMessage = string.Empty,
                WelcomeColour1 = 112,
                WelcomeColour2 = 0,
                WelcomeColour3 = 251,
                EmbedColour1 = 112,
                EmbedColour2 = 0,
                EmbedColour3 = 251,
                Antilink = false,
                Verified = false
            };
            Config.Add(newconfig);
            SaveConfig();
            return newconfig;
        }
    }
}
