// -----------------------------------------------------------------------
// <copyright file="InformHeadlineService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.InformHeadline.Criteria;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.CRM.InformHeadline.Repositories;
    using Fanex.CRM.Infrastructure.Helpers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InformHeadlineService : IInformHeadlineService
    {
        private readonly IInformHeadlineRepository _informHeadlineRepository;

        public InformHeadlineService()
            : this(null)
        {
        }

        public InformHeadlineService(IInformHeadlineRepository informHeadlineRepository)
        {
            _informHeadlineRepository = informHeadlineRepository ?? new InformHeadlineRepository();            
        }

        /// <summary>
        /// Get informs.
        /// </summary>
        /// <param name="criteria">Criteria data.</param>
        /// <returns>List of informs.</returns>
        public IList<InformHeadline> GetInforms(GetInformHeadlineCriteria criteria)
        {
            try
            {
                return _informHeadlineRepository.GetInforms(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }
    }
}
