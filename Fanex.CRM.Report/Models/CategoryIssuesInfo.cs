// --------------------------------------------------------------------------
// <copyright file="CategoryIssuesInfo.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Category Issues Information.
    /// </summary>
    [Serializable]
    public class CategoryIssuesInfo : BaseModel
    {
        public CategoryIssuesInfo()
        {
        }

        public CategoryIssuesInfo(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public List<int> NumberOfIssueList { get; set; }

        public int NumberOfIssueGrandTotal
        {
            get
            {
                var numberOfIssueLength = NumberOfIssueList.Count;
                var grandTotal = 0;
                for (var i = 0; i < numberOfIssueLength; i++)
                {
                    grandTotal += NumberOfIssueList[i];
                }

                return grandTotal;
            }
        }

        public List<CategoryIssuesInfo> ChildReports { get; set; }

        public static void SumNumberOfIssueList(CategoryIssuesInfo fromInfo, CategoryIssuesInfo toInfo)
        {
            var numberOfIssueLength = fromInfo.NumberOfIssueList.Count;
            for (var i = 0; i < numberOfIssueLength; i++)
            {
                toInfo.NumberOfIssueList[i] += fromInfo.NumberOfIssueList[i];
            }
        }
    }
}
