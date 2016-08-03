// --------------------------------------------------------------------------
// <copyright file="UpdateRemarkCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.10">Created</change>
// </history>

namespace Fanex.CRM.Remark.Criteria
{
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    /// <summary>
    /// Update remarks criteria.
    /// </summary>
    public class UpdateRemarkCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public string Remarks { get; set; }

        public int UserId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        public string IP
        {
            get
            {
                return IPHelper.ClientIP;
            }
        }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_CustomerRemarkUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
