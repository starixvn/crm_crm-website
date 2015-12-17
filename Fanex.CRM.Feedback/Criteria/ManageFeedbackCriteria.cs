// --------------------------------------------------------------------------
// <copyright file="ManageFeedbackCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Criteria
{
    using System;
    using System.Collections;
    using Fanex.CRM.Common.Helpers;
    using Fanex.Data.Repository;

    /// <summary>
    /// ManageFeedback Criteria.
    /// </summary>
    public class ManageFeedbackCriteria : CriteriaBase
    {
        public int? FeedbackId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Username { get; set; }

        public int IsGuest { get; set; }

        public int CommunicationId { get; set; }

        public int MainCategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public int StatusId { get; set; }

        public int IsReviewed { get; set; }

        public int ResponseId { get; set; }

        public string HandledBy { get; set; }

        public int PageIndex { get; set; }

        public int ItemsPerPage { get; set; }

        public int TotalRows { get; set; }

        public override Hashtable GetSpParams()
        {
            var inputParams = new Hashtable();
            inputParams.Add("fromdate", DateTimeUtilize.ChangeClientFromTimeToServerTime(FromDate));
            inputParams.Add("todate", DateTimeUtilize.ChangeClientToTimeToServerTime(ToDate));
            if (FeedbackId.HasValue && FeedbackId.Value > 0)
            {
                inputParams.Add("feedbackid", FeedbackId.Value);
            }

            inputParams.Add("username", Username);
            if (IsGuest >= 0)
            {
                inputParams.Add("isguest", IsGuest == 1);
            }

            if (CommunicationId > 0)
            {
                inputParams.Add("communicationid", CommunicationId);
            }

            if (MainCategoryId > 0)
            {
                inputParams.Add("maincategoryid", MainCategoryId);
            }

            if (SubCategoryId > 0)
            {
                inputParams.Add("subcategoryid", SubCategoryId);
            }

            if (StatusId > 0)
            {
                inputParams.Add("statusid", StatusId);
            }

            if (ResponseId > 0)
            {
                inputParams.Add("responseid", ResponseId);
            }

            //// 0 : All
            //// 1 : Reviewed
            //// -1: Not reviewed
            if (IsReviewed == 1)
            {
                inputParams.Add("isreviewed", true);
            }
            else if (IsReviewed == -1)
            {
                inputParams.Add("isreviewed", false);
            }

            inputParams.Add("handledby", string.IsNullOrWhiteSpace(HandledBy) ? string.Empty : HandledBy);
            inputParams.Add("pageindex", PageIndex);
            inputParams.Add("itemsperpage", ItemsPerPage);
            inputParams.Add("totalrows", 0);

            return inputParams;
        }

        public override string GetSettingKey()
        {
            return "ManageFeedbackList";
        }
    }
}
