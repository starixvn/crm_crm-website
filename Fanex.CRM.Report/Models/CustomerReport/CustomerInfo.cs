// --------------------------------------------------------------------------
// <copyright file="CustomerInfo.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Report.Models.CustomerReport
{
    using System;

    [Serializable]
    public class CustomerInfo
    {
        public string Headline { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public string Phone3 { get; set; }

        public string Mobile { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string Skype { get; set; }

        public string MSN { get; set; }

        public string WhatsApp { get; set; }

        public string Line { get; set; }

        public string WeChat { get; set; }

        public string Yahoo { get; set; }

        public string BBM { get; set; }

        public string QQ { get; set; }

        public string Language { get; set; }

        public bool IsVIP { get; set; }

        public string Pic1 { get; set; }

        public string Pic2 { get; set; }

        public string Pic3 { get; set; }

        public string Pic4 { get; set; }

        public DateTime? BirthDay { get; set; }

        public string Passport { get; set; }

        public string SiteName { get; set; }
    }
}