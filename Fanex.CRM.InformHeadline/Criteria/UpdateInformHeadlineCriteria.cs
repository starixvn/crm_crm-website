// -----------------------------------------------------------------------
// <copyright file="UpdateInformHeadlineCriteria.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Criteria
{
    using Fanex.CRM.Common.Criteria;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateInformHeadlineCriteria : UpdateCriteria
    {
        public int TitleId { get; set; }

        public string Content { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_InformHeadlineUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
