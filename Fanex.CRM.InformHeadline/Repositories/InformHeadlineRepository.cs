// -----------------------------------------------------------------------
// <copyright file="InformHeadlineRepository.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
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
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<InformHeadline>(criteria.GetSpParams()).ToList();
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
        /// Get informs for new case.
        /// </summary>
        /// <param name="criteria">Get new informs criteria.</param>
        /// <returns>List of new informs.</returns>
        public IList<NewInform> GetNewInforms(GetNewInformsCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<NewInform>(criteria.GetSpParams()).ToList();                
            }
        }

        /// <summary>
        /// Get inform statuses.
        /// </summary>
        /// <param name="criteria">Criteria object.</param>
        /// <returns>List of status.</returns>
        public IList<InformStatus> GetInformStatuses(GetInformStatusesCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<InformStatus>(criteria.GetSpParams()).ToList();                
            }
        }

        /// <summary>
        /// Update Inform headline for new cases.
        /// </summary>
        /// <param name="criteria">Criteria object.</param>
        /// <returns>Result object.</returns>
        public Result UpdateContentInformHeadline(UpdateContentInformHeadlineCriteria criteria)
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
        /// Insert Inform headline for new cases.
        /// </summary>
        /// <param name="criteria">Criteria object.</param>
        /// <returns>Result object.</returns>
        public Result InsertInformHeadline(InsertInformHeadlineCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.AddOutParameter("titleid", DbType.Int32, 1);
                objectDB.ExecuteNonQuery();
                var outputParams = objectDB.GetOuputParams();                
                return new Result
                {
                    Code = Converter.ToInt(outputParams["errcode"]),
                    Message = Converter.ToString2(outputParams["errmsg"]),
                    ExtendedData = Converter.ToInt(outputParams["titleid"])
                };
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

        /// <summary>
        /// Get informs for update case.
        /// </summary>
        /// <param name="criteria">Get update informs criteria.</param>
        /// <returns>List of update informs.</returns>
        public IList<UpdateInform> GetUpdateInforms(GetUpdateInformsCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<UpdateInform>(criteria.GetSpParams()).ToList();                
            }
        }

        public TitleDetail GetTileDetail(GetTitleDetailCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.QueryEntity<TitleDetail>(criteria.GetSpParams());               
            }
        }

        public IList<UpdateInform> GetContactInforms(GetContactInformsCriteria criteria)
        {
            criteria.UserName = criteria.UserName ?? string.Empty;
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<UpdateInform>(criteria.GetSpParams()).ToList();
            }
        }
    }
}
