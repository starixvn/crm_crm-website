namespace Fanex.CRM.Common.Models
{
    using System;

    [Serializable]
    public class Phone
    {
        public string CountryCode { get; set; }

        public string AreaCode { get; set; }

        public string PhoneNumber { get; set; }

        public override string ToString()
        {
            string firstJoin = JoinTwoFields(CountryCode, AreaCode);
            return JoinTwoFields(firstJoin, PhoneNumber);
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