namespace Fanex.CRM.Customer.Criteria
{
    using Common.Criteria;
    using Fanex.Data.Repository;

    public class CustomerSuggestionCriteria : BasedCriteria
    {
        public int UserId { get; set; }

        [SpParam(SqlParamName = "Username")]
        public string Query { get; set; }

        public bool IsGuest { get; set; }

        public override string GetSettingKey()
        {
            return "autoSearchSuggest";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}