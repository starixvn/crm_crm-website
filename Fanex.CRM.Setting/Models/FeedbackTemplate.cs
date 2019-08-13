namespace Fanex.CRM.Setting.Models
{
    using System;

    [Serializable]
    public class FeedbackTemplate
    {
        public int TemplateId { get; set; }

        public string Subject { get; set; }

        public bool IsActive { get; set; }
    }
}