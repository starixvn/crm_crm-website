namespace Fanex.CRM.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Criteria.PicAndHeadline;
    using Fanex.CRM.Models.PicAndHeadline;
    using Fanex.Data;

    public interface IPicAssignmentRepository
    {
        Tuple<PicAssignmentValidation, IEnumerable<AssignedDirectAccount>> ValidateAssignmentDirectAccount(ValidateAssignmentCriteria criteria);
    }

    public class PicAssignmentRepository : IPicAssignmentRepository
    {
        public Tuple<PicAssignmentValidation, IEnumerable<AssignedDirectAccount>> ValidateAssignmentDirectAccount(ValidateAssignmentCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = objectDB.QueryMultiple(criteria.GetSpParams()))
                {
                    var validation = multi.Read<PicAssignmentValidation>();
                    var assignedDirectAccount = multi.Read<AssignedDirectAccount>();
                    var commandResult = new CommandResult();

                    var outputParams = objectDB.GetOuputParams();
                    if (outputParams.Count != 0)
                    {
                        criteria.ErrCode = Converter.ToInt(outputParams["errcode"]);
                        criteria.ErrMsg = Converter.ToString2(outputParams["errmsg"]);
                    }

                    return new Tuple<PicAssignmentValidation, IEnumerable<AssignedDirectAccount>>(validation.FirstOrDefault(), assignedDirectAccount);
                }
            }
        }
    }
}