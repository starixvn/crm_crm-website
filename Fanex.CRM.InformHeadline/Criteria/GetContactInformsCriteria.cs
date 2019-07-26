namespace Fanex.CRM.InformHeadline.Criteria
{
    using Fanex.Data.Repository;

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