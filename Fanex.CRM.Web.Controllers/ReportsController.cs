// --------------------------------------------------------------------------
// <copyright file="ReportsController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Entities.ViewModel;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Service.Impl;
    using Fanex.CRM.Service.Interface;

    /// <summary>
    /// Reports Controller.
    /// </summary>
    public class ReportsController : BaseController
    {
        private IFeedbackService _feedbackService;

        public ReportsController()
            : this(null)
        {
        }

        public ReportsController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService ?? new FeedbackService();
        }

        #region Views

        public ActionResult CasinoDailyStatistics()
        {
            return View();
        }

        public ActionResult CasinoWeeklyStatistics()
        {
            return View();
        }

        public ActionResult DailyStatistic()
        {
            return View();
        }

        public ActionResult WeeklyStatistic()
        {
            return View();
        }

        public ActionResult FeedbackStatistics()
        {
            return View();
        }

        public ActionResult FeedbackStatisticDetail()
        {
            return View();
        }

        #endregion
        #region Post Actions

        /// <summary>
        /// Gets the casino daily summary report.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <returns>The casino daily summary report.</returns>
        [HttpPost]
        public JsonResult GetCasinoDailySummaryReport(string fromDate, string toDate)
        {
            var result = new CasinoDailySummary();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.GetCasinoDailySummaryReport(fromDate, toDate);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Gets the casino daily detail report.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date  value.</param>
        /// <param name="fromCache">If set to <c>true</c> [from cache].</param>
        /// <param name="itemPerPage">The item per page.</param>
        /// <param name="currentPage">The current page.</param>
        /// <returns>The casino daily detail report.</returns>
        [HttpPost]
        public JsonResult GetCasinoDailyDetailReport(
            string fromDate,
            string toDate,
            bool fromCache,
            int itemPerPage,
            int currentPage)
        {
            var result = new DailyComplaintsDTO();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.GetCasinoDailyDetailReport(
                    fromDate,
                    toDate,
                    fromCache,
                    itemPerPage,
                    currentPage);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(new
            {
                DailyComplaints = result.DailyComplaints,
                pageindex = result.Pageindex,
                total = result.TotalItems
            });

            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Allows the export casino statistic detail.
        /// </summary>
        /// <returns>Allow result.</returns>
        [HttpPost]
        public JsonResult AllowExportCasinoStatisticDetail()
        {
            var result = new Result();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.AllowExportCasinoStatisticDetail();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Gets the casino weekly summary report.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <returns>The casino weekly summary report.</returns>
        [HttpPost]
        public JsonResult GetCasinoWeeklySummaryReport(string fromDate, string toDate)
        {
            var result = new DailySummaryBO();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.GetCasinoWeeklySummaryReport(fromDate, toDate);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Gets the IBC daily summary report.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <returns>The IBC daily summary report.</returns>
        [HttpPost]
        public JsonResult GetIBCDailySummaryReport(string fromDate, string toDate)
        {
            var result = new DailySummaryBO();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.GetIBCDailySummaryReport(fromDate, toDate);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Gets the licensee daily summary report.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <returns>The licensee daily summary report.</returns>
        [HttpPost]
        public JsonResult GetLicenseeDailySummaryReport(string fromDate, string toDate)
        {
            var result = new DailySummaryBO();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.GetLicenseeDailySummaryReport(fromDate, toDate);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Gets the IBC daily detail report.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="fromCache">If set to <c>true</c> [from cache].</param>
        /// <param name="itemPerPage">The item per page.</param>
        /// <param name="currentPage">The current page.</param>
        /// <returns>The IBC daily detail report.</returns>
        [HttpPost]
        public JsonResult GetIBCDailyDetailReport(
            string fromDate,
            string toDate,
            bool fromCache,
            int itemPerPage,
            int currentPage)
        {
            var result = new DailyComplaintsDTO();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.GetIBCDailyDetailReport(
                    fromDate,
                    toDate,
                    fromCache,
                    itemPerPage,
                    currentPage);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(new
            {
                DailyComplaints = result.DailyComplaints,
                pageindex = result.Pageindex,
                total = result.TotalItems
            });

            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Allows the export IBC statistic detail.
        /// </summary>
        /// <returns>Allow result.</returns>
        [HttpPost]
        public JsonResult AllowExportIbcStatisticDetail()
        {
            var result = new Result();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.AllowExportIbcStatisticDetail();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Gets the licensee daily detail report.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="fromCache">If set to <c>true</c> [from cache].</param>
        /// <param name="itemPerPage">The item per page.</param>
        /// <param name="currentPage">The current page.</param>
        /// <returns>The licensee daily detail report.</returns>
        [HttpPost]
        public JsonResult GetLicenseeDailyDetailReport(
            string fromDate,
            string toDate,
            bool fromCache,
            int itemPerPage,
            int currentPage)
        {
            var result = new DailyComplaintsDTO();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.GetLicenseeDailyDetailReport(
                    fromDate,
                    toDate,
                    fromCache,
                    itemPerPage,
                    currentPage);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(new
            {
                DailyComplaints = result.DailyComplaints,
                pageindex = result.Pageindex,
                total = result.TotalItems
            });

            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Allows the export licensee statistic detail.
        /// </summary>
        /// <returns>Allow result.</returns>
        [HttpPost]
        public JsonResult AllowExportLicenseeStatisticDetail()
        {
            var result = new Result();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.AllowExportLicenseeStatisticDetail();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Gets the feedback statistic.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>The feedback statistic.</returns>
        [HttpPost]
        public JsonResult GetFeedbackStatistic(Params param)
        {
            var result = new FeedbackStatisticsDTO();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.GetFeedbackStatistic(param);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(new
            {
                ColumnNames = result.ColumnNames,
                ColumnIDs = result.ColumnIDs,
                Total = result.Total,
                PageIndex = result.PageIndex,
                rows = result.Rows,
                GrandTotal = result.GrandTotal
            });

            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Gets the feedback statistic detail.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>The feedback statistic detail.</returns>
        [HttpPost]
        public JsonResult GetFeedbackStatisticDetail(Params param)
        {
            var result = new DataGridViewDTO();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.GetFeedbackStatisticDetail(param);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(new
            {
                pageindex = result.PageIndex,
                total = result.TotalItems,
                rows = result.Rows
            });
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Allows the export feedback statistic detail.
        /// </summary>
        /// <returns>Allow result.</returns>
        [HttpPost]
        public JsonResult AllowExportFeedbackStatisticDetail()
        {
            var result = new Result();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.AllowExportFeedbackStatisticDetail();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Exports the IBC detail report.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="reportTitle">The report title.</param>
        /// <param name="email">The email.</param>
        /// <param name="isSendMail">If set to <c>true</c> [is send mail].</param>
        /// <returns>The IBC detail report.</returns>
        [HttpPost]
        public JsonResult ExportIBCDetailReport(
            string fromDate,
            string toDate,
            string reportTitle,
            string email,
            bool isSendMail = false)
        {
            Result result = null;
            JsonResult jsonResult;
            try
            {
                _feedbackService.ExportExcelIBCDailyDetailReport(
                    fromDate,
                    toDate,
                    reportTitle,
                    email,
                    isSendMail);
                result = new Result()
                {
                    ErrCode = 0,
                    ErrMsg = "Successfully"
                };
            }
            catch (Exception ex)
            {
                result = new Result()
                {
                    ErrCode = 100,
                    ErrMsg = ex.Message
                };

                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Exports the licensee detail report.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="reportTitle">The report title.</param>
        /// <param name="email">The email.</param>
        /// <param name="isSendMail">If set to <c>true</c> [is send mail].</param>
        /// <returns>The licensee detail report.</returns>
        [HttpPost]
        public JsonResult ExportLicenseeDetailReport(
            string fromDate,
            string toDate,
            string reportTitle,
            string email,
            bool isSendMail = false)
        {
            Result result = null;
            JsonResult jsonResult;
            try
            {
                _feedbackService.ExportExcelLicenseeDetailReport(
                    fromDate,
                    toDate,
                    reportTitle,
                    email,
                    isSendMail);
                result = new Result()
                {
                    ErrCode = 0,
                    ErrMsg = "Successfully"
                };
            }
            catch (Exception ex)
            {
                result = new Result()
                {
                    ErrCode = 100,
                    ErrMsg = ex.Message
                };

                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Exports the casino detail report.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="reportTitle">The report title.</param>
        /// <param name="email">The email.</param>
        /// <param name="isSendMail">If set to <c>true</c> [is send mail].</param>
        /// <returns>The casino detail report.</returns>
        [HttpPost]
        public JsonResult ExportCasinoDetailReport(
            string fromDate,
            string toDate,
            string reportTitle,
            string email,
            bool isSendMail = false)
        {
            Result result = null;
            JsonResult jsonResult;
            try
            {
                _feedbackService.ExportExcelCasinoDailyDetailReport(
                    fromDate,
                    toDate,
                    reportTitle,
                    email,
                    isSendMail);
                result = new Result()
                {
                    ErrCode = 0,
                    ErrMsg = "Successfully"
                };
            }
            catch (Exception ex)
            {
                result = new Result()
                {
                    ErrCode = 100,
                    ErrMsg = ex.Message
                };

                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        #endregion
    }
}