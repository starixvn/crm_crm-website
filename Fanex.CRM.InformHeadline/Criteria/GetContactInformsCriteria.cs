// -----------------------------------------------------------------------
// <copyright file="GetContactInformsCriteria.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetContactInformsCriteria : CriteriaBase
    {
        public int TitleId { get; set; }

        public string UserName { get; set; }

        public string StatusId { get; set; }

        public int PageIndex { get; set; }

        public int ItemsPerPage { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_InformHeadlineDetail";
        }

        public override bool IsValid()
        {
            return TitleId > 0;
        }
    }
}
