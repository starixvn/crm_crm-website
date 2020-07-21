namespace Fanex.CRM.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Criteria.PicAndHeadline;
    using Fanex.CRM.Models;
    using Fanex.CRM.Models.PicAndHeadline;
    using Fanex.CRM.Services.Interfaces;

    public class ChangeLogService : IChangeLogService
    {
        public BaseReport<ChangeLogInfo> GetHeadlineDAReport(GetHeadlineDALogReportCriteria criteria)
        {
            return new BaseReport<ChangeLogInfo>
            {
                DataList = new List<ChangeLogInfo>
                {
                    new ChangeLogInfo
                    {
                        LogId = 1050,
                        LogTime = DateTime.Now,
                        UserName = "S01",
                        ActionName = "Update the PIC of a headline",
                        OldValue = "Group headline: H01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        NewValue = "Group headline: S01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        EditByName = "StarixIT"
                    },
                    new ChangeLogInfo
                    {
                        LogId = 1049,
                        LogTime = DateTime.Now,
                        UserName = "S01",
                        ActionName = "Set customer to Credit Term Headline",
                        NewValue = "Set as group headline",
                        EditByName = "User01"
                    }
                },
                Totals = 2
            };
        }
    }
}