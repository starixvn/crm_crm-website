// --------------------------------------------------------------------------
// <copyright file="FeedbackSubjectTemplateInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Models.FeedbackSubjectManagement
{
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Feedback Subject Template Information.
    /// </summary>
    public class FeedbackSubjectTemplateInfo : BaseModel
    {
        public string TemplateContent { get; set; }
    }
}
