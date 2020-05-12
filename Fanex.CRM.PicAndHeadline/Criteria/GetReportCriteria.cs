using Fanex.CRM.Common.Criteria;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.CRM.PicAndHeadline.Criteria
{
    public class GetReportCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public int Role { get; set; }

        public int TotalRows { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_PicManagementSel";
        }

        public override bool IsValid() => true;
    }
}
