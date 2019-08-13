namespace Fanex.CRM.Feedback.Criteria
{
    using Common.Criteria;
    using System.Collections;

    public class LatestFeedbackCriteria : BasedCriteria
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public int Top { get; set; }

        public override string GetSettingKey()
        {
            if (CustId == 0)
            {
                return "FeedbackSelTopFeedback";
            }

            return "FeedbackSelTopFeedbackByCustId";
        }

        public override Hashtable GetSpParams()
        {
            var inputParams = new Hashtable();
            inputParams.Add("top", Top);
            inputParams.Add("UserRole", UserRole);
            if (CustId != 0)
            {
                inputParams.Add("custid", CustId);
            }

            return inputParams;
        }

        public override bool IsValid()
        {
            return Top > 0;
        }
    }
}
