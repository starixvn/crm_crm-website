namespace Fanex.CRM.Common.Criteria
{
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    public class UpdateCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "userid")]
        public int UserId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        [SpParam(SqlParamName = "ip")]
        public string IP
        {
            get
            {
                return IPHelper.ClientIP;
            }
        }

        [SpParam(SqlParamName = "errcode")]
        public int ErrorCode { get; set; }

        [SpParam(SqlParamName = "errmsg")]
        public string ErrorMessage { get; set; }
    }
}