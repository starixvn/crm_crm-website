
namespace Fanex.CRM.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Fanex.CRM.Entities.ViewModel;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Service.Impl;
    using Fanex.CRM.Service.Interface;
    using Fanex.CRM.Entities;
    using System.Web.Mvc.Async;

    public class ReportsController : BaseController
    {
        IFeedbackService _feedbackService;

        public ReportsController(): this(null) { }
        public ReportsController(IFeedbackService feedbackService) {
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

        [HttpPost]
        public JsonResult GetCasinoDailyDetailReport(string fromDate, string toDate, bool fromCache, int itemPerPage, int currentPage)
        {           
            var result = new DailyComplaintsDTO();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.GetCasinoDailyDetailReport(fromDate, toDate, fromCache, itemPerPage, currentPage);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

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

        [HttpPost]
        public JsonResult GetIBCDailyDetailReport(string fromDate, string toDate, bool fromCache, int itemPerPage, int currentPage)
        {
            var result = new DailyComplaintsDTO();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.GetIBCDailyDetailReport(fromDate, toDate, fromCache, itemPerPage, currentPage);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            } 

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

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

        [HttpPost]
        public JsonResult GetLicenseeDailyDetailReport(string fromDate, string toDate, bool fromCache, int itemPerPage, int currentPage)
        {
            var result = new DailyComplaintsDTO();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.GetLicenseeDailyDetailReport(fromDate, toDate, fromCache, itemPerPage, currentPage);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

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
            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

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
            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

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
        
        [HttpPost]
        public JsonResult ExportIBCDetailReport(string fromDate, string toDate, string reportTitle, string email, bool isSendMail = false)
        {
            Result result = null;
            JsonResult jsonResult;
            try
            {
                _feedbackService.ExportExcelIBCDailyDetailReport(fromDate, toDate, reportTitle, email, isSendMail);
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

        [HttpPost]
        public JsonResult ExportLicenseeDetailReport(string fromDate, string toDate, string reportTitle, string email, bool isSendMail = false)
        {
            Result result = null;
            JsonResult jsonResult;
            try
            {
                _feedbackService.ExportExcelLicenseeDetailReport(fromDate, toDate, reportTitle, email, isSendMail);
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

        [HttpPost]
        public JsonResult ExportCasinoDetailReport(string fromDate, string toDate, string reportTitle, string email, bool isSendMail = false)
        {
            Result result = null;
            JsonResult jsonResult;
            try
            {
                _feedbackService.ExportExcelCasinoDailyDetailReport(fromDate, toDate, reportTitle, email, isSendMail);
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