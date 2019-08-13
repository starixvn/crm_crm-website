namespace Fanex.CRM.Remark.Criteria
{
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    public class UpdateRemarkCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public string Remarks { get; set; }

        public int UserId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        public string IP
        {
            get
            {
                return IPHelper.ClientIP;
            }
        }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_CustomerRemarkUpd";
        }

        public override bool IsValid() => true;
    }
}