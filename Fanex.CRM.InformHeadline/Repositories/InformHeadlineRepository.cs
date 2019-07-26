namespace Fanex.CRM.InformHeadline.Repositories
{
    using System.Data;
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.InformHeadline.Criteria;
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
    }
}