namespace Fanex.CRM.AdminTool.Criteria.FeedbackSubjectManagement
{
    using System.Collections;
    using Common.Criteria;

    public class ChangeSubjectTemplateStateCriteria : BasedCriteria
    {
        public int TemplateId { get; set; }

        public bool IsActive { get; set; }

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("templateid", TemplateId);
            param.Add("UserRole", UserRole);
            param.Add("isactive", IsActive);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            return param;
        }

        public override string GetSettingKey()
        {
            return "FeedSubjectTemplateDelete";
        }

        public override bool IsValid() => true;
    }
}