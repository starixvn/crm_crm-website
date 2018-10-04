using Fanex.CRM.Common.Helpers;
using Fanex.CRM.Feedback.Models;
using Fanex.Data.Repository;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fanex.CRM.Feedback.Criteria
{
    public class CheckFeedbackDuplicationCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public DateTime StartTime { get; set; }

        public IEnumerable<CategoryFilter> Categories { get; set; }

        public string CategoriesContent
        {
            get
            {
                return SerializerHelper.SerializeList(this.Categories);
            }
        }

        public int FeedbackID { get; set; }

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("CustId", CustId);
            param.Add("StartTime", DateTimeUtilize.ChangeTimeZoneToServer(StartTime));
            param.Add("Category", CategoriesContent);
            param.Add("FeedbackID", FeedbackID);

            return param;
        }


        public override string GetSettingKey()
        {
            return "Acc_CRM_FeedbackDoubleCheck";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
