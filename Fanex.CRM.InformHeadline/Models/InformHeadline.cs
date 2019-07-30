namespace Fanex.CRM.InformHeadline.Models
{
    using System;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;

    public class InformHeadline
    {
        public int TitleId { get; set; }

        public string Title { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ClientCreatedDate
        {
            get
            {
                return DateTimeUtilize.ChangeTimeZoneToClientDateTime(CreatedDate).ToShortFormatedDate();
            }
        }

        public int Total { get; set; }

        public string Status { get; set; }

        public int Done { get; set; }

        public int RequestToCallBack { get; set; }

        public int NoNeedToCall { get; set; }

        public int NoReply { get; set; }

        public int Uncontactable { get; set; }

        public int InternalAccount { get; set; }

        public int NeverTouch { get; set; }

        public int KeptForReference { get; set; }
    }
}