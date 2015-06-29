// -----------------------------------------------------------------------
// <copyright file="GetUpdateInformsCriteria.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetUpdateInformsCriteria : CriteriaBase
    {
        public int TitleId { get; set; }

        public int TypeAccount { get; set; }

        public string Currency { get; set; }

        public string Level { get; set; }

        public string Site { get; set; }

        public bool ChangeFilter { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_InformHeadlineSearchSel";
        }

        public override bool IsValid()
        {
            return TitleId > 0;
        }
    }
}
