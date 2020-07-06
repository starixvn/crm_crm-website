using Fanex.Common;
using Fanex.CRM.PicAndHeadline.Criteria;
using Fanex.CRM.PicAndHeadline.Models;
using Fanex.Data;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.CRM.PicAndHeadline.Repositories
{
    public class PicReportRepository : IPicReportRepository
    {
        public IEnumerable<PicInfo> GetReport(GetReportCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = objectDB.QueryMultiple(criteria.GetSpParams()))
                {
                    var pics = multi.Read<PicInfo>();
                    var communicationTools = multi.Read<CommunicationTool>();

                    foreach(var p in pics)
                    {
                        p.CommunicationTools = communicationTools.Where(c => c.PicId == p.PicId).ToList();
                    }

                    var outputParams = objectDB.GetOuputParams();
                    if (outputParams.Count != 0)
                    {
                        criteria.TotalRows = Converter.ToInt(outputParams["totalrows"]);
                    }

                    return pics;
                }
            }
        }
    }
}
