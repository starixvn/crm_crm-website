// --------------------------------------------------------------------------
// <copyright file="IFeedbackService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Service.Interface
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Entities.ViewModel;

    public interface IFeedbackService
    {
        Feedback GetById(int id);

        Result AddGuestFeedback(Feedback feedback);

        Result AddFeedback(Feedback feedback);

        Result EditFeedback(Feedback feedback);

        DataGridViewDTO SelFeedbackByCustId(int custId, string fromDate, string toDate, int pageIndex, int pageSize, bool fromCache);

        DataGridViewDTO GetFeedback(
            string fromDate,
            string toDate,
            string username,
            int isGuest,
            int communicationId,
            int mainCategoryId,
            int subCategoryId,
            int statusId,
            int isReviewed,
            int responseId,
            int pageIndex,
            int pageSize,
            bool fromCache);

        FeedbackStatisticsDTO GetFeedbackStatistic(Params param);

        DataGridViewDTO GetFeedbackStatisticDetail(Params param);

        DataGridViewDTO GetDeletedFeedback(
            string fromDate,
            string toDate, 
            string userName,
            int isGuest, 
            int communicationId, 
            int mainCategoryId,
            int subCategoryId, 
            int isReviewed, 
            int responseId,
            int pageIndex, 
            int pageSize, 
            bool fromCache);

        DataGridViewDTO SelTopFeedbackByCustId(int custId, int selectTop, int pageIndex, int pageSize, bool fromCache);

        void ExportExcelFeedbackStatisticDetail(
            int type, 
            int valueId,
            int mainCategoryId,
            int subCategoryId,
            string fromDate,
            string toDate,
            string reportTitle);

        void ExportExcelFeedbackStatistic(int type, string fromDate, string toDate, string reportTitle);

        DataGridViewDTO SelTopFeedback(int selectTop, int pageIndex, int pageSize, bool fromCache);

        Result AllowExportFeedbackStatisticDetail();

        DailySummaryBO GetIBCDailySummaryReport(string fromDate, string toDate);

        DailyComplaintsDTO GetIBCDailyDetailReport(string fromDate, string toDate, bool fromCache, int itemPerPage, int currentPage);

        void ExportExcelIBCDailySummaryReport(string fromDate, string toDate, string reportTitle);

        void ExportExcelIBCDailyDetailReport(string fromDate, string toDate, string reportTitle, string email, bool isSendMail);

        DailySummaryBO GetLicenseeDailySummaryReport(string fromDate, string toDate);

        DailyComplaintsDTO GetLicenseeDailyDetailReport(string fromDate, string toDate, bool fromCache, int itemPerPage, int currentPage);

        void ExportExcelLicenseeDailySummaryReport(string fromDate, string toDate, string reportTitle);

        void ExportExcelLicenseeDetailReport(string fromDate, string toDate, string reportTitle, string email, bool isSendMail);

        CasinoDailySummary GetCasinoDailySummaryReport(string fromDate, string toDate);

        DailyComplaintsDTO GetCasinoDailyDetailReport(string fromDate, string toDate, bool fromCache, int itemPerPage, int currentPage);

        void ExportExcelCasinoDailySummaryReport(string fromDate, string toDate, string reportTitle);

        void ExportExcelCasinoDailyDetailReport(string fromDate, string toDate, string reportTitle, string email, bool isSendMail);

        DailySummaryBO GetCasinoWeeklySummaryReport(string fromDate, string toDate);

        void ExportExcelCasinoWeeklySummaryReport(string fromDate, string toDate, string reportTitle);

        List<FeedbackTemplate> GetFeedbackSubjectTemplateItems(int feedback = 0);

        List<FeedbackTemplate> GetFeedbackSubjectTemplateItemsRange(int range);

        int GetNumOfFeedbackSubjectTemplateItem();

        Result AddSubjectFeedbackTemplate(string subject);

        Result EditSubjectFeedbackTemplate(int id, string newSubject);

        Result DeleteSubjectFeedbackTemplate(int id);

        FeedbackTemplateDGViewDTO SelFeedbackTemplatePageWithSearch(string searchString, int pageIndex, int pageSize);

        Result AllowExportIbcStatisticDetail();

        Result AllowExportCasinoStatisticDetail();

        Result AllowExportLicenseeStatisticDetail();

        DataTable GetAutoSuggestionCustomer(string userName);

        void ExportExcelAllFeedbackStatisticsDetail(
            int type,
            string fromDate,
            string toDate,
            string reportTitle);
    }
}
