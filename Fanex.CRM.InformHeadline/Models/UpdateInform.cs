// -----------------------------------------------------------------------
// <copyright file="UpdateInform.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Models
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateInform
    {
        public int OldCustId { get; set; }

        public int NewCustId { get; set; }

        public int HeadlineId { get; set; }

        public int GroupId { get; set; }

        public string UserName { get; set; }

        public string OldUserName { get; set; }

        public string NewUserName { get; set; }

        public DateTime UpdatedTime { get; set; }

        public int InformHeadlineStatusId { get; set; }

        public string Remark { get; set; }

        public string Handler { get; set; }

        public int Extension { get; set; }

        public int ProductStatus { get; set; }

        public bool IsKeep { get; set; }

        public int Case { get; set; }
    }
}
