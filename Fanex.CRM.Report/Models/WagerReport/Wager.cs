namespace Fanex.CRM.Report.Models.WagerReport
{
    using System;

    [Serializable]
    public class Wager
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Amount { get; set; }
    }
}
