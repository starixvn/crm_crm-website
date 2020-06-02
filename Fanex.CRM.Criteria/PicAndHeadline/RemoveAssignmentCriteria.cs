namespace Fanex.CRM.Criteria.PicAndHeadline
{
    using System.Collections;
    using Fanex.CRM.Common.Criteria;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Services;

    public class RemoveAssignmentCriteria : BasedCriteria
    {
        public string RemovedPICs { get; set; }

        public bool InActivatedPIC { get; set; }

        public static int UserId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        public static string IP
        {
            get
            {
                return IPHelper.ClientIP;
            }
        }

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("RemovedPICs", RemovedPICs);
            param.Add("InActivatedPIC", InActivatedPIC);

            param.Add("userid", UserId);
            param.Add("loginip", IP);
            param.Add("ErrCode", 0);
            param.Add("ErrMsg", string.Empty);

            return param;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_PICAssignmentDel";
        }

        public override bool IsValid() => !string.IsNullOrWhiteSpace(RemovedPICs);
    }
}