using System.Collections;
using Fanex.CRM.Common.Criteria;
using Fanex.CRM.Common.Helpers;
using Fanex.CRM.Common.Services;

namespace Fanex.CRM.Criteria.PicAndHeadline
{
    public class ChangePicStateCriteria : BasedCriteria
    {
        public int PicId { get; set; }

        public string Username { get; set; }

        public bool IsActive { get; set; }

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

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("PicId", PicId);
            param.Add("isactive", IsActive);

            param.Add("userid", UserId);
            param.Add("loginip", IP);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);

            return param;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_PICStatusUpd";
        }

        public override bool IsValid() => true;
    }
}