// --------------------------------------------------------------------------
// <copyright file="ManageFeedbackViewModel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.ViewModels
{
    /// <summary>
    /// ManageFeedback ViewModel.
    /// </summary>
    public class ManageFeedbackViewModel
    {
        public int? CustId { get; set; }

        public string CustName { get; set; }

        public int? IsGuest { get; set; }
    }
}
