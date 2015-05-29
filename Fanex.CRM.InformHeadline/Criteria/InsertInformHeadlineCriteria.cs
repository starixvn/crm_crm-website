// -----------------------------------------------------------------------
// <copyright file="InsertInformHeadlineCriteria.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Criteria
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.CRM.Common.Criteria;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InsertInformHeadlineCriteria : UpdateCriteria
    {
        public string Title { get; set; }

        public string Currency { get; set; }

        public string Level { get; set; }

        public string Site { get; set; }

        public int TypeAccount { get; set; }

        [SpParam(Ignored = true)]
        public IList<UpdateInform> Informs { get; set; }

        /// <summary>
        /// Get SP parameters.
        /// </summary>
        /// <returns>Hash table parameters.</returns>
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("title", Title);
            param.Add("Currency", Currency);
            param.Add("Level", Level);
            param.Add("Site", Site);
            param.Add("TypeAccount", TypeAccount);
            param.Add("UserId", UserId);
            param.Add("ip", IP);

            param.Add("errcode", ErrorCode);
            param.Add("errmsg", ErrorMessage);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_InformHeadlineIns";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
