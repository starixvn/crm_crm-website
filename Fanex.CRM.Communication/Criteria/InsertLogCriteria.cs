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
    using Common.Helpers;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;
    using Models;
    using System.Collections;

    /// <summary>
    /// Insert Log Criteria.
    /// </summary>
    public class InsertLogCriteria : ModifyLogCriteria
    {
        public System.Collections.Generic.IEnumerable<ProductFilter> Products { get; set; }

        public string ProductsContent
        {
            get
            {
                return SerializerHelper.SerializeList(this.Products);
            }
        }

        public System.Collections.Generic.IEnumerable<CategoryFilter> Categories { get; set; }

        public string CategoriesContent
        {
            get
            {
                return SerializerHelper.SerializeList(this.Categories);
            }
        }

        public string CommunicationIds { get; set; }

        public string ExtensionIds { get; set; }

        [SpParam(SqlParamName = "creatorid")]
        public int CreatorId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        [SpParam(SqlParamName = "autofield")]
        public string AutoField { get; set; }

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

            param.Add("communicationid", CommunicationIds);
            param.Add("stationid", ExtensionIds);
            param.Add("category", CategoriesContent);
            param.Add("product", ProductsContent);

            param.Add("siteid", SiteId);
            param.Add("roleid", RoleId);
            param.Add("responseid", ResponseId);
            param.Add("statusid", StatusId);
            param.Add("licenseetype", LicenseeTypeId);
            param.Add("description", StringExtension.GetValueNotNull(Remark));
            param.Add("solution", StringExtension.GetValueNotNull(Solution));
            param.Add("subject", StringExtension.GetValueNotNull(Subject));
            param.Add("userid", UserId);
            param.Add("ip", IP);
            param.Add("autofield", AutoField);

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
