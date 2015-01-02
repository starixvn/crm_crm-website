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
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Remark.Criteria;
    using Fanex.CRM.Remark.Models;
    using Fanex.Data;
    using Fanex.Data.Repository;

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
        public Remarks GetRemarks(ICriteria criteria)
        {
            IList<string> specialRemarks = new List<string>();
            IList<string> remarks = new List<string>();

            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader != null)
                    {
                        string remark = null;
                        while (reader.Read())
                        {
                            // Old editor is textarea
                            // New editor is wysiwyg html editor
                            // So, we MUST replace like below to make sure that remarks HISTORY are displayed properly.
                            remark = Converter.ToString2(reader["Remark"])
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
                    }
                }
            }

            return new Remarks { SpecialRemarks = specialRemarks, NormalRemarks = remarks };
        }

        /// <summary>
        /// Updates the remarks.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The result object.</returns>
        public Result UpdateRemarks(UpdateRemarkCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.ExecuteNonQuery(criteria.GetSpParams());
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
