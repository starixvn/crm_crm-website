using Fanex.CRM.Feedback.Criteria;
using Fanex.CRM.Feedback.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.CRM.Feedback.Services
{
    public interface IQuickUpdateFeedbackService
    {
        QuickUpdateFeedback GetFeedback(GetQuickUpdateFeedbackCriteria criteria);

        void UpdateFeedback(UpdateQuickUpdateFeedbackCriteria criteria);
    }
}
