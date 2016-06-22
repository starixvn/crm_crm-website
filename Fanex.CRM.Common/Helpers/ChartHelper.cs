// --------------------------------------------------------------------------
// <copyright file="ChartHelper.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Helpers
{
    using System;
    using Fanex.CRM.Infrastructure.Resources;

    /// <summary>
    /// Chart helper.
    /// </summary>
    public static class ChartHelper
    {
        public static string GetViewChartFromToDateTitle(string fromDate, string toDate)
        {
            return string.Equals(fromDate, toDate, StringComparison.OrdinalIgnoreCase) ?
                fromDate :
                string.Format(Labels.Message_ViewChart_FromToDate, fromDate, toDate);
        }
    }
}
