// --------------------------------------------------------------------------
// <copyright file="UpdateCustomerVerifyFlagCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Dashboard.Criteria
{
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Customer Dashboard Information Criteria.
    /// </summary>
    public class UpdateCustomerVerifyFlagCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        public bool IsVerifyEmail { get; set; }

        public bool IsVerifyPassport { get; set; }

        public bool IsVerifyBirthday { get; set; }

        public bool IsVerifySecretCode { get; set; }

        public bool IsVerifyBetCredit { get; set; }

        public bool IsVerifyGivenCredit { get; set; }

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

        [SpParam(SqlParamName = "errcode")]
        public int ErrorCode { get; set; }

        [SpParam(SqlParamName = "errmsg")]
        public string ErrorMessage { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_Dashboard_UpdateVerifyFlag";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
