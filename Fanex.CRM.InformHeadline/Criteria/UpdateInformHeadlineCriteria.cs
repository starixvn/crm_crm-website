namespace Fanex.CRM.InformHeadline.Criteria
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.CRM.Common.Criteria;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.Data.Repository;

    public class UpdateInformHeadlineCriteria : UpdateCriteria
    {
        public string TitleId { get; set; }

        public string Title { get; set; }

        public string Currency { get; set; }

        public string Level { get; set; }

        public string Site { get; set; }

        public int TypeAccount { get; set; }

        public bool ChangeFilter { get; set; }

        public string Content { get; set; }

        public bool ExcludeIgnoredAccounts { get; set; }

        public byte IsVip { get; set; }

        [SpParam(Ignored = true)]
        public IList<UpdateInform> Informs { get; set; }

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable
            {
                { "titleid", TitleId },
                { "title", Title },
                { "Currency", Currency },
                { "Level", Level },
                { "Site", Site },
                { "TypeAccount", TypeAccount },
                { "ExcludeIgnored", ExcludeIgnoredAccounts },
                { "changefilter", ChangeFilter },
                { "Content", Content },
                { "UserId", UserId },
                { "ip", IP },
                { "IsVip", IsVip },

                { "errcode", ErrorCode },
                { "errmsg", ErrorMessage }
            };
            return param;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_InformHeadlineSearchUpd";
        }

        public override bool IsValid() => true;
    }
}