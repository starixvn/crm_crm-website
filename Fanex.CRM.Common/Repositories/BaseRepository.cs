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
    using Fanex.Data.Repository;

    /// <summary>
    /// Base Repository.
    /// </summary>
    public class BaseRepository
    {
        public Result ExecuteNonQuery(CriteriaBase criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.ExecuteNonQuery(criteria.GetSpParams());
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
