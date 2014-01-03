namespace Fanex.CRM.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using DNAide.Core;
    using Fanex.CRM.DataAccess.Impl;
    using Fanex.CRM.DataAccess.Interface;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Service.Interface;

    public class UserService : IUserService
    {
        private readonly IUserDataAccess _userRepository;
        private readonly IPermissionService _permissionService;

        public UserService() : this(null, null) { }

        public UserService(IUserDataAccess userRepository, IPermissionService permissionService)
        {
            this._userRepository = userRepository ?? new UserDataAccess();
            this._permissionService = permissionService ?? new PermissionService();
        }

        public User GetUseInfo(string userName)
        {
            try
            {
                return this._userRepository.GetUserInfo(userName);

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(GetUseInfo) - Error: {0}<br> - User: {1}", ex.Message, userName));
                return new User();
            }
        }

        public IEnumerable<Menu> GetUserMenu(int userId)
        {
            try
            {
                var listUserMenu = this._userRepository.GetMenu(userId) as List<Menu>;
                return listUserMenu;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(GetUserMenu) - Error: {0}<br> - userId: {1}", ex.Message, userId));
                return new List<Menu>();
            }
        }        

        public Result CheckUserOnlineStatus()
        {
            
            var result = new Result();
            try                       
            {
                var isOnline = false;
                
                var onUserId = CastAide.AsInt32(SessionExtension.OnlineUserId);

                if (!AppSetting.IsLocalTest)
                {
                    isOnline = this._userRepository.OnlineUserUpdate(onUserId);
                    if (isOnline)
                        result.ErrCode = 0;
                    else
                    {
                        result.ErrCode = 500;
                        result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblErrorSessionIsExpired");
                        HttpContext.Current.Session.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrCode = 600;
                result.ErrMsg = ex.Message;
                HttpContext.Current.Session.Clear();
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(CheckUserOnlineStatus) - {0}", ex.Message));
            }

            return result;
        }

        public void AssignRoles(int custid, ref Dictionary<string, int> userRoleAccess, ref Dictionary<string, int> userRageAccess)
        {
            try
            {
                var pageAcess = new Dictionary<string, int>();
                var roleAccess = new Dictionary<string, int>();
                char[] delimiterChars = { ';' };

                var userRoles = this._permissionService.GetCustomerRoles(custid);

                if (userRoles != null && userRoles.Count != 0)
                {
                    string filenames = "";
                    foreach (var userRole in userRoles)
                    {
                        filenames = userRole.FileNames.ToLower();

                        string functionName = userRole.FuncName.ToLower();
                        int actualPolicy = userRole.ActualPolicy;
                        roleAccess.Add(functionName, actualPolicy);

                        if (string.IsNullOrEmpty(filenames))
                            continue;
                        string[] fileList = filenames.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string file in fileList)
                        {
                            var cleanFilePath = file.Trim();
                            if (!cleanFilePath.IndexOf("?").Equals(-1))
                            {
                                cleanFilePath = file.Substring(0, file.IndexOf('?'));
                                cleanFilePath = cleanFilePath.Trim();
                            }

                            if (!pageAcess.ContainsKey(cleanFilePath))
                                pageAcess.Add(cleanFilePath, actualPolicy);
                        }
                    }
                }
                
                userRageAccess = pageAcess;
                userRoleAccess = roleAccess;


            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }


        }
    }
}
