using Fanex.Common;
using Fanex.CRM.PicAndHeadline.Criteria;
using Fanex.CRM.PicAndHeadline.Models;
using Fanex.Data;
using System.Collections.Generic;

namespace Fanex.CRM.PicAndHeadline.Repositories
{
    public class PicReportRepository : IPicReportRepository
    {
        public IEnumerable<PicInfo> GetReport(GetReportCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                var data = objectDB.Query<PicInfo>(criteria.GetSpParams());
                var outputParams = objectDB.GetOuputParams();
                if (outputParams.Count != 0)
                {
                    criteria.TotalRows = Converter.ToInt(outputParams["totalrows"]);
                }

                return data;
            }
        }
    }
}
