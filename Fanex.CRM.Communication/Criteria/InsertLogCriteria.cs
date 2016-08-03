// --------------------------------------------------------------------------
// <copyright file="InsertLogCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Communication.Criteria
{
    using System.Collections;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    /// <summary>
    /// Insert Log Criteria.
    /// </summary>
    public class InsertLogCriteria : ModifyLogCriteria
    {
        [SpParam(SqlParamName = "creatorid")]
        public int CreatorId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

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
            param.Add("subproductid", SubProductId);
            param.Add("bettypeid", BetTypeId);
            param.Add("responseid", ResponseId);
            param.Add("statusid", StatusId);
            param.Add("licenseetype", LicenseeTypeId);
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
    }
}
