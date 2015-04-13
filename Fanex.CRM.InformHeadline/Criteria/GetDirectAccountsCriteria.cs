// -----------------------------------------------------------------------
// <copyright file="GetDirectAccountsCriteria.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetDirectAccountsCriteria : CriteriaBase
    {
        public int TypeAccount { get; set; }

        public string Currency { get; set; }

        public string Level { get; set; }

        public string Site { get; set; }

        public int PageIndex { get; set; }

        public int ItemsPerPage { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_InformHeadlineSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
