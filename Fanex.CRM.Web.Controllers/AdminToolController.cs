// --------------------------------------------------------------------------
// <copyright file="AdminToolController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

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

    /// <summary>
    /// Admin Tool Controller.
    /// </summary>
    public class AdminToolController : BaseController
    {
        private ISettingService _settingService;

        private IFeedbackService _feedbackService;

        private ICountryService _countryService;

        public AdminToolController()
            : this(null, null, null)
        {
        }

        public AdminToolController(ISettingService settingService, IFeedbackService feedbackService, ICountryService countryService)
        {
            _settingService = settingService ?? new SettingService();
            _feedbackService = feedbackService ?? new FeedbackService();
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

        /// <summary>
        /// Selects all setting.
        /// </summary>
        /// <returns>All setting.</returns>
        [HttpPost]
        public JsonResult SelAllSetting()
        {
            var result = new AllSetting();
            JsonResult jsonResult;
            try
            {
                result = _settingService.SelAllSetting();
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
        /// Gets the number of feedback subject template item.
        /// </summary>
        /// <returns>The number of feedback subject template item.</returns>
        [HttpPost]
        public JsonResult GetNumOfFeedbackSubjectTemplateItem()
        {
            int result = 0;
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.GetNumOfFeedbackSubjectTemplateItem();
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
        /// Gets the feedback subject template items range.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <returns>The feedback subject template items range.</returns>
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

        /// <summary>
        /// Gets all country.
        /// </summary>
        /// <returns>All country.</returns>
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

        /// <summary>
        /// Gets the setting relation by name.
        /// </summary>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>The setting relation by name.</returns>
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

        /// <summary>
        /// Selects all site.
        /// </summary>
        /// <returns>All site information.</returns>
        [HttpPost]
        public JsonResult SelAllSite()
        {
            var result = new List<Site>();
            JsonResult jsonResult;
            try
            {
                result = _settingService.SelAllSite();
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
        /// Gets the setting by parent.
        /// </summary>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>The setting by parent.</returns>
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

        /// <summary>
        /// Selects the setting by type.
        /// </summary>
        /// <param name="type">The type value.</param>
        /// <param name="excludeItem">The exclude item.</param>
        /// <returns>The setting by type.</returns>
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

        /// <summary>
        /// Gets all setting.
        /// </summary>
        /// <param name="settingName">Name of the setting.</param>
        /// <returns>All setting.</returns>
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

        /// <summary>
        /// Deletes the setting by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Delete result.</returns>
        [HttpPost]
        public JsonResult DeleteSettingById(int id, int userId)
        {
            Result result;
            JsonResult jsonResult;
            try
            {
                string ip = Fanex.CRM.Infrastructure.Helper.Helper.GetClientIP();
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

        /// <summary>
        /// Gets the setting by type no child.
        /// </summary>
        /// <param name="type">The type value.</param>
        /// <returns>The setting by type no child.</returns>
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

        /// <summary>
        /// Gets tool by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Tool by identifier.</returns>
        [HttpPost]
        public JsonResult GetById(int id)
        {
            var result = new Setting();
            JsonResult jsonResult;
            try
            {
                result = _settingService.GetById(id);
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
        /// Gets all setting parent.
        /// </summary>
        /// <param name="settingName">Name of the setting.</param>
        /// <returns>All setting parent.</returns>
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

        /// <summary>
        /// Inserts the setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>Insert result.</returns>
        [HttpPost]
        public JsonResult InsertSetting(Setting setting, int userId, int parentId)
        {
            Result result;
            JsonResult jsonResult;
            try
            {
                string ip = Fanex.CRM.Infrastructure.Helper.Helper.GetClientIP();
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

        /// <summary>
        /// Updates the setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="oldSettingName">Old name of the setting.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Update result.</returns>
        [HttpPost]
        public JsonResult UpdateSetting(Setting setting, string oldSettingName, int userId)
        {
            Result result;
            JsonResult jsonResult;
            try
            {
                string ip = Fanex.CRM.Infrastructure.Helper.Helper.GetClientIP();
                if (setting.Value.ToLower().Equals(oldSettingName.ToLower()))
                {
                    result = _settingService.Update(setting, ip, userId);
                }
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
                    {
                        result = _settingService.Update(setting, ip, userId);
                    }
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

        /// <summary>
        /// Gets the type of the setting by.
        /// </summary>
        /// <param name="type">The type value.</param>
        /// <returns>The type of the setting by.</returns>
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

        /// <summary>
        /// Updates the setting relation.
        /// </summary>
        /// <param name="settingId">The setting identifier.</param>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>Update result.</returns>
        [HttpPost]
        public JsonResult UpdateSettingRelation(string settingId, string settingName, int userId, int parentId)
        {
            Result result;
            JsonResult jsonResult;
            try
            {
                string ip = Fanex.CRM.Infrastructure.Helper.Helper.GetClientIP();
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

        /// <summary>
        /// Deletes the setting relation by identifier.
        /// </summary>
        /// <param name="settingId">The setting identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>Delete result.</returns>
        [HttpPost]
        public JsonResult DeleteSettingRelationById(int settingId, int userId, int parentId)
        {
            Result result;
            JsonResult jsonResult;
            try
            {
                string ip = Fanex.CRM.Infrastructure.Helper.Helper.GetClientIP();
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

        /// <summary>
        /// Deletes the subject feedback template.
        /// </summary>
        /// <param name="templateId">The template identifier.</param>
        /// <returns>Delete result.</returns>
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

        /// <summary>
        /// Selects the feedback template page with search.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>The feedback template page with search.</returns>
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
        /// Gets the feedback subject template item.
        /// </summary>
        /// <param name="templateId">The template identifier.</param>
        /// <returns>The feedback subject template item.</returns>
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

        /// <summary>
        /// Edits the subject feedback template.
        /// </summary>
        /// <param name="templateId">The template identifier.</param>
        /// <param name="newSubject">The new subject.</param>
        /// <returns>Edit result.</returns>
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

        /// <summary>
        /// Adds the subject feedback template.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns>Add result.</returns>
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