
namespace Fanex.CRM.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Service.Impl;
    using Fanex.CRM.Service.Interface;
    using System.Data;
    using Fanex.CRM.Entities.ViewModel;
    using System.Collections.Specialized;
    using DNAide.Core;

    public class CustomerController: BaseController
    {
        ICustomerService _customerService;
        ISettingService _settingService;
        IFeedbackService _feedbackService;
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

        [HttpPost]
        public JsonResult GetCustomerInfoByUserName(string userName, bool isGuest)
        {
            var result = new Customer();
            JsonResult jsonResult;
            try
            {
                result = _customerService.GetCustomerInfoByUserName(userName, isGuest);
                if (result != null)
                {
                    int siteId = result.Site.Id;
                    result.Site = _settingService.GetById(siteId);

                    SessionExtension.AddSession(System.Web.HttpContext.Current.Session,
                        Enumerations.Functions.CRMCustomer.SearchCustomerInfo.ToString(),
                        new SearchCustomerInfo() { UserName = userName, IsGuest = isGuest });
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

        [HttpPost]
        public JsonResult UpdateCustomerInfo(int userId, int custId, string userName, string firstName, string lastName,
            int titleId, string birthDay, int languageId, string phone, string mobilePhone, string fax, string email,
            string bettingCode, string refCode2, string pic1, string pic2, string pic3, string question, string answer,
            int countryId, bool isVip, int relatedCustId, int siteId, string bankName, string bankAddress, string bankAccountName,
            string bankAccountNo, string bankSwiftCode, string remark, int creatorId, string bankHolderName, string specialRemark, bool isUpdateUserName, string skype, string msn)
        {
            var result = new Result();
            JsonResult jsonResult;
            try
            {
                #region private variables                
                var customer = new Customer();
                var ip = Fanex.CRM.Infrastructure.Helper.Helper.getClientIP();
                #endregion

                #region assign data
                customer.Answer = answer;
                #region bank info
                customer.BankInfo = new BankInfo
                {
                    AccountName = bankAccountName,
                    AccountNo = bankAccountNo,
                    BankAddress = bankAddress,
                    BankName = bankName,
                    SwiftCode = bankSwiftCode
                };
                #endregion
                customer.BettingCode = bettingCode;
                customer.Country = countryId;
                customer.CreatedDate = DateTime.Now;
                customer.Creator = new User { Id = userId };
                #region customer info
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
                #endregion
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
                #endregion

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
        
        [HttpPost]
        public JsonResult VerifyUserName(string UserName)
        {
            var result = new List<RelatedCustomer>();
            JsonResult jsonResult;
            try
            {
                result = _customerService.VerifyUserName(UserName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpGet]
        public JsonResult CustomerAutoSuggestion(int userId, string query, bool isGuest, string rTime) 
        {
            var result = new CustomersSuggestion();
            JsonResult jsonResult;
            try
            {
                var customerList = _customerService.GetAutoSearch(userId, query, isGuest);
                result.query = query;
                result.suggestions = isGuest
                    ? SelGuestSuggesstion(query, isGuest)
                    : ConvertCustomerListToObject(customerList);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpGet]
        public JsonResult RelatedCustomerAutoSuggestion(string tag)
        {
            var result = new List <RelatedCustomerSuggestion >();
            JsonResult jsonResult;
            try
            {
                var customerList = _feedbackService.GetAutoSuggestionCustomer(tag);
                result = ConvertRelatedCustomerListToObject(customerList);
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

        private List<RelatedCustomerSuggestion> ConvertRelatedCustomerListToObject(DataTable tbl)
        {
            var suggestion = new List<RelatedCustomerSuggestion>();

            foreach (DataRow row in tbl.Rows)
            {
                suggestion.Add(new RelatedCustomerSuggestion()
                {
                    key = CastAide.AsString(row[0]),
                    value = CastAide.AsString(row[0])
                });
            }
            return suggestion;
        }
        #endregion
    }
}