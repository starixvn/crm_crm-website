using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.CRM.Feedback.Criteria
{
    public class GetQuickUpdateFeedbackCriteria : CriteriaBase
    {
        public int FeedbackID { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_FeedbackQuickSel";
        }

        public override bool IsValid()
        {
            return FeedbackID > 0;
        }
    }
}
