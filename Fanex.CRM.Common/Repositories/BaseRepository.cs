namespace Fanex.CRM.Common.Repositories
{
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public interface IBaseRepository
    {
        CommandResult ExecuteNonQuery(ICriteria criteria);
    }

    public class BaseRepository : IBaseRepository
    {
        public CommandResult ExecuteNonQuery(ICriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = objectDB.GetOuputParams();
                return new CommandResult
                {
                    Code = Converter.ToInt(outputParams["errcode"]),
                    Message = Converter.ToString2(outputParams["errmsg"])
                };
            }
        }
    }
}