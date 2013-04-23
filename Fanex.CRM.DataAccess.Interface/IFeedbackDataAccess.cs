
namespace Fanex.CRM.DataAccess.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.CRM.Entities.ViewModel;
    //using Fanex.CRM.Infrastructure.Repository;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Entities;

    public interface IFeedbackDataAccess
    {
        Feedback GetById(int Id);
        bool AddGuestFeedback(Feedback entity, string Ip, ref Result result);
        bool AddFeedback(Feedback entity, string Ip, ref Result result);
        bool EditFeedback(Feedback entity, int UserId, string Ip, ref Result result);
        IEnumerable<Feedback> GetFeedbackByCustId(int custId, DateTime fromDate, DateTime toDate);
        IEnumerable<Feedback> GetFeedback(DateTime fromDate, DateTime toDate,string userName,int isGuest, int communicationID, int mainCategoryID, int subCategoryID, int statusID, int isReviewed, int responseId);
        DataTableCollection GetFeedbackStatistic(Params param);
        IEnumerable<Feedback> GetFeedbackStatisticDetail(Params param, ref int totalRows);
        IEnumerable<Feedback> GetDeletedFeedback(DateTime fromDate, DateTime toDate, string userName, int isGuest, int communicationID, int mainCategoryID, int subCategoryID, int isReviewed, int responseId);
        IEnumerable<Feedback> SelTopFeedbackByCustId(int custId, int selectTop);
        IEnumerable<Feedback> SelTopFeedback(int selectTop);

        DataSet GetIBCDailySummaryReport(DateTime fromDate, DateTime toDate, Enumerations.ReportTypes siteType);
        DataSet GetIBCDailyDetailReport(DateTime fromDate, DateTime toDate, Enumerations.ReportTypes siteType);
        DataSet GetLicenseeWeeklySummaryReport(DateTime fromDate, DateTime toDate);
        DataSet GetLicenseeWeeklyDetailReport(DateTime fromDate, DateTime toDate);
        DataSet GetCasinoDailySummaryReport(DateTime fromDate, DateTime toDate, int reportType);
        DataSet GetCasinoDailyDetailReport(DateTime fromDate, DateTime toDate);
        DataSet GetFeedbackSubjectTemplateItems(int templateId = 0);

        Result AddSubjectFeedbackTemplate(string subject);
        Result EditSubjectFeedbackTemplate(int id, string newSubject);
        Result DeleteSubjectFeedbackTemplate(int id);

        DataSet AutoSuggestionCustomer(string userName);
    }

}
