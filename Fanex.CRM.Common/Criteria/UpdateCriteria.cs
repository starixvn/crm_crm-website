namespace Fanex.CRM.Common.Criteria
{
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    public class UpdateCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "userid")]
        public int UserId => AppSession.CurrentUser.Id;

        [SpParam(SqlParamName = "ip")]
        public string IP => IPHelper.ClientIP;

        [SpParam(SqlParamName = "errcode")]
        public int ErrorCode { get; set; }

        [SpParam(SqlParamName = "errmsg")]
        public string ErrorMessage { get; set; }
    }
}