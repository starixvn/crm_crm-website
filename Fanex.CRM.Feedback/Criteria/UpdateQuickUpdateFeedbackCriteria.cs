using Fanex.CRM.Common.Helpers;
using Fanex.CRM.Common.Services;
using Fanex.Data.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.CRM.Feedback.Criteria
{
    public class UpdateQuickUpdateFeedbackCriteria : CriteriaBase
    {
        public int FeedbackID { get; set; }

        public int NoInquiries { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_CustomerFeedbackQuickUpd";
        }

        public override Hashtable GetSpParams()
        {
            Hashtable inputParams = new Hashtable
            {
                { "feedbackid", FeedbackID },
                { "userid", AppSession.CurrentUser.Id },
                { "NoInquiries", NoInquiries },
                { "ip", IPHelper.ClientIP },
                { "errcode", ErrorCode },
                { "errmsg", ErrorMessage }
            };
            
            return inputParams;
        }

        public override bool IsValid()
        {
            return FeedbackID > 0;
        }
    }
}
