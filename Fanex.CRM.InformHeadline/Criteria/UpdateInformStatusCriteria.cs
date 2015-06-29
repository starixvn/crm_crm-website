// -----------------------------------------------------------------------
// <copyright file="UpdateInformStatusCriteria.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Criteria
{
    using Fanex.CRM.Common.Criteria;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateInformStatusCriteria : UpdateCriteria
    {
        public int TitleId { get; set; }

        public bool StatusType { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_InformTitleStatusUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
