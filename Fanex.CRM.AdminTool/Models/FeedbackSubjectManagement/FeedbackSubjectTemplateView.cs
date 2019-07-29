namespace Fanex.CRM.AdminTool.Models.FeedbackSubjectManagement
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;

    public class FeedbackSubjectTemplateView : BaseModel
    {
        public List<FeedbackSubjectTemplate> Templates { get; set; }

        public int TotalTemplates { get; set; }
    }
}