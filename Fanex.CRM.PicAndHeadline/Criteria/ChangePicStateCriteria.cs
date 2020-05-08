using Fanex.CRM.Common.Criteria;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.CRM.PicAndHeadline.Criteria
{
    public class ChangePicStateCriteria : BasedCriteria
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public bool IsActive { get; set; }

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("Id", Id);
            param.Add("UserRole", UserRole);
            param.Add("isactive", IsActive);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            return param;
        }

        public override string GetSettingKey()
        {
            return "";
        }

        public override bool IsValid() => true;
    }
}
