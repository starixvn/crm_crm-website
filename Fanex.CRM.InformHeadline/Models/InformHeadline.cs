// -----------------------------------------------------------------------
// <copyright file="InformHeadline.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InformHeadline
    {
        public int TitleId { get; set; }

        public string Title { get; set; }

        public DateTime CreatedDate { get; set; }

        public int Total { get; set; }

        public string Status { get; set; }

        public int Done { get; set; }

        public int RequestToCallBack { get; set; }

        public int NoNeedToCall { get; set; }

        public int NoReply { get; set; }

        public int Uncontactable { get; set; }

        public int InternalAccount { get; set; }

        public int NeverTouch { get; set; }
    }
}
