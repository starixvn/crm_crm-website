// --------------------------------------------------------------------------
// <copyright file="InsertLogCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Communication.Criteria
{
    using System;
    using System.Collections;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Insert Log Criteria.
    /// </summary>
    public class InsertLogCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        [SpParam(SqlParamName = "relatedcustid")]
        public string RelatedCustomerId { get; set; }

        [SpParam(SqlParamName = "languageid")]
        public int LanguageId { get; set; }

        [SpParam(SqlParamName = "countryid")]
        public int CountryId { get; set; }

        [SpParam(SqlParamName = "feedbackdate")]
        public DateTime StartTime { get; set; }

        [SpParam(SqlParamName = "resolutiondate")]
        public DateTime EndTime { get; set; }

        [SpParam(SqlParamName = "communicationid")]
        public int CommunicationToolId { get; set; }

        [SpParam(SqlParamName = "stationid")]
        public int ExtensionId { get; set; }

        [SpParam(SqlParamName = "maincategoryid")]
        public int MainCategoryId { get; set; }

        [SpParam(SqlParamName = "subcategoryid")]
        public int SubCategoryId { get; set; }

        [SpParam(SqlParamName = "siteid")]
        public int SiteId { get; set; }

        [SpParam(SqlParamName = "roleid")]
        public int RoleId { get; set; }

        [SpParam(SqlParamName = "productid")]
        public int ProductId { get; set; }

        [SpParam(SqlParamName = "bettypeid")]
        public int BetTypeId { get; set; }

        [SpParam(SqlParamName = "responseid")]
        public int ResponseId { get; set; }

        [SpParam(SqlParamName = "statusid")]
        public int StatusId { get; set; }

        [SpParam(SqlParamName = "description")]
        public string Remark { get; set; }

        [SpParam(SqlParamName = "solution")]
        public string Solution { get; set; }

        [SpParam(SqlParamName = "subject")]
        public string Subject { get; set; }

        [SpParam(SqlParamName = "creatorid")]
        public int CreatorId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        [SpParam(SqlParamName = "userid")]
        public int UserId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        [SpParam(SqlParamName = "ip")]
        public string IP
        {
            get
            {
                return Helper.GetClientIP();
            }
        }

        [SpParam(SqlParamName = "errcode")]
        public int ErrorCode { get; set; }

        [SpParam(SqlParamName = "errmsg")]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>The SP parameters.</returns>
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("custid", CustomerId);
            param.Add("relatedcustid", StringExtension.GetValueNotNull(RelatedCustomerId));
            param.Add("languageid", LanguageId);
            param.Add("countryid", CountryId);
            param.Add("feedbackdate", StartTime);
            param.Add("resolutiondate", EndTime);
            param.Add("communicationid", CommunicationToolId);
            param.Add("stationid", ExtensionId);
            param.Add("maincategoryid", MainCategoryId);
            param.Add("subcategoryid", SubCategoryId);
            param.Add("siteid", SiteId);
            param.Add("roleid", RoleId);
            param.Add("productid", ProductId);
            param.Add("bettypeid", BetTypeId);
            param.Add("responseid", ResponseId);
            param.Add("statusid", StatusId);
            param.Add("description", StringExtension.GetValueNotNull(Remark));
            param.Add("solution", StringExtension.GetValueNotNull(Solution));
            param.Add("subject", StringExtension.GetValueNotNull(Subject));
            param.Add("userid", UserId);
            param.Add("ip", IP);

            param.Add("errcode", ErrorCode);
            param.Add("errmsg", ErrorMessage);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Communication_Log_Insert";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
