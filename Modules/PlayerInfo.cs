using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace R6DiscordBot.Modules
{
    public partial class PlayerInfo
    {
        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("found")]
        public bool Found { get; set; }

        [JsonProperty("player")]
        public Player Player { get; set; }

        [JsonProperty("custom")]
        public Custom Custom { get; set; }

        [JsonProperty("refresh")]
        public Refresh Refresh { get; set; }

        [JsonProperty("social")]
        public Social Social { get; set; }

        [JsonProperty("stats")]
        public Stats Stats { get; set; }

        [JsonProperty("ranked")]
        public Ranked Ranked { get; set; }

        [JsonProperty("operators_old")]
        public OperatorsOld OperatorsOld { get; set; }
    }

    public partial class Custom
    {
        [JsonProperty("customurl")]
        public bool Customurl { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("visitors")]
        public long Visitors { get; set; }

        [JsonProperty("banned")]
        public bool Banned { get; set; }
    }

    public partial class OperatorsOld
    {
        [JsonProperty("wins")]
        public Dictionary<string, long> Wins { get; set; }

        [JsonProperty("losses")]
        public Dictionary<string, long> Losses { get; set; }

        [JsonProperty("kills")]
        public Dictionary<string, long> Kills { get; set; }

        [JsonProperty("deaths")]
        public Dictionary<string, long> Deaths { get; set; }

        [JsonProperty("timeplayed")]
        public Dictionary<string, long> Timeplayed { get; set; }
    }

    public partial class Player
    {
        [JsonProperty("p_id")]
        public Guid PId { get; set; }

        [JsonProperty("p_user")]
        public Guid PUser { get; set; }

        [JsonProperty("p_name")]
        public string PName { get; set; }

        [JsonProperty("p_platform")]
        public string PPlatform { get; set; }
    }

    public partial class Ranked
    {
        [JsonProperty("AS_kills")]
        public long AsKills { get; set; }

        [JsonProperty("AS_deaths")]
        public long AsDeaths { get; set; }

        [JsonProperty("AS_wins")]
        public long AsWins { get; set; }

        [JsonProperty("AS_losses")]
        public long AsLosses { get; set; }

        [JsonProperty("AS_abandons")]
        public long AsAbandons { get; set; }

        [JsonProperty("AS_mmr")]
        public long AsMmr { get; set; }

        [JsonProperty("AS_maxmmr")]
        public long AsMaxmmr { get; set; }

        [JsonProperty("AS_champ")]
        public long AsChamp { get; set; }

        [JsonProperty("AS_mmrchange")]
        public long AsMmrchange { get; set; }

        [JsonProperty("AS_actualmmr")]
        public long AsActualmmr { get; set; }

        [JsonProperty("AS_matches")]
        public long AsMatches { get; set; }

        [JsonProperty("AS_wl")]
        public string AsWl { get; set; }

        [JsonProperty("AS_kd")]
        public string AsKd { get; set; }

        [JsonProperty("AS_rank")]
        public long AsRank { get; set; }

        [JsonProperty("AS_rankname")]
        public string AsRankname { get; set; }

        [JsonProperty("AS_maxrank")]
        public long AsMaxrank { get; set; }

        [JsonProperty("AS_maxrankname")]
        public string AsMaxrankname { get; set; }

        [JsonProperty("AS_killpermatch")]
        public long AsKillpermatch { get; set; }

        [JsonProperty("AS_deathspermatch")]
        public long AsDeathspermatch { get; set; }

        [JsonProperty("allkills")]
        public long Allkills { get; set; }

        [JsonProperty("alldeaths")]
        public long Alldeaths { get; set; }

        [JsonProperty("allwins")]
        public long Allwins { get; set; }

        [JsonProperty("alllosses")]
        public long Alllosses { get; set; }

        [JsonProperty("allabandons")]
        public long Allabandons { get; set; }

        [JsonProperty("EU_kills")]
        public long EuKills { get; set; }

        [JsonProperty("EU_deaths")]
        public long EuDeaths { get; set; }

        [JsonProperty("EU_wins")]
        public long EuWins { get; set; }

        [JsonProperty("EU_losses")]
        public long EuLosses { get; set; }

        [JsonProperty("EU_abandons")]
        public long EuAbandons { get; set; }

        [JsonProperty("EU_mmr")]
        public long EuMmr { get; set; }

        [JsonProperty("EU_maxmmr")]
        public long EuMaxmmr { get; set; }

        [JsonProperty("EU_champ")]
        public long EuChamp { get; set; }

        [JsonProperty("EU_mmrchange")]
        public long EuMmrchange { get; set; }

        [JsonProperty("EU_actualmmr")]
        public long EuActualmmr { get; set; }

        [JsonProperty("EU_matches")]
        public long EuMatches { get; set; }

        [JsonProperty("EU_wl")]
        public string EuWl { get; set; }

        [JsonProperty("EU_kd")]
        public string EuKd { get; set; }

        [JsonProperty("EU_rank")]
        public long EuRank { get; set; }

        [JsonProperty("EU_rankname")]
        public string EuRankname { get; set; }

        [JsonProperty("EU_maxrank")]
        public long EuMaxrank { get; set; }

        [JsonProperty("EU_maxrankname")]
        public string EuMaxrankname { get; set; }

        [JsonProperty("EU_killpermatch")]
        public long EuKillpermatch { get; set; }

        [JsonProperty("EU_deathspermatch")]
        public long EuDeathspermatch { get; set; }

        [JsonProperty("NA_kills")]
        public long NaKills { get; set; }

        [JsonProperty("NA_deaths")]
        public long NaDeaths { get; set; }

        [JsonProperty("NA_wins")]
        public long NaWins { get; set; }

        [JsonProperty("NA_losses")]
        public long NaLosses { get; set; }

        [JsonProperty("NA_abandons")]
        public long NaAbandons { get; set; }

        [JsonProperty("NA_mmr")]
        public long NaMmr { get; set; }

        [JsonProperty("NA_maxmmr")]
        public long NaMaxmmr { get; set; }

        [JsonProperty("NA_champ")]
        public long NaChamp { get; set; }

        [JsonProperty("NA_mmrchange")]
        public long NaMmrchange { get; set; }

        [JsonProperty("NA_actualmmr")]
        public long NaActualmmr { get; set; }

        [JsonProperty("NA_matches")]
        public long NaMatches { get; set; }

        [JsonProperty("NA_wl")]
        public string NaWl { get; set; }

        [JsonProperty("NA_kd")]
        public string NaKd { get; set; }

        [JsonProperty("NA_rank")]
        public long NaRank { get; set; }

        [JsonProperty("NA_rankname")]
        public string NaRankname { get; set; }

        [JsonProperty("NA_maxrank")]
        public long NaMaxrank { get; set; }

        [JsonProperty("NA_maxrankname")]
        public string NaMaxrankname { get; set; }

        [JsonProperty("NA_killpermatch")]
        public long NaKillpermatch { get; set; }

        [JsonProperty("NA_deathspermatch")]
        public long NaDeathspermatch { get; set; }

        [JsonProperty("mmr")]
        public long Mmr { get; set; }

        [JsonProperty("maxmmr")]
        public long Maxmmr { get; set; }

        [JsonProperty("kd")]
        public double Kd { get; set; }

        [JsonProperty("rank")]
        public long Rank { get; set; }

        [JsonProperty("rankname")]
        public string Rankname { get; set; }

        [JsonProperty("maxrank")]
        public long Maxrank { get; set; }

        [JsonProperty("maxrankname")]
        public string Maxrankname { get; set; }

        [JsonProperty("champ")]
        public long Champ { get; set; }

        [JsonProperty("topregion")]
        public string Topregion { get; set; }

        [JsonProperty("actualmmr")]
        public long Actualmmr { get; set; }

        [JsonProperty("allmatches")]
        public long Allmatches { get; set; }

        [JsonProperty("allkd")]
        public string Allkd { get; set; }

        [JsonProperty("allwl")]
        public string Allwl { get; set; }

        [JsonProperty("killpermatch")]
        public long Killpermatch { get; set; }

        [JsonProperty("deathspermatch")]
        public long Deathspermatch { get; set; }
    }

    public partial class Refresh
    {
        [JsonProperty("queued")]
        public bool Queued { get; set; }

        [JsonProperty("possible")]
        public bool Possible { get; set; }

        [JsonProperty("qtime")]
        public long Qtime { get; set; }

        [JsonProperty("utime")]
        public long Utime { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }
    }

    public partial class Social
    {
        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("utime")]
        public long Utime { get; set; }

        [JsonProperty("userid")]
        public long Userid { get; set; }

        [JsonIgnore]
        public Guid uplay_user { get; set; }

        [JsonProperty("uplay_user")]
        [JsonConverter(typeof(NullToDefaultConverter<Guid>))]
        public Guid? UplayUser { get { return uplay_user == Guid.Empty ? null : (Guid?)uplay_user; } set { uplay_user = (value == null ? Guid.Empty : value.Value); } }

        [JsonProperty("uplay_name")]
        public string UplayName { get; set; }

        [JsonProperty("discord")]
        public string Discord { get; set; }

        [JsonProperty("discord_id")]
        public string DiscordId { get; set; }

        [JsonProperty("discord_user")]
        public string DiscordUser { get; set; }

        [JsonProperty("esl")]
        public string Esl { get; set; }

        [JsonProperty("bio")]
        public string Bio { get; set; }

        [JsonProperty("background")]
        public Uri Background { get; set; }

        [JsonProperty("embed")]
        public Uri Embed { get; set; }

        [JsonProperty("aliases_hide")]
        public long AliasesHide { get; set; }

        [JsonProperty("twitch_display")]
        public long TwitchDisplay { get; set; }

        [JsonProperty("premium")]
        public Premium Premium { get; set; }

        [JsonProperty("is_premium")]
        public bool IsPremium { get; set; }
    }

    public partial class Premium
    {
        [JsonProperty("tabwire")]
        public bool Tabwire { get; set; }

        [JsonProperty("discord")]
        public bool Discord { get; set; }

        [JsonProperty("twitch")]
        public bool Twitch { get; set; }
    }

    public partial class Stats
    {
        [JsonProperty("level")]
        public long Level { get; set; }

        [JsonProperty("casualpvp_kills")]
        public long CasualpvpKills { get; set; }

        [JsonProperty("casualpvp_death")]
        public long CasualpvpDeath { get; set; }

        [JsonProperty("casualpvp_matchwon")]
        public long CasualpvpMatchwon { get; set; }

        [JsonProperty("casualpvp_matchlost")]
        public long CasualpvpMatchlost { get; set; }

        [JsonProperty("casualpvp_timeplayed")]
        public long CasualpvpTimeplayed { get; set; }

        [JsonProperty("casualpvp_hoursplayed")]
        public long CasualpvpHoursplayed { get; set; }

        [JsonProperty("casualpvp_matches")]
        public long CasualpvpMatches { get; set; }

        [JsonProperty("casualpvp_kd")]
        public string CasualpvpKd { get; set; }

        [JsonProperty("casualpvp_wl")]
        public string CasualpvpWl { get; set; }

        [JsonProperty("rankedpvp_kills")]
        public long RankedpvpKills { get; set; }

        [JsonProperty("rankedpvp_death")]
        public long RankedpvpDeath { get; set; }

        [JsonProperty("rankedpvp_matchwon")]
        public long RankedpvpMatchwon { get; set; }

        [JsonProperty("rankedpvp_matchlost")]
        public long RankedpvpMatchlost { get; set; }

        [JsonProperty("rankedpvp_timeplayed")]
        public long RankedpvpTimeplayed { get; set; }

        [JsonProperty("rankedpvp_hoursplayed")]
        public long RankedpvpHoursplayed { get; set; }

        [JsonProperty("rankedpvp_matches")]
        public long RankedpvpMatches { get; set; }

        [JsonProperty("rankedpvp_kd")]
        public string RankedpvpKd { get; set; }

        [JsonProperty("rankedpvp_wl")]
        public string RankedpvpWl { get; set; }

        [JsonProperty("generalpvp_headshot")]
        public long GeneralpvpHeadshot { get; set; }

        [JsonProperty("generalpvp_kills")]
        public long GeneralpvpKills { get; set; }

        [JsonProperty("generalpvp_timeplayed")]
        public long GeneralpvpTimeplayed { get; set; }

        [JsonProperty("generalpve_kills")]
        public long GeneralpveKills { get; set; }

        [JsonProperty("generalpve_death")]
        public long GeneralpveDeath { get; set; }

        [JsonProperty("generalpve_matchwon")]
        public long GeneralpveMatchwon { get; set; }

        [JsonProperty("generalpve_matchlost")]
        public long GeneralpveMatchlost { get; set; }

        [JsonProperty("generalpve_headshot")]
        public long GeneralpveHeadshot { get; set; }

        [JsonProperty("generalpve_timeplayed")]
        public long GeneralpveTimeplayed { get; set; }

        [JsonProperty("generalpvp_hoursplayed")]
        public long GeneralpvpHoursplayed { get; set; }

        [JsonProperty("generalpvp_death")]
        public long GeneralpvpDeath { get; set; }

        [JsonProperty("generalpvp_kd")]
        public string GeneralpvpKd { get; set; }

        [JsonProperty("generalpvp_matchwon")]
        public long GeneralpvpMatchwon { get; set; }

        [JsonProperty("generalpvp_matchlost")]
        public long GeneralpvpMatchlost { get; set; }

        [JsonProperty("generalpvp_matches")]
        public long GeneralpvpMatches { get; set; }

        [JsonProperty("generalpvp_wl")]
        public string GeneralpvpWl { get; set; }

        [JsonProperty("generalpvp_hsrate")]
        public string GeneralpvpHsrate { get; set; }

        [JsonProperty("generalpvp_killassists")]
        public long GeneralpvpKillassists { get; set; }

        [JsonProperty("generalpvp_meleekills")]
        public long GeneralpvpMeleekills { get; set; }

        [JsonProperty("generalpvp_revive")]
        public long GeneralpvpRevive { get; set; }

        [JsonProperty("generalpvp_penetrationkills")]
        public long GeneralpvpPenetrationkills { get; set; }

        [JsonProperty("generalpve_hoursplayed")]
        public long GeneralpveHoursplayed { get; set; }

        [JsonProperty("generalpve_matches")]
        public long GeneralpveMatches { get; set; }

        [JsonProperty("generalpve_kd")]
        public string GeneralpveKd { get; set; }

        [JsonProperty("generalpve_wl")]
        public string GeneralpveWl { get; set; }

        [JsonProperty("generalpve_hsrate")]
        public string GeneralpveHsrate { get; set; }

        [JsonProperty("plantbombpvp_matchwon")]
        public long PlantbombpvpMatchwon { get; set; }

        [JsonProperty("plantbombpvp_matchlost")]
        public long PlantbombpvpMatchlost { get; set; }

        [JsonProperty("secureareapvp_matchwon")]
        public long SecureareapvpMatchwon { get; set; }

        [JsonProperty("secureareapvp_matchlost")]
        public long SecureareapvpMatchlost { get; set; }

        [JsonProperty("rescuehostagepvp_matchwon")]
        public long RescuehostagepvpMatchwon { get; set; }

        [JsonProperty("rescuehostagepvp_matchlost")]
        public long RescuehostagepvpMatchlost { get; set; }

        [JsonProperty("plantbombpvp_matches")]
        public long PlantbombpvpMatches { get; set; }

        [JsonProperty("plantbombpvp_wl")]
        public string PlantbombpvpWl { get; set; }

        [JsonProperty("secureareapvp_matches")]
        public long SecureareapvpMatches { get; set; }

        [JsonProperty("secureareapvp_wl")]
        public string SecureareapvpWl { get; set; }

        [JsonProperty("rescuehostagepvp_matches")]
        public long RescuehostagepvpMatches { get; set; }

        [JsonProperty("rescuehostagepvp_wl")]
        public string RescuehostagepvpWl { get; set; }

        [JsonProperty("tabmmr")]
        public long Tabmmr { get; set; }

        [JsonProperty("tabrank")]
        public long Tabrank { get; set; }

        [JsonProperty("tabrankname")]
        public string Tabrankname { get; set; }
    }

    public partial struct Maxmmr
    {
        public long? Integer;
        public string String;

        public static implicit operator Maxmmr(long Integer) => new Maxmmr { Integer = Integer };
        public static implicit operator Maxmmr(string String) => new Maxmmr { String = String };
    }

    public class NullToDefaultConverter<T> : JsonConverter where T : struct
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            if (token == null || token.Type == JTokenType.Null)
                return default(T);
            return token.ToObject(objectType);
        }

        // Return false instead if you don't want default values to be written as null
        public override bool CanWrite { get { return true; } }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (EqualityComparer<T>.Default.Equals((T)value, default(T)))
                writer.WriteNull();
            else
                writer.WriteValue(value);
        }
    }

    public partial class PlayerDiscord
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("user")]
        public Guid User { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("avatar")]
        public Uri Avatar { get; set; }

        [JsonProperty("banned")]
        public bool Banned { get; set; }

        [JsonProperty("level")]
        public long Level { get; set; }

        [JsonProperty("packchance")]
        public string Packchance { get; set; }

        [JsonProperty("found")]
        public bool Found { get; set; }

        [JsonProperty("discord")]
        public Discord Discord { get; set; }
    }

    public partial class Discord
    {
        [JsonProperty("mmrchange")]
        public string Mmrchange { get; set; }

        [JsonProperty("mmr")]
        public long Mmr { get; set; }

        [JsonProperty("truemmr")]
        public long Truemmr { get; set; }

        [JsonProperty("maxmmr")]
        public long Maxmmr { get; set; }

        [JsonProperty("rank")]
        public long Rank { get; set; }

        [JsonProperty("rankname")]
        public string Rankname { get; set; }

        [JsonProperty("champ")]
        public long Champ { get; set; }

        [JsonProperty("kd")]
        public string Kd { get; set; }

        [JsonProperty("kills")]
        public long Kills { get; set; }

        [JsonProperty("deaths")]
        public long Deaths { get; set; }

        [JsonProperty("wins")]
        public long Wins { get; set; }

        [JsonProperty("losses")]
        public long Losses { get; set; }

        [JsonProperty("wl")]
        public string Wl { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }
    }
}
