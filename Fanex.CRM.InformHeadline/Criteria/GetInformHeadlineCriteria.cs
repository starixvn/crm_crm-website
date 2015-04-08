// -----------------------------------------------------------------------
// <copyright file="GetInformHeadlineCriteria.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetInformHeadlineCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Acc_CRM_InformHeadlineList";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
