// --------------------------------------------------------------------------
// <copyright file="UpdateInformHeadlinesCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.14">Created</change>
// </history>

namespace Fanex.CRM.Customer.Criteria.InformHeadlines
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Customer.Models.InformHeadlines;
    using Fanex.Data.Repository;

    /// <summary>
    /// UpdateInformHeadlinesCriteria class.
    /// </summary>
    public class UpdateInformHeadlinesCriteria : CriteriaBase
    {
        public int TitleId { get; set; }

        public string Title { get; set; }

        [SpParam(Ignored = true)]
        public IList<InformHeadline> Informs { get; set; }

        [SpParam(Ignored = true)]
        public bool IsTitleChanged { get; set; }

        public int UserId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        public string Content { get; set; }

        public string IP 
        {
            get
            {
                return IPHelper.ClientIP;
            }
        }

        public int NoHeadLine { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_InformHeadline_Update";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
