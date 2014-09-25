// --------------------------------------------------------------------------
// <copyright file="FeedbackSubjectTemplateView.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Models.FeedbackSubjectManagement
{
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Feedback Subject Template View.
    /// </summary>
    public class FeedbackSubjectTemplateView : BaseModel
    {
        public List<FeedbackSubjectTemplateInfo> Templates { get; set; }

        public int TotalTemplates { get; set; }
    }
}
