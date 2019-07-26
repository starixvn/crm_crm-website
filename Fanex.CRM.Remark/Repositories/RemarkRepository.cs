namespace Fanex.CRM.Remark.Repositories
{
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Remark.Models;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class RemarkRepository : IRemarkRepository
    {
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
                            .Replace(new[] { "\n", "\r", "\n\r", "\r\n" }, "<br/>");
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
    }
}