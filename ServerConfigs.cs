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
                EmbedColour3 = 251
            };

            Config.Add(newconfig);
            SaveConfig();
            return newconfig;
        }
    }

    public class UsersBase
    {
        public ulong DiscordID { get; set; }
        public string UplayID { get; set; }
    }

    public static class UsersClass
    {
        private static readonly List<UsersBase> Users = new List<UsersBase>();
        private static readonly string filePath = "Resources/Users.json";

        static UsersClass()
        {
            try
            {
                var jsonText = File.ReadAllText(filePath);
                Users = JsonConvert.DeserializeObject<List<UsersBase>>(jsonText);
            }
            catch (Exception)
            {
                SaveConfig();
            }
        }

        public static UsersBase GetUser(string id)
        {
            var result = Users.FirstOrDefault(x => x.UplayID == id);
            return result;
        }

        public static UsersBase GetUserID(ulong id)
        {
            var result = Users.FirstOrDefault(x => x.DiscordID == id);
            return result;
        }

        public static List<UsersBase> GetAllUsers()
        {
            return Users;
        }

        public static void SaveConfig()
        {
            var json = JsonConvert.SerializeObject(Users, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static UsersBase CreateUser(ulong discord, string uplay)
        {
            var newuser = new UsersBase
            {
                DiscordID = discord,
                UplayID = uplay
            };

            Users.Add(newuser);
            SaveConfig();
            return newuser;
        }
    }
}
