// --------------------------------------------------------------------------
// <copyright file="CustomerController.cs" company="Nexcel Solutions Vietnam">
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
    using System.Collections.Specialized;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using DNAide.Core;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Entities.ViewModel;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Service.Impl;
    using Fanex.CRM.Service.Interface;

    /// <summary>
    /// Customer Controller.
    /// </summary>
    public class CustomerController : BaseController
    {
        private ICustomerService _customerService;

        private ISettingService _settingService;

        private IFeedbackService _feedbackService;

        public CustomerController()
            : this(null, null, null)
        {
        }

        public CustomerController(ICustomerService customerService, ISettingService settingService, IFeedbackService feedbackService)
        {
            _customerService = customerService ?? new CustomerService();
            _settingService = settingService ?? new SettingService();
            _feedbackService = feedbackService ?? new FeedbackService();
        }

        #region Views

        public ActionResult CRMInfo()
        {
            return View();
        }

        public ActionResult EditCRMInfo()
        {
            return View();
        }
        #endregion

        #region Post Actions

        /// <summary>
        /// Selects the list up line.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <returns>The list up line.</returns>
        [HttpPost]
        public JsonResult SelListUpline(int custId)
        {
            var result = new List<Customer>();
            JsonResult jsonResult;
            try
            {
                result = _customerService.GetListUpline(custId);
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
        /// Updates the automatic customer search.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="custID">The customer identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns>Update result.</returns>
        [HttpPost]
        public JsonResult UpdateAutoCustomerSearch(int userID, int custID, string userName)
        {
            var result = new Result();
            JsonResult jsonResult;
            try
            {
                var ip = Fanex.CRM.Infrastructure.Helper.Helper.getClientIP();
                result = _customerService.UpdateAutoCustomerSearch(userID, custID, userName);
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
        /// Gets the customer information by user name.
        /// </summary>
        /// <param name="username">Name of the user.</param>
        /// <param name="isGuest">If set to <c>true</c> [is guest].</param>
        /// <returns>The customer information by user name.</returns>
        [HttpPost]
        public JsonResult GetCustomerInfoByUserName(string username, bool isGuest)
        {
            var result = new Customer();
            JsonResult jsonResult;
            try
            {
                result = _customerService.GetCustomerInfoByUserName(username, isGuest);
                if (result != null)
                {
                    int siteId = result.Site.Id;
                    result.Site = _settingService.GetById(siteId);

                    SessionExtension.AddSession(
                        System.Web.HttpContext.Current.Session,
                        Enumerations.Functions.CRMCustomer.SearchCustomerInfo.ToString(),
                        new SearchCustomerInfo() { UserName = username, IsGuest = isGuest });
                }
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
        /// Gets the customer information by identifier.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <returns>The customer information by identifier.</returns>
        [HttpPost]
        public JsonResult GetCustomerInfoById(int custId)
        {
            var result = new Customer();
            JsonResult jsonResult;
            try
            {
                result = _customerService.GetCustomerInfoById(custId);
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
        /// Updates the customer information.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="titleId">The title identifier.</param>
        /// <param name="birthDay">The birth day.</param>
        /// <param name="languageId">The language identifier.</param>
        /// <param name="phone">The phone value.</param>
        /// <param name="mobilePhone">The mobile phone.</param>
        /// <param name="fax">The fax value.</param>
        /// <param name="email">The email.</param>
        /// <param name="bettingCode">The betting code.</param>
        /// <param name="refCode2">The reference code2.</param>
        /// <param name="pic1">The pic1 value.</param>
        /// <param name="pic2">The pic2 value.</param>
        /// <param name="pic3">The pic3 value.</param>
        /// <param name="question">The question.</param>
        /// <param name="answer">The answer.</param>
        /// <param name="countryId">The country identifier.</param>
        /// <param name="isVip">If set to <c>true</c> [is VIP].</param>
        /// <param name="relatedCustId">The related customer identifier.</param>
        /// <param name="siteId">The site identifier.</param>
        /// <param name="bankName">Name of the bank.</param>
        /// <param name="bankAddress">The bank address.</param>
        /// <param name="bankAccountName">Name of the bank account.</param>
        /// <param name="bankAccountNo">The bank account no.</param>
        /// <param name="bankSwiftCode">The bank swift code.</param>
        /// <param name="remark">The remark.</param>
        /// <param name="creatorId">The creator identifier.</param>
        /// <param name="bankHolderName">Name of the bank holder.</param>
        /// <param name="specialRemark">The special remark.</param>
        /// <param name="isUpdateUserName">If set to <c>true</c> [is update user name].</param>
        /// <param name="skype">The skype.</param>
        /// <param name="msn">The MSN value.</param>
        /// <returns>Update result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        public JsonResult UpdateCustomerInfo(
            int userId,
            int custId,
            string userName,
            string firstName,
            string lastName,
            int titleId,
            string birthDay,
            int languageId,
            string phone,
            string mobilePhone,
            string fax,
            string email,
            string bettingCode,
            string refCode2,
            string pic1,
            string pic2,
            string pic3,
            string question,
            string answer,
            int countryId,
            bool isVip,
            int relatedCustId,
            int siteId,
            string bankName,
            string bankAddress,
            string bankAccountName,
            string bankAccountNo,
            string bankSwiftCode,
            string remark,
            int creatorId,
            string bankHolderName,
            string specialRemark,
            bool isUpdateUserName,
            string skype,
            string msn)
        {
            var result = new Result();
            JsonResult jsonResult;
            try
            {
                var customer = new Customer();
                var ip = Fanex.CRM.Infrastructure.Helper.Helper.getClientIP();

                customer.Answer = answer;
                customer.BankInfo = new BankInfo
                {
                    AccountName = bankAccountName,
                    AccountNo = bankAccountNo,
                    BankAddress = bankAddress,
                    BankName = bankName,
                    SwiftCode = bankSwiftCode
                };

                customer.BettingCode = bettingCode;
                customer.Country = countryId;
                customer.CreatedDate = DateTime.Now;
                customer.Creator = new User { Id = userId };

                customer.CustomerInfo = new CustomerInfo
                {
                    BirthDay = !string.IsNullOrEmpty(birthDay) ? DateTime.Parse(birthDay) : DateTime.Now,
                    Email = email,
                    Fax = fax,
                    FirstName = firstName,
                    LastName = lastName,
                    MobilePhone = mobilePhone,
                    Phone = phone,
                    Title = new Setting { Id = titleId },
                    Skype = skype,
                    MSN = msn
                };

                customer.Id = custId;
                customer.IsVip = isVip;
                customer.Language = languageId;
                customer.Pic1 = pic1;
                customer.Pic2 = pic2;
                customer.Pic3 = pic3;
                customer.Question = question;
                customer.RefCode2 = refCode2;
                customer.RelatedCustomer = new Customer { Id = relatedCustId };
                customer.Remark = remark;
                customer.Site = new Setting { Id = siteId };
                customer.UserName = userName;
                customer.SpecialRemark = specialRemark;
                customer.BankInfo.AccountHolderName = bankHolderName;

                result = _customerService.UpdateCustomerInfo(customer, isUpdateUserName, ip);
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
        /// Verifies the name of the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>Verify result.</returns>
        [HttpPost]
        public JsonResult VerifyUserName(string username)
        {
            var result = new List<RelatedCustomer>();
            JsonResult jsonResult;
            try
            {
                result = _customerService.VerifyUserName(username);
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
        /// The customer automatic suggestion.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="query">The query.</param>
        /// <param name="isGuest">If set to <c>true</c> [is guest].</param>
        /// <param name="requestTime">The request time.</param>
        /// <returns>Suggestion result.</returns>
        [HttpGet]
        public JsonResult CustomerAutoSuggestion(int userId, string query, bool isGuest, string requestTime)
        {
            var result = new CustomersSuggestion();
            JsonResult jsonResult;
            try
            {
                var customerList = _customerService.GetAutoSearch(userId, query, isGuest);
                result.QueryParameters = query;
                result.Suggestions = isGuest ?
                    SelGuestSuggesstion(query, isGuest) :
                    ConvertCustomerListToObject(customerList);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(new
            {
                data = result.QueryData,
                query = result.QueryParameters,
                suggestions = result.Suggestions
            });

            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// The related customer automatic suggestion.
        /// </summary>
        /// <param name="tag">The tag value.</param>
        /// <returns>Suggestion result.</returns>
        [HttpGet]
        public JsonResult RelatedCustomerAutoSuggestion(string tag)
        {
            var result = new List<RelatedCustomerSuggestion>();
            var dtoResult = new List<dynamic>();
            JsonResult jsonResult;
            try
            {
                var customerList = _feedbackService.GetAutoSuggestionCustomer(tag);
                result = ConvertRelatedCustomerListToObject(customerList);
                for (var i = 0; i < result.Count; i++)
                {
                    dtoResult.Add(new
                    {
                        key = result[i].Key,
                        value = result[i].Value
                    });
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(dtoResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        #endregion
        #region private functions

        private StringCollection ConvertCustomerListToObject(DataTable tbl)
        {
            StringCollection suggestion = new StringCollection();
            foreach (DataRow row in tbl.Rows)
            {
                suggestion.Add(CastAide.AsString(row[0]));
            }

            return suggestion;
        }

        private StringCollection SelGuestSuggesstion(string userName, bool isGuest)
        {
            var suggestion = new StringCollection();
            var guestList = _customerService.CustomerAutoSuggest(userName, isGuest);
            foreach (var customer in guestList)
            {
                suggestion.Add(customer["UserName"].AsString.Replace("2_2", "."));
            }

            return suggestion;
        }

        /// <summary>
        /// Converts the related customer list to object.
        /// </summary>
        /// <param name="tbl">The table.</param>
        /// <returns>Convert result.</returns>
        private List<RelatedCustomerSuggestion> ConvertRelatedCustomerListToObject(DataTable tbl)
        {
            var suggestion = new List<RelatedCustomerSuggestion>();

            foreach (DataRow row in tbl.Rows)
            {
                suggestion.Add(new RelatedCustomerSuggestion()
                {
                    Key = CastAide.AsString(row[0]),
                    Value = CastAide.AsString(row[0])
                });
            }

            return suggestion;
        }

        #endregion
    }
}