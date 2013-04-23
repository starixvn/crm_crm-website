namespace Fanex.CRM.Infrastructure.Common
{
    public class Enumerations
    {
        public static class Functions
        {
            public enum AdminTool
            {
                AdminTool = 0, SettingManagement = 1, SettingRelationManagement = 2, FBSubjectTemplateManagement = 3
            }

            public enum Feedback
            {
                CreateFeedback = 0, EditFeedback = 1, FeedbackReview = 2, DeleteFeedback = 3
            }

            public enum CRMCustomer
            {
                CustomerInfo = 0, SearchCustomerInfo = 1
            }

            public enum CRMReport
            {
                FeedbackStatistics = 0
            }

            public enum RevewMode 
            { 
                NotUpdate = 0, Update = 1, Remove = 2
            }

            public enum CRMStatus
            {
                Pending = 11001, Closed = 11002, Solved = 11003, Deleted = 11004, Checked = 1106
            }
        }

        public enum ReportTypes
        {
            All = 0,
            CreditSite = 1,
            Licensee = 2
        }

        public enum ErrorCode
        {
            SettingIsExist = 100,
            SystemError = 101,
            
            UpdateFeedback = 500,
            UpdateCustomerInfo = 501,
            UpdateSetting = 502,
            DeleteFeedback = 503
        }

    }
}
