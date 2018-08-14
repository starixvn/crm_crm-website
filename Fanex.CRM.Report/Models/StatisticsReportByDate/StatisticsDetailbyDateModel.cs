namespace Fanex.CRM.Report.Models.StatisticsReportByDate
{
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Helpers;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class StatisticsDetailbyDateModel
    {
        public StatisticsDetailbyDateModel()
        {
            Phones = new List<Phone>();
            PICs = new List<PersonalInfoContact>();
        }

        #region Communication

        public int FeedbackID { get; set; }

        public List<Phone> Phones { get; set; }

        public List<PersonalInfoContact> PICs { get; set; }

        public Fax Fax { get; set; }

        public Phone MobilePhone { get; set; }

        public string Skype { get; set; }

        public string Email { get; set; }

        public string MSN { get; set; }

        public string WEChat { get; set; }

        public string WhatsApp { get; set; }

        public string Line { get; set; }

        public string Yahoo { get; set; }

        public string QQ { get; set; }

        public string Bbm { get; set; }

        #endregion

        public DateTime StartDate { get; set; }

        public string StartDateString
        {
            get { return DateTimeUtilize.ChangeTimeZoneToClientDateTime(StartDate).ToShortFormatedDate(); }
        }

        public string StartTime
        {
            get { return DateTimeUtilize.ChangeTimeZoneToClientDateTime(StartDate).ToHourMinuteMiddayTypeFormated(); }
        }

        public string EndTime { get; set; }

        public TimeInfo Duration
        {
            get
            {
                var feedbackDate = DateTimeUtilize.ChangeTimeZoneToClientDateTime(StartDate);
                var resolutionDate = DateTimeUtilize.ChangeTimeZoneToClientDateTime(ResolutionTime);
                var timeSpan = resolutionDate - feedbackDate;

                return new TimeInfo
                {
                    Days = timeSpan.Days,
                    Hours = timeSpan.Hours,
                    Minutes = timeSpan.Minutes
                };
            }
        }

        public string Username { get; set; }

        public Setting SiteInfo { get; set; }

        public Setting CommunicationTool { get; set; }

        public string Role { get; set; }

        public string Contact
        {
            get
            {
                BaseCustomer customer = new BaseCustomer
                {
                    PhoneInfo = Phones,
                    PersonalInfoContacts = PICs,
                    FaxInfo = Fax,
                    MobilePhone = MobilePhone,
                    Skype = Skype,
                    Email = Email,
                    MSN = MSN,
                    WeChat = WEChat,
                    WhatsApp = WhatsApp,
                    Line = Line,
                    Yahoo = Yahoo,
                    QQ = QQ,
                    BBM = Bbm
                };

                return FieldHelper.GenerateContactInfo(customer);
            }
        }

        public Setting Extension { get; set; }

        public Setting Product { get; set; }

        public Setting SubProduct { get; set; }

        public Country Country { get; set; }

        public Setting MainCategory { get; set; }

        public Setting SubCategory { get; set; }

        public string Remark { get; set; }

        public string AutoField { get; set; }

        public string RemarkText { get; set; }

        public string Solution { get; set; }

        public string StatusName { get; set; }

        public Setting Response { get; set; }

        public string Handler { get; set; }

        public DateTime CreationTime { get; set; }

        public string CreationTimeString
        {
            get { return DateTimeUtilize.ChangeTimeZoneToClient(CreationTime); }
        }

        public DateTime ResolutionTime { get; set; }

        public string ResolutionTimeString
        {
            get { return DateTimeUtilize.ChangeTimeZoneToClientDateTime(ResolutionTime).ToHourMinuteMiddayTypeFormated(); }
        }

        public int ProductID { get; set; }

        public int SubProductID { get; set; }

        public int CategoryID { get; set; }

        public int SubCategoryID { get; set; }

        public int CommunicationID { get; set; }

        public int StationID { get; set; }
    }
}
