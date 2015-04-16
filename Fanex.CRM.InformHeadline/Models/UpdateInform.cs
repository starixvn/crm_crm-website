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
        public int HeadlineId { get; set; }

        public int GroupId { get; set; }

        public string UserName { get; set; }

        public DateTime UpdatedTime { get; set; }

        public int InformHeadlineStatusId { get; set; }

        public string Remark { get; set; }

        public string Handler { get; set; }

        public int Extension { get; set; }

        public int ProductStatus { get; set; }

        public int Case { get; set; }
    }
}
