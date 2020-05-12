using Fanex.CRM.Common.Criteria;
using Fanex.CRM.Common.Helpers;
using Fanex.CRM.Common.Services;
using Fanex.CRM.PicAndHeadline.Models;
using Fanex.Data.Repository;
using System.Collections;
using System.Collections.Generic;

namespace Fanex.CRM.PicAndHeadline.Criteria
{
    public class InsertPicCriteria : BasedCriteria
    {
        public string Username { get; set; }
        public int RoleId { get; set; }
        public IEnumerable<PicCommunicationTool> CommunicationTools { get; set; }
        public string Remark { get; set; }

        [SpParam(SqlParamName = "creatorid")]
        public int CreatorId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

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

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("Picname", Username);
            param.Add("RoleId", RoleId);
            param.Add("CommunicationTools", SerializerHelper.SerializeList(this.CommunicationTools));
            param.Add("Remark", Remark);

            param.Add("userid", UserId);
            param.Add("loginip", IP);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_PicIns";
        }

        public override bool IsValid() => true;
    }
}
