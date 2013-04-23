namespace Fanex.CRM.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web;
    using Fanex.CRM.DataAccess.Impl;
    using Fanex.CRM.DataAccess.Interface;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Service.Interface;
    using MongoDB.Driver;

    public class CustomerService : ICustomerService
    {
        private const string KeyCustomer = "_customerKey";
        private readonly ICustomerDataAccess _repository;
        private User _loginUser;
        private readonly IPermissionService _permissionService;

        public CustomerService() : this(null, null) { }

        public CustomerService(ICustomerDataAccess repository, IPermissionService permissionService)
        {
            this._repository = repository ?? new CustomerDataAccess();
            this._permissionService = permissionService ?? new PermissionService();
            this._loginUser = SessionExtension.GetSession(HttpContext.Current.Session, "CRMUserInfo") as User;
        }

        public Customer GetCustomerInfoById(int custId)
        {
            try
            {
                var customer = _repository.FindBy(custId);
                return customer;
            }
            catch (Exception ex)
            {                
                Helper.SendMailErrorNotice(string.Format("(Customer GetCustomerInfoById: {0}) - {1}", custId,
                                                         ex.Message));
                LogExtensions.ExceptionHandler(ex);
                return new Customer();
            }
        }

        public List<RelatedCustomer> VerifyUserName(string userName)
        {
            try
            {
                var result = this._repository.VerifyUserName(userName);
                return result;
            }
            catch (Exception ex)
            {
                Helper.SendMailErrorNotice(string.Format("(Customer VerifyUserName: {0}) - {1}", userName,
                                                         ex.Message));
                LogExtensions.ExceptionHandler(ex);
                return new List<RelatedCustomer>();
            }
        }

        public Customer GetCustomerInfoByUserName(string userName, bool isGuest)
        {
            try
            {
                HttpContext.Current.Session.Remove(Enumerations.Functions.CRMCustomer.SearchCustomerInfo.ToString());

                var customer = this._repository.FindBy(userName, isGuest);
                return customer;
            }
            catch (Exception ex)
            {                
                Helper.SendMailErrorNotice(string.Format("(Customer GetCustomerInfoByUserName: {0}) - {1}",
                                                         userName, ex.Message));
                LogExtensions.ExceptionHandler(ex);
                return new Customer();
            }
        }

        public List<Customer> GetListUpline(int custId)
        {
            try
            {
                List<Customer> listUpline = this._repository.FindAllUpline(custId);
                return listUpline;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(Customer GetListUpline: {0}) - {1}", custId, ex.Message));
                return new List<Customer>();
            }
        }

        public Result UpdateCustomerInfo(Customer customer, bool isUpdateUserName, string ip)
        {
            var result = new Result();
            try
            {
                if (!this._permissionService.GetHostPermission(Enumerations.Functions.CRMCustomer.CustomerInfo))
                {
                    result.ErrCode = 500;
                    result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblNoUpdateCustomerPermission");
                    return result;
                }
                this._repository.UpdateCustomerInfo(customer, isUpdateUserName, ip, ref result);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(
                    string.Format(
                        "(Customer UpdateCustomerInfo) - Error: {0}<br> - User: {1}; {2}<br> - Update by: {3}",
                        ex.Message, customer.UserName, customer.Id, customer.Creator.Id));
            }
            return result;
        }


        public System.Data.DataTable GetAutoSearch(int userID, string userName, bool isGuest)
        {
            var result = new DataTable();
            try
            {
                var getData = this._repository.QueryUserName(userID, userName, isGuest);
                if (getData != null && getData.Tables.Count > 0)
                {
                    result = getData.Tables[0];
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }
                
        public Result UpdateAutoCustomerSearch(int custID, int UserID, string userName)
        {
            var result = new Result();
            try
            {
                if (!this._permissionService.GetHostPermission(Enumerations.Functions.CRMCustomer.CustomerInfo))
                {
                    result.ErrCode = 500;
                    result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblNoUpdateCustomerPermission");
                    return result;
                }
                if (UserID > 0)
                    this._repository.UpdateCustomerSearch(custID, UserID, userName, ref result);
                else if (UserID < 0)
                    UpdateCustomerSearchCountValue(UserID, userName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(
                    string.Format(
                        "(Update custSearch) - Error: {0}<br> - User: {1}; {2}<br> - Update by: {3}",
                        ex.Message, custID, UserID, userName));
            }
            return result;
        }

        public MongoCursor<MongoDB.Bson.BsonDocument> CustomerAutoSuggest(string customerName, bool isGuest)
        {
            MongoCursor<MongoDB.Bson.BsonDocument> result = null;
            try
            {
                result = this._repository.CustomerAutoSuggest(customerName.ToLower(), isGuest);

            }
            catch (Exception ex)
            {
                Helper.SendMailErrorNotice(
                    string.Format(
                        "(Update CustomerAutoSuggest) - Error: {0}<br> - User: {1}; IsGuest: {2}",
                        ex.Message, customerName, isGuest));
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public bool UpdateCustomerSearchCountValue(int custId, string customerName)
        {
            bool result = false;
            try
            {
                result = this._repository.UpdateCustomerSearchCountValue(custId, customerName);
            }
            catch (Exception ex)
            {
                Helper.SendMailErrorNotice(
                    string.Format(
                        "(UpdateCustomerSearchCountValue) - custId: {0}<br> - User: {1};",
                        ex.Message, custId));
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
    }
}
