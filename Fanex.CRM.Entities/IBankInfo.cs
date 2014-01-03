namespace Fanex.CRM.Entities
{
    public interface IBankInfo
    {
        string BankName { get; set; }

        string BankAddress { get; set; }

        string SwiftCode { get; set; }

        string AccountName { get; set; }

        string AccountNo { get; set; }
    }
}
