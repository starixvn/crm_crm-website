namespace Fanex.CRM.InformHeadline.Repositories
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.InformHeadline.Criteria;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.Data;

    public class InformHeadlineRepository : IInformHeadlineRepository
    {
        public CommandResult InsertInformHeadline(InsertInformHeadlineCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.AddOutParameter("titleid", DbType.Int32, 1);
                objectDB.ExecuteNonQuery();
                var outputParams = objectDB.GetOuputParams();
                return new CommandResult
                {
                    Code = Converter.ToInt(outputParams["errcode"]),
                    Message = Converter.ToString2(outputParams["errmsg"]),
                    ExtendedData = Converter.ToInt(outputParams["titleid"])
                };
            }
        }

        public IEnumerable<RemovedAccountLog> GetRemovedAccountLogs(GetRemovedAccountLogsCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                var logs = objectDB.Query<RemovedAccountLog>(criteria.GetSpParams());
                Hashtable outputParams = objectDB.GetOuputParams();
                if (outputParams.Count != 0)
                {
                    criteria.TotalRows = Converter.ToInt(outputParams["totalrows"]);
                }

                return logs;
            }
        }

        public IEnumerable<RemovedAccountLogDetails> GetRemovedAccountLogDetails(GetRemovedAccountLogDetailsCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                var logs = objectDB.Query<RemovedAccountLogDetails>(criteria.GetSpParams());
                Hashtable outputParams = objectDB.GetOuputParams();
                if (outputParams.Count != 0)
                {
                    criteria.TotalRows = Converter.ToInt(outputParams["totalrows"]);
                }

                return logs;
            }
        }
    }
}