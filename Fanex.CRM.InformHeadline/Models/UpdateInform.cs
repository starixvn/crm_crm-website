namespace Fanex.CRM.InformHeadline.Models
{
    using System;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;

    public class UpdateInform
    {
        private const int NeverTouchStatusId = 45006;

        private int _informHeadlineStatusId;

        public int OldCustId { get; set; }

        public int NewCustId { get; set; }

        public int HeadlineId { get; set; }

        public int GroupId { get; set; }

        public string UserName { get; set; }

        public string OldUserName { get; set; }

        public string NewUserName { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public string ClientUpdatedTime
        {
            get
            {
                return UpdatedTime.HasValue
                    ? DateTimeUtilize.ChangeTimeZoneToClientDateTime(UpdatedTime.Value).ToFullFormatedDateTime() : string.Empty;
            }
        }

        public int InformHeadlineStatusId
        {
            get
            {
                return _informHeadlineStatusId == 0 ? NeverTouchStatusId : _informHeadlineStatusId;
            }

            set
            {
                _informHeadlineStatusId = value;
            }
        }

        public string Remark { get; set; }

        public string Handler { get; set; }

        public int Extension { get; set; }

        public int ProductStatus { get; set; }

        public bool IsKeep { get; set; }

        public int Case { get; set; }
    }
}