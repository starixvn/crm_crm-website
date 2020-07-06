namespace Fanex.CRM.Criteria.PicAndHeadline
{
    using System.Collections;
    using Fanex.CRM.Common.Criteria;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Services;

    public class AssignPicCriteria : BasedCriteria
    {
        public int HeadlineId { get; set; }

        public string PicId { get; set; }

        public string DirectAccount { get; set; }

        public bool IsKeppAssign { get; set; }

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
            param.Add("HeadlineId", HeadlineId);
            param.Add("PicId", PicId);
            param.Add("DirectAccount", DirectAccount);
            param.Add("IsKeepAssign", IsKeppAssign);

            param.Add("userid", UserId);
            param.Add("loginip", IP);
            param.Add("ErrorCode", 0);
            param.Add("ErrorMsg", string.Empty);

            return param;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_PICAssignment";
        }

        public override bool IsValid() => HeadlineId >= 0 &&
            !string.IsNullOrWhiteSpace(PicId) &&
            !string.IsNullOrWhiteSpace(DirectAccount);
    }
}