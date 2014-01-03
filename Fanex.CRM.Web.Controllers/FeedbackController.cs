
namespace Fanex.CRM.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Service.Impl;
    using Fanex.CRM.Service.Interface;
    using Fanex.CRM.Entities.ViewModel;
    using System.Web.Routing;
    using Fanex.CRM.Web.Controllers.Helper;

    public class FeedbackController: BaseController
    {
        private readonly IFeedbackService _feedbackService;
        IPermissionService _permission;

        public FeedbackController(): this(null, null) { }
                
        public FeedbackController(IFeedbackService feedbackService, IPermissionService permissionService) 
        {
            _feedbackService = feedbackService ?? new FeedbackService();
            _permission = permissionService ?? new PermissionService();
        }

        #region Views
        public ActionResult AddFeedback()
        {
            ViewBag.Title = "Add Feedback";
            return View();
        }

        public ActionResult AddGuestFeedback()
        {
            ViewBag.Title = "Add Guest Feedback";
            return View();
        }

        public ActionResult DeletedFeedback()
        {
            if (!_permission.GetHostPermission(Fanex.CRM.Infrastructure.Common.Enumerations.Functions.Feedback.DeleteFeedback, AppSession.RoleAccess))
                return new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Common",
                    action = "AccessDenied"
                }));

            return View();
        }

        public ActionResult LastestFeedback()
        {
            return View();
        }

        public ActionResult ManageFeedback()
        {
            return View();
        }

        public ActionResult CustomerLastestFeedback()
        {
            return View();
        }

        public ActionResult EditFeedback()
        {
            return View();
        }
        #endregion

        #region Post Actions
        [HttpPost]
        public JsonResult AddGuestFeedback(Feedback feedback)
        {
            var result = new Result();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.AddGuestFeedback(feedback);
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
        public JsonResult SelTopFeedbackByCustId(int custId, int selectTop, int pageIndex, int pageSize, bool fromCache)
        {
            var result = new DataGridViewDTO();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.SelTopFeedbackByCustId(custId, selectTop, pageIndex, pageSize, fromCache);
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
        public JsonResult GetDeletedFeedback(string fromDate, string toDate, string userName, int isGuest, int communicationID, int mainCategoryID, int subCategoryID, int isReviewed, int responseId, int pageIndex, int pageSize, bool fromCache)
        {
            var result = new DataGridViewDTO();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.GetDeletedFeedback(fromDate, toDate, userName, isGuest, communicationID, mainCategoryID, 
                    subCategoryID, isReviewed, responseId, pageIndex, pageSize, fromCache);
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
        public JsonResult GetFeedback(string fromDate, string toDate, string UserName, int IsGuest, int CommunicationID, int MainCategoryID, int SubCategoryID, int StatusID, int IsReviewed, int responseId, int pageIndex, int pageSize, bool fromCache)
        {
            var result = new DataGridViewDTO();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.GetFeedback(fromDate, toDate, UserName, IsGuest, CommunicationID, MainCategoryID, SubCategoryID,
                    StatusID, IsReviewed, responseId, pageIndex, pageSize, fromCache);
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
        public JsonResult SelTopFeedback(int selectTop, int pageIndex, int pageSize, bool fromCache)
        {
            var result = new DataGridViewDTO();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.SelTopFeedback(selectTop, pageIndex, pageSize, fromCache);
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
        public JsonResult GetById(int Id)
        {
            var result = new Feedback();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.GetById(Id);
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
        public JsonResult EditFeedback(Feedback feedback)
        {
            var result = new Result();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.EditFeedback(feedback);
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
        public JsonResult AddFeedback(Feedback feedback)
        {
            var result = new Result();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.AddFeedback(feedback);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        #endregion
    }
}