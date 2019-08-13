namespace Fanex.CRM.AdminTool.Criteria.FeedbackSubjectManagement
{
    using System.Collections;
    using Common.Enumerations;
    using Common.Services;
    using Fanex.CRM.Common.Models;

    public class SearchFeedbackSubjectTemplateCriteria : PagingCriteria
    {
        public SearchFeedbackSubjectTemplateCriteria()
        {
            UserRole = AppSession.CurrentUser.UserRole;
        }

        public UserRole UserRole { get; set; }

        public string Subject { get; set; }

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("templateid", 0);
            param.Add("UserRole", UserRole);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Setting_FeedbackTemplateSubject_Get";
        }

        public override bool IsValid() => true;
    }
}