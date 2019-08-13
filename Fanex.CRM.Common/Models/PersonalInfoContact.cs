namespace Fanex.CRM.Common.Models
{
    using System;

    [Serializable]
    public class PersonalInfoContact
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Phone))
            {
                return Name + Phone;
            }

            return Name + " - " + Phone;
        }
    }
}