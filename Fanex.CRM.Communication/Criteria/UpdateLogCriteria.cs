// --------------------------------------------------------------------------
// <copyright file="UpdateLogCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Communication.Criteria
{
    using Common.Helpers;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;
    using Models;

    /// <summary>
    /// Update Log Criteria.
    /// </summary>
    public class UpdateLogCriteria : ModifyLogCriteria
    {
        [SpParam(SqlParamName = "feedbackid")]
        public int FeedbackId { get; set; }

        [SpParam(SqlParamName = "isreviewed")]
        public bool IsReviewed { get; set; }

        [SpParam(SqlParamName = "review")]
        public string Review { get; set; }

        [SpParam(SqlParamName = "updatereviewedmode")]
        public int UpdateReviewedMode { get; set; }

        [SpParam(SqlParamName = "reviewerid")]
        public int ReviewerId
        {
            get
            {
                return IsReviewed ? AppSession.CurrentUser.Id : 0;
            }
        }

        public string AutoField { get; set; }

        [SpParam(Ignored = true)]
        public System.Collections.Generic.IEnumerable<ProductFilter> Products { get; set; }

        [SpParam(SqlParamName = "product")]
        public string ProductsContent
        {
            get
            {
                return SerializerHelper.SerializeList(this.Products);
            }
        }

        [SpParam(Ignored = true)]
        public System.Collections.Generic.IEnumerable<CategoryFilter> Categories { get; set; }

        [SpParam(SqlParamName = "category")]
        public string CategoriesContent
        {
            get
            {
                return SerializerHelper.SerializeList(this.Categories);
            }
        }

        [SpParam(SqlParamName = "communicationid")]
        public string CommunicationIds { get; set; }

        [SpParam(SqlParamName = "stationid")]
        public string ExtensionIds { get; set; }

        public override string GetSettingKey()
        {
            return "Communication_Log_Update";
        }
    }
}
