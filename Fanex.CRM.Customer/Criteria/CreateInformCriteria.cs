// --------------------------------------------------------------------------
// <copyright file="CreateInformCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.09">Created</change>
// </history>

namespace Fanex.CRM.Customer.Criteria
{
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// CreateInform criteria.
    /// </summary>
    public class CreateInformCriteria : CriteriaBase
    {
        public string Title { get; set; }

        public int UserId 
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        public override string GetSettingKey()
        {
            return "Customer_InformHeadline_Insert";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Title);
        }
    }
}
