using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Common;
using Fanex.CRM.Feedback.Criteria;
using Fanex.CRM.Feedback.Models;
using Fanex.Data;
using Fanex.Data.Repository;

namespace Fanex.CRM.Feedback.Repositories
{
    public class QuickUpdateFeedbackRepository : IQuickUpdateFeedbackRepository
    {
        public QuickUpdateFeedback GetFeedback(ICriteria criteria)
        {
            QuickUpdateFeedback feedback = new QuickUpdateFeedback();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader != null && reader.Read())
                    {
                        feedback.FeedbackId = Converter.ToInt(reader["FeedbackId"]);
                        feedback.NoInquiries = Converter.ToInt(reader["NoInquiries"]);                                                   
                    }
                }
            }

            return feedback;
        }

        public void UpdateFeedback(UpdateQuickUpdateFeedbackCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();
                if (outputParams != null && outputParams.Count > 0)
                {
                    criteria.ErrorCode = (int)outputParams["errcode"];
                    criteria.ErrorMessage = (string)outputParams["errmsg"];
                }
            }
        }
    }
}
