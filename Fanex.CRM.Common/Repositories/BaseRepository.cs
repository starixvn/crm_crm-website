// --------------------------------------------------------------------------
// <copyright file="BaseRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Repositories
{
    using Fanex.Common;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Base Repository.
    /// </summary>
    public class BaseRepository
    {
        /// <summary>
        /// Runs the store procedure.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Run SP result.</returns>
        public Result RunSP(CriteriaBase criteria)
        {
            Result runResult = null;
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.ExecuteNonQuery();
                var outputParams = objectDB.GetOuputParams();
                runResult = new Result
                {
                    Code = Converter.ToInt(outputParams["errcode"]),
                    Message = Converter.ToString2(outputParams["errmsg"])
                };
            }

            return runResult;
        }
    }
}
