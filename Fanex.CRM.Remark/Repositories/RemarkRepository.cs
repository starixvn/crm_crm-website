// --------------------------------------------------------------------------
// <copyright file="RemarkRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.10">Created</change>
// </history>

namespace Fanex.CRM.Remark.Repositories
{
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Repositories;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Remark.Criteria;
    using Fanex.CRM.Remark.Models;
    using Fanex.Data;
    using Fanex.Data.Repository;

    /// <summary>
    /// Remark repository class.
    /// </summary>
    public class RemarkRepository : BaseRepository, IRemarkRepository
    {
        #region IRemarkRepository Members

        public Remarks GetRemarks(ICriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var specialRemarks = new List<string>();
                    var remarks = new List<string>();

                    while (reader.Read())
                    {                        
                        var remark = Converter.ToString2(reader["Remark"])
                            .Replace(new string[] { "\n", "\r", "\n\r", "\r\n" }, "<br/>");
                        if (Converter.ToBool(reader["IsSpecial"]))
                        {
                            specialRemarks.Add(remark);
                        }
                        else
                        {
                            remarks.Add(remark);
                        }
                    }

                    return new Remarks { SpecialRemarks = specialRemarks, NormalRemarks = remarks };
                }
            }
        }

        public Result UpdateRemarks(UpdateRemarkCriteria criteria)
        {
            return ExecuteNonQuery(criteria);            
        }

        #endregion
    }
}
