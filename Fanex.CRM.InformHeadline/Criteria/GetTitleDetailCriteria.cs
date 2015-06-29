// -----------------------------------------------------------------------
// <copyright file="GetTitleDetailCriteria.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetTitleDetailCriteria : CriteriaBase
    {
        public int TitleId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_InformTitleDetail";
        }

        public override bool IsValid()
        {
            return TitleId > 0;
        }
    }
}
