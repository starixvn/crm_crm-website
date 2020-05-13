using Fanex.CRM.Common.Criteria;
using Fanex.CRM.Common.Helpers;
using Fanex.CRM.Common.Services;
using Fanex.CRM.PicAndHeadline.Models;
using Fanex.Data.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.CRM.PicAndHeadline.Criteria
{
    public class EditPicCriteria : BasedCriteria
    {
        public int PicId { get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
        public IEnumerable<PicCommunicationTool> CommunicationTools { get; set; }
        public string Remark { get; set; }
        public bool IsActive { get; set; }

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
            param.Add("PicId", PicId);
            param.Add("Picname", Username);
            param.Add("RoleId", RoleId);
            param.Add("CommunicationTools", SerializerHelper.SerializeList(this.CommunicationTools));
            param.Add("Remark", Remark);
            param.Add("IsActive", IsActive);

            param.Add("userid", UserId);
            param.Add("loginip", IP);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_PicUpd";
        }

        public override bool IsValid() => true;
    }
}
