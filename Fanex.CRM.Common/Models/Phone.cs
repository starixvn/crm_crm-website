// --------------------------------------------------------------------------
// <copyright file="PhoneInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Models
{
    using System;

    /// <summary>
    /// Phone Info.
    /// </summary>
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

        private string JoinTwoFields(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(value))
            {
                return name + value;
            }

            return name + " - " + value;
        }
    }
}
