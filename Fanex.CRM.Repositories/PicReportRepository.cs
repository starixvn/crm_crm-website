using System.Collections.Generic;
using System.Linq;
using Fanex.Common;
using Fanex.CRM.Criteria.PicAndHeadline;
using Fanex.CRM.Models.PicAndHeadline;
using Fanex.Data;

namespace Fanex.CRM.Repositories
{
    public interface IPicReportRepository
    {
        IEnumerable<PicInfo> GetReport(GetReportCriteria criteria);

        IEnumerable<Common.Models.Setting> GetPicCommunicationTools(GetPicCommunicationToolsCriteria criteria);
    }

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

                    foreach (var p in pics)
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

        public IEnumerable<Common.Models.Setting> GetPicCommunicationTools(GetPicCommunicationToolsCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                return objectDB.Query<Common.Models.Setting>(criteria.GetSpParams());
            }
        }
    }
}