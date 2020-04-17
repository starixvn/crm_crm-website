namespace Fanex.CRM.BackgroundJob.Models
{
    public class MatchInfo
    {
        public int MatchId { get; set; }

        public int SportType { get; set; }

        public string SportNameEN { get; set; }
        public string SportNameCS { get; set; }

        public string KickOffTime { get; set; }

        public bool IsTest { get; set; }

        public string LeagueNameEN { get; set; }
        public string LeagueNameCS { get; set; }

        public string HomeNameEN { get; set; }
        public string HomeNameCS { get; set; }

        public string AwayNameEN { get; set; }
        public string AwayNameCS { get; set; }
    }
}