namespace Fanex.CRM.Communication.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CRM.Common.Criteria;

    public class UpdateMultipleLogsCriteria : UpdateCriteria
    {
        public override string GetSettingKey()
        {
            return "MultipleLogsUpdate";
        }
    }
}
