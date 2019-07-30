namespace Fanex.CRM.Customer.Criteria
{
    using Common.Criteria;

    public class CustomerAutoSuggestionCriteria : BasedCriteria
    {
        public int UserId { get; set; }

        public string Username { get; set; }

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