// --------------------------------------------------------------------------
// <copyright file="ModifyLogCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Communication.Criteria
{
    using System;
    using Fanex.CRM.Common.Criteria;
    using Fanex.Data.Repository;

    /// <summary>
    /// Modify Criteria.
    /// </summary>
    public class ModifyLogCriteria : UpdateCriteria
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

        [SpParam(Ignored = true)]
        public int LicenseeTypeId { get; set; }

        [SpParam(SqlParamName = "description")]
        public string Remark { get; set; }

        [SpParam(SqlParamName = "solution")]
        public string Solution { get; set; }

        [SpParam(SqlParamName = "subject")]
        public string Subject { get; set; }

        public override bool IsValid()
        {
            return true;
        }
    }
}
