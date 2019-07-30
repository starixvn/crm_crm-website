namespace Fanex.CRM.InformHeadline.Criteria
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.CRM.Common.Criteria;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.Data.Repository;

    public class InsertInformHeadlineCriteria : UpdateCriteria
    {
        public string Title { get; set; }

        public string Currency { get; set; }

        public string Level { get; set; }

        public string Site { get; set; }

        public int TypeAccount { get; set; }

        public bool IsVip { get; set; }

        public bool ExcludeIgnoredAccounts { get; set; }

        [SpParam(Ignored = true)]
        public IList<UpdateInform> Informs { get; set; }

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("title", Title);
            param.Add("Currency", Currency);
            param.Add("Level", Level);
            param.Add("Site", Site);
            param.Add("TypeAccount", TypeAccount);
            param.Add("ExcludeIgnored", ExcludeIgnoredAccounts);
            param.Add("UserId", UserId);
            param.Add("IsVip", IsVip);
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