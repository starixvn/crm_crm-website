namespace Fanex.CRM.BackgroundJob.Models
{
    public class TransactionInfo
    {
        public int MessageId { get; set; }
        public int RequestId { get; set; }
        public long TransId { get; set; }
        public int CustId { get; set; }
        public string Username { get; set; }
        public string Bettype { get; set; }
        public string Site { get; set; }
        public bool IsDeposit { get; set; }

        public int LicenseeManagerId { get; set; }
        public string LicenseeManager { get; set; }
    }
}