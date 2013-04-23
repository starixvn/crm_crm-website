
namespace Fanex.CRM.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Entities.ViewModel;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Service.Impl;
    using Fanex.CRM.Service.Interface;

    public class AdminToolController: BaseController
    {
        ISettingService _settingService;
        IFeedbackService _feedbackService;
        ICountryService _countryService;

        public AdminToolController()
            : this(null, null, null)
        {
        }

        public AdminToolController(ISettingService settingService, IFeedbackService feedbackService, ICountryService countryService)
        {
            _settingService = settingService?? new SettingService();
            _feedbackService = feedbackService?? new FeedbackService();
            _countryService = countryService ?? new CountryService();
        }
        #region Views
        public ActionResult AddSettingRelation()
        {
            return View();
        }

        public ActionResult SettingEdit()
        {
            return View();
        }

        public ActionResult SettingIns()
        {
            return View();
        }

        public ActionResult SettingManage() 
        {
            return View();
        }

        public ActionResult SettingRelation()
        {
            return View();
        }

        public ActionResult SubjectTemplateManagement()
        {
            return View();
        }

        public ActionResult SubjectTemplateEdit()
        {
            return View();
        }

        public ActionResult SubjectTemplateIns()
        {
            return View();
        }
        #endregion

        #region post Actions
        
        [HttpPost]
        public JsonResult SelAllSetting() 
        {
            var result = new AllSetting();
            JsonResult jsonResult;
            try
            {
                result = _settingService.SelAllSetting();
            }
            catch(Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            
            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetNumOfFeedbackSubjectTemplateItem() 
        {
            int result = 0;
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.GetNumOfFeedbackSubjectTemplateItem();
            }
            catch(Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetFeedbackSubjectTemplateItemsRange(int range) 
        {
            var result = new List<FeedbackTemplate>();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.GetFeedbackSubjectTemplateItemsRange(range);
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
        public JsonResult GetAllCountry()
        {
            IEnumerable<Country> result = new List<Country>();
            JsonResult jsonResult;
            try
            {
                result = _countryService.GetAll();
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
        public JsonResult GetSettingRelationByName(string settingName, int parentId)
        {
            IEnumerable<Setting> result = new List<Setting>();
            JsonResult jsonResult;
            try
            {
                result = _settingService.GetSettingRelationByName(settingName, parentId);
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
        public JsonResult SelAllSite()
        {
            var result = new List<Site>();
            JsonResult jsonResult;
            try
            {
                result = _settingService.SelAllSite();
            }
            catch(Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetSettingByParent(int parentId)
        {
            IEnumerable<Setting> result = new List<Setting>();
            JsonResult jsonResult;
            try
            {
                result = _settingService.GetByParent(parentId);
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
        public JsonResult SelSettingByType(string type, string excludeItem)
        {
            var result = new List<Setting>();
            JsonResult jsonResult;
            try
            {
                result = _settingService.GetByType(type, excludeItem);
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
        public JsonResult GetAllSetting(string settingName)
        {
            var result = new List<Setting>();
            JsonResult jsonResult;
            try
            {
                result = _settingService.GetAllGroupByType()
                    .Where(s => !s.Type.Equals(settingName))
                    .Select(s => s).ToList();
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
        public JsonResult DeleteSettingById(int id, int userId)
        {
            Result result;
            JsonResult jsonResult;
            try
            {
                string ip = Fanex.CRM.Infrastructure.Helper.Helper.getClientIP();
                result = _settingService.Delete(id, ip, userId);
            }
            catch (Exception ex)
            {
                result = new Result() 
                    { 
                        ErrCode = (int)Enumerations.ErrorCode.SystemError,
                        ErrMsg = ex.Message
                    };
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetSettingByTypeNoChild(string type)
        {
            IEnumerable<Setting> result = new List<Setting>();
            JsonResult jsonResult;
            try
            {
                result = _settingService.GetByTypeNoChild(type);
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
            var result = new Setting();
            JsonResult jsonResult;
            try
            {
                result = _settingService.GetById(Id);
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
        public JsonResult GetAllSettingParent(string settingName)
        {
            var result = new List<Setting>();
            JsonResult jsonResult;
            try
            {
                result = _settingService.GetAll().Where(s => !s.Type.Equals(settingName)).Select(s => s).ToList();
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
        public JsonResult InsertSetting(Setting setting, int userId, int parentId)
        {
            Result result;
            JsonResult jsonResult;
            try
            {
                string ip = Fanex.CRM.Infrastructure.Helper.Helper.getClientIP();
                bool f = _settingService.IsExistedSetting(setting);
                if (f)
                {
                    result = new Result() { 
                        ErrCode = (int) Enumerations.ErrorCode.SettingIsExist,
                        ErrMsg = "Setting existed already"
                    };
                }
                else
                {
                    result = _settingService.Create(setting, ip, userId, parentId);
                }
            }
            catch (Exception ex)
            {
                result = new Result()
                {
                    ErrCode = (int)Enumerations.ErrorCode.SystemError,
                    ErrMsg = ex.Message
                };
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult UpdateSetting(Setting setting, string oldSettingName, int userId)
        {            
            Result result;
            JsonResult jsonResult;
            try
            {
                string ip = Fanex.CRM.Infrastructure.Helper.Helper.getClientIP();
                if (setting.Value.ToLower().Equals(oldSettingName.ToLower()))
                    result = _settingService.Update(setting, ip, userId);
                else
                {
                    bool f = _settingService.IsExistedSetting(setting);
                    if (f)
                    {
                        result = new Result()
                        {
                            ErrCode = (int)Enumerations.ErrorCode.SettingIsExist,
                            ErrMsg = "Setting existed already"
                        };
                    }
                    else
                        result = _settingService.Update(setting, ip, userId);
                }
            }
            catch (Exception ex)
            {
                result = new Result()
                {
                    ErrCode = (int)Enumerations.ErrorCode.SystemError,
                    ErrMsg = ex.Message
                };
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetSettingByType(string type)
        {
            IEnumerable<Setting> result = new List<Setting>();
            JsonResult jsonResult;
            try
            {
                result = _settingService.GetByType(type).ToList();
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
        public JsonResult UpdateSettingRelation(string settingId, string settingName, int userId, int parentId)
        {            
            Result result;
            JsonResult jsonResult;
            try
            {
                string ip = Fanex.CRM.Infrastructure.Helper.Helper.getClientIP();
                result = _settingService.Create(settingId, settingName, ip, userId, parentId);
                
            }
            catch (Exception ex)
            {
                result = new Result()
                {
                    ErrCode = (int)Enumerations.ErrorCode.SystemError,
                    ErrMsg = ex.Message
                };
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult DeleteSettingRelationById(int settingId, int userId, int parentId)
        {
            Result result;
            JsonResult jsonResult;
            try
            {
                string ip = Fanex.CRM.Infrastructure.Helper.Helper.getClientIP();
                result = _settingService.Delete(settingId, ip, userId, parentId);

            }
            catch (Exception ex)
            {
                result = new Result()
                {
                    ErrCode = (int)Enumerations.ErrorCode.SystemError,
                    ErrMsg = ex.Message
                };
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult DeleteSubjectFeedbackTemplate(int templateId)
        {
            Result result;
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.DeleteSubjectFeedbackTemplate(templateId);

            }
            catch (Exception ex)
            {
                result = new Result()
                {
                    ErrCode = (int)Enumerations.ErrorCode.SystemError,
                    ErrMsg = ex.Message
                };
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SelFeedbackTemplatePageWithSearch(string searchString, int pageIndex, int pageSize)
        {
            var result = new FeedbackTemplateDGViewDTO();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.SelFeedbackTemplatePageWithSearch(searchString, pageIndex, pageSize);

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
        public JsonResult GetFeedbackSubjectTemplateItem(int templateId)
        {

            var result = new FeedbackTemplate();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.GetFeedbackSubjectTemplateItems(templateId)[0];

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
        public JsonResult EditSubjectFeedbackTemplate(int templateId, string newSubject)
        {
            Result result;
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.EditSubjectFeedbackTemplate(templateId, newSubject);

            }
            catch (Exception ex)
            {
                result = new Result() { 
                    ErrCode = (int)Enumerations.ErrorCode.SystemError,
                    ErrMsg= ex.Message
                };
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AddSubjectFeedbackTemplate(string subject)
        {
            Result result;
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.AddSubjectFeedbackTemplate(subject);

            }
            catch (Exception ex)
            {
                result = new Result()
                {
                    ErrCode = (int)Enumerations.ErrorCode.SystemError,
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