namespace Fanex.CRM.AdminTool.Models.FeedbackSubjectManagement
{
    using Fanex.CRM.Common.Models;

    public class FeedbackSubjectTemplate : BaseModel
    {
        public string TemplateContent { get; set; }

        public bool IsActive { get; set; }
    }
}