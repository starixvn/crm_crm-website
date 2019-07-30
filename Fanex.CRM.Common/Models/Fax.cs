namespace Fanex.CRM.Common.Models
{
    using System;

    [Serializable]
    public class Fax
    {
        public string CountryCode { get; set; }

        public string AreaCode { get; set; }

        public string FaxNumber { get; set; }

        public override string ToString()
        {
            string firstJoin = JoinTwoFields(CountryCode, AreaCode);
            return JoinTwoFields(firstJoin, FaxNumber);
        }

        private static string JoinTwoFields(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(value))
            {
                return name + value;
            }

            return name + " - " + value;
        }
    }
}