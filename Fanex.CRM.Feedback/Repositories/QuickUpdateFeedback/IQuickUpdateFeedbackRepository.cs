using Fanex.CRM.Feedback.Criteria;
using Fanex.CRM.Feedback.Models;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.CRM.Feedback.Repositories
{
    public interface IQuickUpdateFeedbackRepository
    {
        QuickUpdateFeedback GetFeedback(ICriteria criteria);

        void UpdateFeedback(UpdateQuickUpdateFeedbackCriteria criteria);
    }
}
