namespace Fanex.CRM.AdminTool.Criteria.FeedbackSubjectManagement
{
    using System.Collections;
    using Common.Criteria;

    public class EditFeedbackSubjectTemplateCriteria : BasedCriteria
    {
        public int Id { get; set; }

        public string TemplateContent { get; set; }

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("templateid", Id);
            param.Add("UserRole", UserRole);
            param.Add("subject", TemplateContent);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            return param;
        }

        public override string GetSettingKey()
        {
            return "FeedSubjectTemplateUpdate";
        }

        public override bool IsValid() => true;
    }
}