using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R6DiscordBot.Modules
{
    public class Result
    {
        public string p_id { get; set; }
        public string p_name { get; set; }
        public string p_level { get; set; }
        public string p_platform { get; set; }
        public string p_user { get; set; }
        public string p_currentmmr { get; set; }
        public string p_currentrank { get; set; }
        public string kd { get; set; }
    }

    public class RootPlayerSearch
    {
        public List<Result> results { get; set; }
        public int totalresults { get; set; }
    }
}
