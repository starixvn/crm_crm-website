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
        public IList<InformHeadline> GetInforms(GetInformHeadlineCriteria criteria)
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
        public Result UpdateInformStatus(UpdateInformHeadlineCriteria criteria)
        {
            Result editResult = null;
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.ExecuteNonQuery();
                var outputParams = objectDB.GetOuputParams();
                editResult = new Result
                {
                    Code = Converter.ToInt(outputParams["errcode"]),
                    Message = Converter.ToString2(outputParams["errmsg"])
                };
            }

            return editResult;
        }
    }
}
