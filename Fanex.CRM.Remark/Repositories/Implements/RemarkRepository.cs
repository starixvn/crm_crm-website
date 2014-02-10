// --------------------------------------------------------------------------
// <copyright file="RemarkRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.10">Created</change>
// </history>

namespace Fanex.CRM.Remark.Repositories.Implements
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Remark.Models;
    using Fanex.CRM.Remark.Repositories.Interfaces;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Remark repository class.
    /// </summary>
    public class RemarkRepository : IRemarkRepository
    {
        #region IRemarkRepository Members

        /// <summary>
        /// Gets the remarks.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Remark objects list.</returns>
        public IEnumerable<RemarkDetail> GetRemarks(ICriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                return objectDB.Query<RemarkDetail>(criteria.GetSpParams());
            }
        }

        /// <summary>
        /// Updates the remarks.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The result object.</returns>
        public Result UpdateRemarks(ICriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                var effectedRows = objectDB.ExecuteNonQuery();
                
                var outputParams = objectDB.GetOuputParams();
                return new Result
                {
                    Code = Converter.ToInt(outputParams["ErrCode"]),
                    Message = Converter.ToString2(outputParams["ErrMsg"])
                };
            }
        }

        #endregion
    }
}
