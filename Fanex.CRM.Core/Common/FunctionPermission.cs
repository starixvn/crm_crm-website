// --------------------------------------------------------------------------
// <copyright file="FunctionPermission.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Common
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FunctionPermission
    {
        public const string AdminTool = "AdminTool";

        public const string SettingManagement = "SettingManagement";

        public const string SettingRelationManagement = "SettingManagement";

        public const string FBSubjectTemplateManagement = "FBSubjectTemplateManagement";

        public const string CreateFeedback = "CreateFeedback";

        public const string EditFeedback = "EditFeedback";

        public const string FeedbackReview = "FeedbackReview";

        public const string DeleteFeedback = "DeleteFeedback";

        public const string CustomerInfo = "CustomerInfo";

        public const string SearchCustomerInfo = "Fanex.CRM.Customer.Services.Implements.CustomerService.GetCustomerInfoByUsernameSessionKey";

        public const string FeedbackStatistics = "FeedbackStatistics";
    }
}
