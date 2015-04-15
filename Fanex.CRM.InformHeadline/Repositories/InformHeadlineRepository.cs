// -----------------------------------------------------------------------
// <copyright file="InformHeadlineRepository.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Repositories
{
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.CRM.InformHeadline.Criteria;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InformHeadlineRepository : IInformHeadlineRepository
    {
        /// <summary>
        /// Get informs data.
        /// </summary>
        /// <param name="criteria">Get inform criteria.</param>
        /// <returns>List of informs.</returns>
        public IList<InformHeadline> GetInforms(GetInformHeadlinesCriteria criteria)
        {
            var informs = new List<InformHeadline>();
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                var rawdata = db.Query<InformHeadline>(criteria.GetSpParams());
                foreach (var item in rawdata)
                {                    
                    informs.Add(item);
                }
                
                return informs;
            }
        }

        /// <summary>
        /// Update inform status.
        /// </summary>
        /// <param name="criteria">Criteria data.</param>
        /// <returns>Result object.</returns>
        public Result UpdateInformStatus(UpdateInformStatusCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.ExecuteNonQuery();
                var outputParams = objectDB.GetOuputParams();
                return new Result
                {
                    Code = Converter.ToInt(outputParams["errcode"]),
                    Message = Converter.ToString2(outputParams["errmsg"])
                };
            }
        }

        /// <summary>
        /// Get direct accounts for new case.
        /// </summary>
        /// <param name="criteria">Get direct account criteria.</param>
        /// <returns>List of direct account.</returns>
        public IList<DirectAccount> GetDirectAccounts(GetDirectAccountsCriteria criteria)
        {
            var result = new List<DirectAccount>();
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                var rawdata = db.Query<DirectAccount>(criteria.GetSpParams());
                foreach (var item in rawdata)
                {
                    result.Add(item);
                }

                return result;
            }
        }

        /// <summary>
        /// Get inform statuses.
        /// </summary>
        /// <param name="criteria">Criteria object.</param>
        /// <returns>List of status.</returns>
        public IList<InformStatus> GetInformStatuses(GetInformStatusesCriteria criteria)
        {
            var result = new List<InformStatus>();
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                var rawdata = db.Query<InformStatus>(criteria.GetSpParams());
                foreach (var item in rawdata)
                {
                    result.Add(item);
                }

                return result;
            }
        }

        /// <summary>
        /// Update Inform headline for new cases.
        /// </summary>
        /// <param name="criteria">Criteria object.</param>
        /// <returns>Result object.</returns>
        public Result UpdateInformHeadline(UpdateInformHeadlineCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.ExecuteNonQuery();
                var outputParams = objectDB.GetOuputParams();
                return new Result
                {
                    Code = Converter.ToInt(outputParams["errcode"]),
                    Message = Converter.ToString2(outputParams["errmsg"])
                };
            }
        }
    }
}
