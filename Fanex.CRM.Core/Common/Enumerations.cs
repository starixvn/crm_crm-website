// --------------------------------------------------------------------------
// <copyright file="Enumerations.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Common
{
    /// <summary>
    /// Enumerations class.
    /// </summary>
    public class Enumerations
    {
        public enum ReportTypes
        {
            /// <summary>
            /// All type value.
            /// </summary>
            All = 0,

            /// <summary>
            /// The credit site.
            /// </summary>
            CreditSite = 1,

            /// <summary>
            /// The licensee.
            /// </summary>
            Licensee = 2
        }

        public enum ErrorCode
        {
            /// <summary>
            /// The setting is exist.
            /// </summary>
            SettingIsExist = 100,

            /// <summary>
            /// The system error.
            /// </summary>
            SystemError = 101,

            /// <summary>
            /// The update feedback.
            /// </summary>
            UpdateFeedback = 500,

            /// <summary>
            /// The update customer information.
            /// </summary>
            UpdateCustomerInfo = 501,

            /// <summary>
            /// The update setting.
            /// </summary>
            UpdateSetting = 502,

            /// <summary>
            /// The delete feedback.
            /// </summary>
            DeleteFeedback = 503
        }

        /// <summary>
        /// Functions class.
        /// </summary>
        public static class Functions
        {
            public enum AdminTool
            {
                /// <summary>
                /// The admin tool.
                /// </summary>
                AdminTool = 0,

                /// <summary>
                /// The setting management.
                /// </summary>
                SettingManagement = 1,

                /// <summary>
                /// The setting relation management.
                /// </summary>
                SettingRelationManagement = 2,

                /// <summary>
                /// The FB subject template management.
                /// </summary>
                FBSubjectTemplateManagement = 3
            }

            public enum Feedback
            {
                /// <summary>
                /// The create feedback.
                /// </summary>
                CreateFeedback = 0,

                /// <summary>
                /// The edit feedback.
                /// </summary>
                EditFeedback = 1,

                /// <summary>
                /// The feedback review.
                /// </summary>
                FeedbackReview = 2,

                /// <summary>
                /// The delete feedback.
                /// </summary>
                DeleteFeedback = 3
            }

            public enum CRMCustomer
            {
                /// <summary>
                /// The customer information.
                /// </summary>
                CustomerInfo = 0,

                /// <summary>
                /// The search customer information.
                /// </summary>
                SearchCustomerInfo = 1
            }

            public enum CRMReport
            {
                /// <summary>
                /// The feedback statistics.
                /// </summary>
                FeedbackStatistics = 0
            }

            public enum RevewMode
            {
                /// <summary>
                /// The not update.
                /// </summary>
                NotUpdate = 0,

                /// <summary>
                /// The update.
                /// </summary>
                Update = 1,

                /// <summary>
                /// The remove.
                /// </summary>
                Remove = 2
            }

            public enum CRMStatus
            {
                /// <summary>
                /// The pending.
                /// </summary>
                Pending = 11001,

                /// <summary>
                /// The closed.
                /// </summary>
                Closed = 11002,

                /// <summary>
                /// The solved.
                /// </summary>
                Solved = 11003,

                /// <summary>
                /// The deleted.
                /// </summary>
                Deleted = 11004,

                /// <summary>
                /// The checked.
                /// </summary>
                Checked = 1106
            }
        }
    }
}
