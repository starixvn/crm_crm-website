using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.CRM.Feedback.Criteria;
using Fanex.CRM.Feedback.Models;
using Fanex.CRM.Feedback.Repositories;

namespace Fanex.CRM.Feedback.Services
{
    public class QuickUpdateFeedbackService : IQuickUpdateFeedbackService
    {
        private readonly IQuickUpdateFeedbackRepository _quickUpdateFeedbackRepository;

        public QuickUpdateFeedbackService()
            : this(null)
        {
        }

        public QuickUpdateFeedbackService(IQuickUpdateFeedbackRepository quickUpdateFeedbackRepository)
        {
            _quickUpdateFeedbackRepository = quickUpdateFeedbackRepository ?? new QuickUpdateFeedbackRepository();
        }

        public QuickUpdateFeedback GetFeedback(GetQuickUpdateFeedbackCriteria criteria)
        {
            return _quickUpdateFeedbackRepository.GetFeedback(criteria);
        }

        public void UpdateFeedback(UpdateQuickUpdateFeedbackCriteria criteria)
        {
            _quickUpdateFeedbackRepository.UpdateFeedback(criteria);
        }
    }
}
