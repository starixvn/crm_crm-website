namespace Fanex.CRM.Criteria.PicAndHeadline
{
    using System.Collections;
    using Fanex.CRM.Common.Criteria;

    public class ValidateAssignmentCriteria : BasedCriteria
    {
        public int HeadlineId { get; set; }

        public string PicId { get; set; }

        public string Username { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("HeadlineId", HeadlineId);
            param.Add("PicID", PicId);
            param.Add("Username", Username);
            param.Add("errcode", ErrCode);
            param.Add("errmsg", ErrMsg);

            return param;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_PICAssignmentValidation";
        }

        public override bool IsValid() => HeadlineId >= 0 &&
            !string.IsNullOrWhiteSpace(PicId) &&
            !string.IsNullOrWhiteSpace(Username);
    }
}