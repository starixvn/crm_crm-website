namespace Fanex.CRM.AdminTool.Criteria.FeedbackSubjectManagement
{
    using System.Collections;
    using Common.Criteria;

    public class FeedbackSubjectTemplateByIdCriteria : BasedCriteria
    {
        public int TemplateId { get; set; }

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("templateid", TemplateId);
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