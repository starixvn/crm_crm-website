namespace Fanex.CRM.Criteria.PicAndHeadline
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.CRM.Common.Criteria;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Models.PicAndHeadline;

    public class ValidateEditPicCriteria : BasedCriteria
    {
        public int PicId { get; set; }

        public IEnumerable<PicCommunicationTool> CommunicationTools { get; set; }

        public string Message { get; set; }

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("PicId", PicId);
            param.Add("CommunicationTools", SerializerHelper.SerializeList(CommunicationTools));
            param.Add("Message", string.Empty);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_PicUpdValidation";
        }

        public override bool IsValid() => PicId > 0;
    }
}