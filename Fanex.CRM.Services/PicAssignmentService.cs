namespace Fanex.CRM.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Criteria.PicAndHeadline;
    using Fanex.CRM.Models.PicAndHeadline;
    using Fanex.CRM.Repositories;

    public interface IPicAssignmentService
    {
        Tuple<PicAssignmentValidation, IEnumerable<AssignedDirectAccount>> ValidateAssignmentDirectAccount(ValidateAssignmentCriteria criteria);
    }

    public class PicAssignmentService : IPicAssignmentService
    {
        private readonly IPicAssignmentRepository _picAssignmentRepository;

        public PicAssignmentService() : this(null)
        {
        }

        public PicAssignmentService(IPicAssignmentRepository picAssignmentRepository)
        {
            _picAssignmentRepository = picAssignmentRepository ?? new PicAssignmentRepository();
        }

        public Tuple<PicAssignmentValidation, IEnumerable<AssignedDirectAccount>> ValidateAssignmentDirectAccount(ValidateAssignmentCriteria criteria)
        {
            return _picAssignmentRepository.ValidateAssignmentDirectAccount(criteria);
        }
    }
}