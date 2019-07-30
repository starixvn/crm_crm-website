namespace Fanex.CRM.InformHeadline.Models
{
    public class TitleDetail
    {
        public int TitleId { get; set; }

        public string Title { get; set; }

        public string Currencies { get; set; }

        public string Sites { get; set; }

        public int TypeAccount { get; set; }

        public string Positions { get; set; }

        public bool ExcludeIgnoredAccounts { get; set; }
    }
}