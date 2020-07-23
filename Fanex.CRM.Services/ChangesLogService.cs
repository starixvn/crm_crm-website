namespace Fanex.CRM.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Criteria.PicAndHeadline;
    using Fanex.CRM.Models;
    using Fanex.CRM.Models.PicAndHeadline;
    using Fanex.CRM.Services.Interfaces;

    public class ChangesLogService : IChangesLogService
    {
        public BaseReport<ChangesLogInfo> GetHeadlineDAReport(GetHeadlineDALogReportCriteria criteria)
        {
            var dataList = new List<ChangesLogInfo>
                {
                    new ChangesLogInfo
                    {
                        LogId = 1050,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Update the PIC of a headline",
                        OldValue = "Group headline: H01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        NewValue = "Group headline: S01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        EditByName = "StarixIT"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1049,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Set customer to Credit Term Headline",
                        NewValue = "Set as group headline",
                        EditByName = "User01"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1050,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Update the PIC of a headline",
                        OldValue = "Group headline: H01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        NewValue = "Group headline: S01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        EditByName = "StarixIT"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1049,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Set customer to Credit Term Headline",
                        NewValue = "Set as group headline",
                        EditByName = "User01"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1050,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Update the PIC of a headline",
                        OldValue = "Group headline: H01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        NewValue = "Group headline: S01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        EditByName = "StarixIT"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1049,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Set customer to Credit Term Headline",
                        NewValue = "Set as group headline",
                        EditByName = "User01"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1050,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Update the PIC of a headline",
                        OldValue = "Group headline: H01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        NewValue = "Group headline: S01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        EditByName = "StarixIT"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1049,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Set customer to Credit Term Headline",
                        NewValue = "Set as group headline",
                        EditByName = "User01"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1050,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Update the PIC of a headline",
                        OldValue = "Group headline: H01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        NewValue = "Group headline: S01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        EditByName = "StarixIT"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1049,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Set customer to Credit Term Headline",
                        NewValue = "Set as group headline",
                        EditByName = "User01"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1050,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Update the PIC of a headline",
                        OldValue = "Group headline: H01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        NewValue = "Group headline: S01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        EditByName = "StarixIT"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1049,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Set customer to Credit Term Headline",
                        NewValue = "Set as group headline",
                        EditByName = "User01"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1050,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Update the PIC of a headline",
                        OldValue = "Group headline: H01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        NewValue = "Group headline: S01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        EditByName = "StarixIT"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1049,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Set customer to Credit Term Headline",
                        NewValue = "Set as group headline",
                        EditByName = "User01"
                    }
                };

            return new BaseReport<ChangesLogInfo>
            {
                DataList = dataList.Skip((criteria.PageIndex - 1) * criteria.PageSize).Take(criteria.PageSize),
                Totals = 14
            };
        }

        public BaseReport<ChangesLogInfo> GetPicReport(GetPicLogReportCriteria criteria)
        {
            var dataList = new List<ChangesLogInfo>
                {
                    new ChangesLogInfo
                    {
                        LogId = 1050,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Update the PIC of a headline",
                        OldValue = "Group headline: H01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        NewValue = "Group headline: S01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        EditByName = "StarixIT"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1049,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Set customer to Credit Term Headline",
                        NewValue = "Set as group headline",
                        EditByName = "User01"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1050,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Update the PIC of a headline",
                        OldValue = "Group headline: H01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        NewValue = "Group headline: S01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        EditByName = "StarixIT"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1049,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Set customer to Credit Term Headline",
                        NewValue = "Set as group headline",
                        EditByName = "User01"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1050,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Update the PIC of a headline",
                        OldValue = "Group headline: H01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        NewValue = "Group headline: S01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        EditByName = "StarixIT"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1049,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Set customer to Credit Term Headline",
                        NewValue = "Set as group headline",
                        EditByName = "User01"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1050,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Update the PIC of a headline",
                        OldValue = "Group headline: H01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        NewValue = "Group headline: S01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        EditByName = "StarixIT"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1049,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Set customer to Credit Term Headline",
                        NewValue = "Set as group headline",
                        EditByName = "User01"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1050,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Update the PIC of a headline",
                        OldValue = "Group headline: H01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        NewValue = "Group headline: S01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        EditByName = "StarixIT"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1049,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Set customer to Credit Term Headline",
                        NewValue = "Set as group headline",
                        EditByName = "User01"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1050,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Update the PIC of a headline",
                        OldValue = "Group headline: H01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        NewValue = "Group headline: S01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        EditByName = "StarixIT"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1049,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Set customer to Credit Term Headline",
                        NewValue = "Set as group headline",
                        EditByName = "User01"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1050,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Update the PIC of a headline",
                        OldValue = "Group headline: H01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        NewValue = "Group headline: S01\nPIC: Boss1 (Boss), PIC1 (PIC)",
                        EditByName = "StarixIT"
                    },
                    new ChangesLogInfo
                    {
                        LogId = 1049,
                        LogTime = DateTime.Now.ToString("MM-dd-yyyy"),
                        Name = "S01",
                        ActionName = "Set customer to Credit Term Headline",
                        NewValue = "Set as group headline",
                        EditByName = "User01"
                    }
                };

            return new BaseReport<ChangesLogInfo>
            {
                DataList = dataList.Skip((criteria.PageIndex - 1) * criteria.PageSize).Take(criteria.PageSize),
                Totals = 14
            };
        }
    }
}