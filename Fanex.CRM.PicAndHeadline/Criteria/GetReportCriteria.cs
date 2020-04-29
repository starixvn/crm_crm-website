using Fanex.CRM.Common.Criteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.CRM.PicAndHeadline.Criteria
{
    public class GetReportCriteria : BasedCriteria
    {
        public string UserName { get; set; }

        public int RoleId { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
        {
            return string.Empty;
        }

        public override bool IsValid() => true;
    }
}
