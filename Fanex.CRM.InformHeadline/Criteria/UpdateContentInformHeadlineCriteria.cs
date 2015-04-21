// -----------------------------------------------------------------------
// <copyright file="UpdateContentInformHeadlineCriteria.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Criteria
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.CRM.Common.Criteria;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateContentInformHeadlineCriteria : UpdateCriteria
    {
        public int TitleId { get; set; }

        public string Content { get; set; }

        [SpParam(Ignored = true)]
        public IList<UpdateInform> Informs { get; set; }

        /// <summary>
        /// Get SP parameters.
        /// </summary>
        /// <returns>Hash table parameters.</returns>
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("titleid", TitleId);            
            param.Add("UserId", UserId);
            param.Add("Content", Content);
            param.Add("ip", IP);

            param.Add("errcode", ErrorCode);
            param.Add("errmsg", ErrorMessage);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_InformHeadlineUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
