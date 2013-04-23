namespace Fanex.CRM.Service.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using DNAide.Core;
    using Fanex.CRM.DataAccess.Impl;
    using Fanex.CRM.DataAccess.Interface;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Entities.ViewModel;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Configuration;
    using Fanex.CRM.Infrastructure.Core;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Service.Interface;

    public partial class FeedbackService : IFeedbackService
    {
        #region private variables
        private string _FeedbackStatisticDetailSessionKey = "crm.feedbackstatisticdetailsessionkey";
        private string _FeedbackStatisticDetailMappedSessionKey = "crm.FeedbackStatisticDetailMappedSessionKey";
        private string _LastedFeedback = "crm.lastedfeedback";
        private string _LastedFeedbackByCustId = "crm.lastedfeedbackbycustid";
        private string _DeletedFeedback = "crm.deletedfeedbacksessionkey";
        private string _LastedTopFeedbackByCustId = "crm.lastedtopfeedbackbycustid";
        private string _FeedbackStatisticSessionKey = "crm.feedbackstatisticsessionkey";
        private string _LastedTopFeedback = "crm.lastedtopfeedback";
        
        private string _IbcDailySummary = "crm.ibcdailysummary";
        private string _IbcDailyDetail = "crm.ibcdailydetail";
        private string _licenseeDailyDetail = "crm.licenseedailydetail";
        private string _IbcDailyDetailNotMapped = "crm.ibcdailydetailnotmapped";

        private string _LicenseeDailyDetailNotMapped = "crm.ibcdailydetailnotmapped";
        private string _licenseeDailySummary = "crm.licenseedailysummary";

        private string _CasinoDailySummary = "crm.casinodailysummarystatistics";
        private string _CasinoDailyDetail = "crm.casinoweeklydetail";
        private string _CasinoDailyDetailNotMapped = "crm.casinoweeklydetailnotmapped";
        private string _CasinoWeeklySummary = "crm.casinoweeklysummary";
        private readonly IFeedbackDataAccess _repository;
        private readonly ISettingService _settingService;
        private readonly ICountryService _countryService;
        private readonly IPermissionService _permissionService;
        #endregion
        public FeedbackService() : this(null, null, null, null) { }

        public FeedbackService(IFeedbackDataAccess repository, ISettingService settingService, 
            ICountryService countryService, IPermissionService permissionService)
        {
            _repository = repository?? new FeedbackDataAccess();
            _settingService = settingService ?? new SettingService();
            _countryService = countryService ?? new CountryService();
            _permissionService = permissionService ?? new PermissionService();
        }
               
        #region get feedback by id
        public Feedback GetById(int id)
        {
            Check.Argument.IsNotNegativeOrZero(id, "Id");
            Feedback feedback = new Feedback();
            try
            {
                feedback = _repository.GetById(id);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(Feedback GetById: {0}) - {1}", id, ex.Message));
            }
            return feedback;
        }
        #endregion

        #region add guest feedback
        public Result AddGuestFeedback(Feedback feedback)
        {
            var ip = Helper.getClientIP();
            var entity = feedback;

            Result result = new Result();
            try
            {
                Check.Argument.IsValidIP(ip, "Ip");
                if (!_permissionService.GetHostPermission(Enumerations.Functions.Feedback.CreateFeedback))
                {
                    result.ErrCode = (int)Enumerations.ErrorCode.UpdateFeedback;
                    result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblNoCreateFeedbackPermission");
                    return result;
                }
                if(feedback.Status.Id.Equals((int)Enumerations.Functions.CRMStatus.Deleted) &&
                    !_permissionService.GetHostPermission(Enumerations.Functions.Feedback.DeleteFeedback))
                {
                    result.ErrCode = (int)Enumerations.ErrorCode.DeleteFeedback;
                    result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblNoDeletedFeedbackPermission");
                    return result;
                }
                if (!_permissionService.GetHostPermission(Enumerations.Functions.CRMCustomer.CustomerInfo))
                {
                    result.ErrCode = (int)Enumerations.ErrorCode.UpdateCustomerInfo;
                    result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblNoUpdateCustomerPermission");
                    return result;
                }

                _repository.AddGuestFeedback(entity, ip, ref result);
            }
            catch (Exception ex)
            {
                result.ErrCode = -100;
                result.ErrMsg = ex.Message;

                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(
                    string.Format("(AddGuestFeedback) -Error: {0}<br> - Creator: {1}<br> - CustomerId: " +
                                  "{2}<br> - FB Title: {3}<br> - FB Id: {4}<br> - FB Description: {5}" +
                                  "<br> - FB Solution: {6}<br> - FB Review: {7}<br> - Cust Info: " +
                                  "<br>   + FName: {8}<br>   + LName: {9}<br>   + Phone: {10}" +
                                  "<br>   + Fax: {11}<br>   + Skype: {12}<br>   + Mobile: {13}," +
                                  "<br>   + Email: {14}<br>   + MSN: {15}", ex.Message,
                                  feedback.Creator.Id, feedback.Customer.Id, feedback.Subject, feedback.Id,
                                  feedback.Description, feedback.Solution, feedback.Review,
                                  feedback.CustomerInfo.FirstName,
                                  feedback.CustomerInfo.LastName, feedback.CustomerInfo.Phone, feedback.CustomerInfo.Fax,
                                  feedback.CustomerInfo.Skype, feedback.CustomerInfo.MobilePhone,
                                  feedback.CustomerInfo.Email,
                                  feedback.CustomerInfo.MSN));
            }
            return result;
        }
        #endregion
        
        #region add new feedback
        public Result AddFeedback(Feedback feedback)
        {
            var entity = feedback;
            var ip = Helper.getClientIP();

            Result result = new Result();
            try
            {
                Check.Argument.IsValidIP(ip, "Ip");
                if (!_permissionService.GetHostPermission(Enumerations.Functions.Feedback.CreateFeedback))
                {
                    result.ErrCode = (int)Enumerations.ErrorCode.UpdateFeedback;
                    result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblNoCreateFeedbackPermission");
                    return result;
                }
                if (feedback.Status.Id.Equals((int)Enumerations.Functions.CRMStatus.Deleted) && !_permissionService.GetHostPermission(Enumerations.Functions.Feedback.DeleteFeedback))
                {
                    result.ErrCode = (int)Enumerations.ErrorCode.DeleteFeedback;
                    result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblNoDeletedFeedbackPermission");
                    return result;
                }
                if (!_permissionService.GetHostPermission(Enumerations.Functions.CRMCustomer.CustomerInfo))
                {
                    result.ErrCode = (int)Enumerations.ErrorCode.UpdateCustomerInfo;
                    result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblNoUpdateCustomerPermission");
                    return result;
                }
                _repository.AddFeedback(entity, ip, ref result);
            }
            catch (Exception ex)
            {
                result.ErrCode = 100;
                result.ErrMsg = ex.Message;

                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(
                    string.Format("(AddFeedback) -Error: {0}<br> - Creator: {1}<br> - CustomerId: " +
                                  "{2}<br> - FB Title: {3}<br> - FB Id: {4}<br> - FB Description: {5}" +
                                  "<br> - FB Solution: {6}<br> - FB Review: {7}<br> - Cust Info: " +
                                  "<br>   + FName: {8}<br>   + LName: {9}<br>   + Phone: {10}" +
                                  "<br>   + Fax: {11}<br>   + Skype: {12}<br>   + Mobile: {13}," +
                                  "<br>   + Email: {14}<br>   + MSN: {15}", ex.Message,
                                  feedback.Creator.Id, feedback.Customer.Id, feedback.Subject, feedback.Id,
                                  feedback.Description, feedback.Solution, feedback.Review,
                                  feedback.CustomerInfo.FirstName,
                                  feedback.CustomerInfo.LastName, feedback.CustomerInfo.Phone, feedback.CustomerInfo.Fax,
                                  feedback.CustomerInfo.Skype, feedback.CustomerInfo.MobilePhone,
                                  feedback.CustomerInfo.Email,
                                  feedback.CustomerInfo.MSN));
            }
            return result;
        }
        #endregion

        #region update feedback
        public Result EditFeedback(Feedback feedback)
        {
            var entity = feedback;
            var ip = Helper.getClientIP();

            Result result = new Result();
            try
            {
                Check.Argument.IsValidIP(ip, "Ip");
                if (!_permissionService.GetHostPermission(Enumerations.Functions.Feedback.EditFeedback))
                {
                    result.ErrCode = (int)Enumerations.ErrorCode.UpdateFeedback;
                    result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblNoUpdateFeedbackPermission");
                    return result;
                }
                else if (!_permissionService.GetHostPermission(Enumerations.Functions.Feedback.FeedbackReview) && (feedback.UpdateReviewMode == (int)Enumerations.Functions.RevewMode.Update || feedback.UpdateReviewMode == (int)Enumerations.Functions.RevewMode.Remove))
                {
                    result.ErrCode = (int)Enumerations.ErrorCode.UpdateCustomerInfo;
                    result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblNoUpdateReviewFeedbackPermission");
                    return result;
                }
                if (feedback.Status.Id.Equals((int)Enumerations.Functions.CRMStatus.Deleted) && !_permissionService.GetHostPermission(Enumerations.Functions.Feedback.DeleteFeedback))
                {
                    result.ErrCode = (int)Enumerations.ErrorCode.DeleteFeedback;
                    result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblNoDeletedFeedbackPermission");
                    return result;
                }
                _repository.EditFeedback(entity, feedback.Creator.Id, ip,ref result);
            }
            catch (Exception ex)
            {
                result.ErrCode = -100;
                result.ErrMsg = ex.Message;

                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(
                    string.Format("(EditFeedback) -Error: {0}<br> - Creator: {1}<br> - CustomerId: " +
                                  "{2}<br> - FB Title: {3}<br> - FB Id: {4}<br> - FB Description: {5}" +
                                  "<br> - FB Solution: {6}<br> - FB Review: {7}<br> - Cust Info: " +
                                  "<br>   + FName: {8}<br>   + LName: {9}<br>   + Phone: {10}" +
                                  "<br>   + Fax: {11}<br>   + Skype: {12}<br>   + Mobile: {13}," +
                                  "<br>   + Email: {14}<br>   + MSN: {15}", ex.Message,
                                  feedback.Creator.Id, feedback.Customer.Id, feedback.Subject, feedback.Id,
                                  feedback.Description, feedback.Solution, feedback.Review,
                                  feedback.CustomerInfo.FirstName,
                                  feedback.CustomerInfo.LastName, feedback.CustomerInfo.Phone, feedback.CustomerInfo.Fax,
                                  feedback.CustomerInfo.Skype, feedback.CustomerInfo.MobilePhone,
                                  feedback.CustomerInfo.Email,
                                  feedback.CustomerInfo.MSN));
            }
            return result;
        }
        #endregion

        #region select feedback by custid
        public DataGridViewDTO SelFeedbackByCustId(int custId, string fromDate, string toDate, int pageIndex, int pageSize, bool fromCache)
        {
            IEnumerable<FeedbackView> lstFeedback = null;
            DataGridViewDTO feedbackdto = new DataGridViewDTO();
            try
            {
                if (fromCache)
                {
                    lstFeedback = (List<FeedbackView>)SessionExtension.GetSession(HttpContext.Current.Session, _LastedFeedbackByCustId) ??
                            SelAllFeedbackByCustId(custId, fromDate, toDate);
                }
                else
                    lstFeedback = SelAllFeedbackByCustId(custId, fromDate, toDate);


                var lstFeedbackPaging = new List<FeedbackView>();
                if (lstFeedback != null && lstFeedback.Count() > 0)
                {
                    lstFeedbackPaging = lstFeedback.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                    Parallel.ForEach(lstFeedbackPaging, feedback =>
                    {
                        feedback.MainCategory = _settingService.GetById(feedback.MainCategoryId).Value;
                        feedback.SubCategory = _settingService.GetById(feedback.SubCategoryId).Value;
                        feedback.Status = _settingService.GetById(feedback.StatusId).Value;                                                
                    });
                }

                feedbackdto.rows = lstFeedbackPaging;//.OrderBy(s => s.Id);
                feedbackdto.total = lstFeedback != null ? lstFeedback.Count() : 0;
                feedbackdto.pageindex = pageIndex;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(
                    string.Format("(SelFeedbackByCustId) - Error: {0}<br> - Custid: {1}<br> - Date: {2} -> {3}",
                                  ex.Message, custId, fromDate, toDate));

            }
            return feedbackdto;
        }

        private List<FeedbackView> SelAllFeedbackByCustId(int custId, string fromDate, string toDate)
        {
            var lstFeedback = _repository.GetFeedbackByCustId(custId, CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate)).ToList().ToList();
            var result = new List<FeedbackView>();

            if (lstFeedback != null && lstFeedback.Count > 0)
            {
                
                foreach (var feedback in lstFeedback)
                {
                    result.Add(new FeedbackView()
                    {
                        ID = feedback.Id,
                        Creator = feedback.Creator.Username,
                        Username = feedback.Customer.UserName,
                        FeedbackDate = feedback.FeedbackDate.ToString(),
                        Subject = feedback.Subject,
                        MainCategoryId = feedback.Category.Id,
                        SubCategoryId = feedback.SubCategory.Id,
                        StatusId = feedback.Status.Id,
                        Reviewer = feedback.Reviewer.Username,
                        ReviewDate = feedback.SolvedDate.ToString(),
                        RelatedUsername = feedback.RelatedCustomer.UserName,
                        CreateDate = feedback.CreateDate.ToString(),
                        DurationTime = feedback.ResolutionTime != null ? GetResolvingTime(feedback.ResolutionTime, feedback.FeedbackDate) : ""
                    });
                }
            }

            SessionExtension.AddSession(HttpContext.Current.Session, _LastedFeedbackByCustId, result);
            return result;
        }
        #endregion

        #region get latest feedback
        private List<FeedbackView> SelAllLastedFeedback(string fromDate, string toDate, string userName, int isGuest, int communicationID, int mainCategoryID, int subCategoryID, int statusID, int isReviewed, int responseId)
        {
            var lstFeedback = _repository.GetFeedback(CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate), userName, isGuest, communicationID, mainCategoryID, subCategoryID, statusID, isReviewed, responseId).ToList();
            var result = new List<FeedbackView>();            
            if (lstFeedback != null && lstFeedback.Count > 0)
            {
                foreach (var feedback in lstFeedback)
                {
                    var fbDate = CastAide.AsDateTime(feedback.FeedbackDate);
                    var rsDate = CastAide.AsDateTime(feedback.ResolutionTime);
                    result.Add(new FeedbackView()
                    {
                        ID = feedback.Id,
                        Creator = feedback.Creator.Username,
                        Username = feedback.Customer.UserName,
                        FeedbackDate = feedback.FeedbackDate,
                        Subject = feedback.Subject,
                        MainCategoryId = feedback.Category.Id,
                        SubCategoryId = feedback.SubCategory.Id,
                        StatusId = feedback.Status.Id,
                        Reviewer = feedback.Reviewer.Username,
                        ReviewDate = feedback.SolvedDate,
                        RelatedUsername = feedback.RelatedCustomer.UserName,
                        CreateDate = feedback.CreateDate,
                        DurationTime = feedback.ResolutionTime != null ? GetResolvingTime(feedback.ResolutionTime, feedback.FeedbackDate) : ""
                    });
                    
                }

            }

            SessionExtension.AddSession(HttpContext.Current.Session, _LastedFeedback, result);
            return result;
        }

        public DataGridViewDTO GetFeedback(string fromDate, string toDate, string userName, int isGuest, int communicationID, int mainCategoryID, int subCategoryID, int statusID, int isReviewed, int responseId, int pageIndex, int pageSize, bool fromCache)
        {
            var feedbackdto = new DataGridViewDTO();            
            try
            {
                List<FeedbackView> lstFeedback = null;
                if (fromCache)
                {
                    lstFeedback = (List<FeedbackView>)SessionExtension.GetSession(HttpContext.Current.Session, _LastedFeedback) ??
                        SelAllLastedFeedback(fromDate, toDate, userName, isGuest, communicationID, mainCategoryID, subCategoryID, statusID, isReviewed, responseId);
                }
                else
                    lstFeedback = SelAllLastedFeedback(fromDate, toDate, userName, isGuest, communicationID, mainCategoryID, subCategoryID, statusID, isReviewed, responseId);

                var lstFeedbackPaging = new List<FeedbackView>();
                if (lstFeedback != null && lstFeedback.Count() > 0)
                {
                    lstFeedbackPaging = lstFeedback.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

                    Parallel.ForEach(lstFeedbackPaging, feedback =>
                    {
                        feedback.MainCategory = _settingService.GetById(feedback.MainCategoryId).Value;
                        feedback.SubCategory = _settingService.GetById(feedback.SubCategoryId).Value;
                        feedback.Status = _settingService.GetById(feedback.StatusId).Value;
                    });
                }

                feedbackdto.rows = lstFeedbackPaging;//.OrderBy(s => s.Id);
                feedbackdto.total = lstFeedback != null ? lstFeedback.Count() : 0;
                feedbackdto.pageindex = pageIndex;
            }
            catch(Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                
                Helper.SendMailErrorNotice(
                    string.Format(
                        "(GetFeedback) - Error: {0}<br> - Username: {1}<br> - Date: {2} -> {3}<br> - IsGuest: {4}<br> - MainId: {5}<br> - SubId: {6}<br> - Status: {7}<br> - Review: {8}<br> - Response: {9}",
                        ex.Message, userName, fromDate, toDate, isGuest, mainCategoryID, subCategoryID, statusID,
                        isReviewed, responseId));
            }
            return feedbackdto;
        }
        #endregion
                
        #region get feedbackstatistic
        private IEnumerable<FeedbackStatisticsGroup> GetFeedbackStatistic(Params param, ref ArrayList columnName, ref ArrayList columnId, ref FeedbackStatistics grandTotal)
        {
            const int startColNumber = 4;
            var dataTables = _repository.GetFeedbackStatistic(param);
            var list = new List<FeedbackStatisticsGroup>();

            if (dataTables.Count < 2)
                return list;

            var dataTable = dataTables[0];
            var dataTableId = dataTables[1];

            var totalComlumns = dataTable.Columns.Count;
            var totalItems = dataTable.Rows.Count;

            if (totalItems > 0)
            {
                #region assign columnName, columnId

                for (var i = startColNumber; i < totalComlumns; i++)
                {
                    columnId.Add(CastAide.AsInt32(dataTableId.Rows[i - startColNumber][0].ToString()));
                    columnName.Add(System.Web.HttpUtility.HtmlEncode(dataTable.Columns[i].ColumnName));
                    dataTable.Columns[i].ColumnName = "ColumnName" + dataTable.Columns[i].ColumnName.Trim().Replace(" ", "");
                }
                #endregion

                #region assign all data
                var mainCategories =
                    dataTable.AsEnumerable().Select(s => CastAide.AsInt32(s["MainCategoryID"].ToString())).Distinct().ToList();
                FeedbackStatistics feedbackStatistic;
                FeedbackStatisticsGroup feedbackStatisticGroup;
                ArrayList cells;

                foreach (var mainCategoryId in mainCategories)
                {
                    feedbackStatisticGroup = new FeedbackStatisticsGroup();
                    feedbackStatisticGroup.FeedbackStatistics = new List<FeedbackStatistics>();
                    var getFeedbackByCategoryId =
                        dataTable.AsEnumerable().Where(x => x.Field<int>("MainCategoryID") == mainCategoryId).ToList();
                    var mainCategory = string.Empty;
                    #region items
                    float totalCellsByMainCategoryID = 0;
                    foreach (var feedbackGroup in getFeedbackByCategoryId)
                    {
                        mainCategory = CastAide.AsString(feedbackGroup["MainCategory"]);
                        #region asign data to cells field
                        float totalCellsBySubId = 0;
                        cells = new ArrayList();
                        for (var i = startColNumber; i < totalComlumns; i++)
                        {
                            cells.Add(CastAide.AsSingle(feedbackGroup[i]));
                            totalCellsBySubId += CastAide.AsSingle(feedbackGroup[i]);
                            totalCellsByMainCategoryID += CastAide.AsSingle(feedbackGroup[i]);
                        }
                        #endregion

                        var subCategoryValue = string.Format("{0} ({1})", CastAide.AsString(feedbackGroup["SubCategory"]), totalCellsBySubId);
                        feedbackStatistic = new FeedbackStatistics()
                        {
                            MainCategory = mainCategory,
                            MainCategoryId = mainCategoryId,
                            SubCategory = subCategoryValue,
                            SubCategoryId = CastAide.AsInt32(feedbackGroup["SubCategoryID"]),
                            Cells = cells,
                            IsTotal = false
                        };
                        feedbackStatisticGroup.FeedbackStatistics.Add(feedbackStatistic);
                    }
                    feedbackStatisticGroup.TotalMembers = feedbackStatisticGroup.FeedbackStatistics.Count;
                    #endregion

                    #region add subtotal
                    cells = new ArrayList();
                    for (var i = startColNumber; i < totalComlumns; i++)
                    {
                        var total = getFeedbackByCategoryId.AsEnumerable().
                            Sum(x => x.Field<int>(dataTable.Columns[i].ColumnName.Trim()));

                        cells.Add(total);
                    }
                    feedbackStatistic = new FeedbackStatistics()
                    {
                        MainCategory = mainCategory,
                        MainCategoryId = mainCategoryId,
                        SubCategory = string.Format("{0} ({1})", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblsubTotal"), totalCellsByMainCategoryID),
                        SubCategoryId = 0,
                        Cells = cells,
                        IsTotal = true
                    };
                    feedbackStatisticGroup.FeedbackStatistics.Add(feedbackStatistic);

                    feedbackStatisticGroup.Title = string.Format("{0} - {1} item(s)", mainCategory, feedbackStatisticGroup.TotalMembers);
                    #endregion

                    list.Add(feedbackStatisticGroup);
                }

                #region grand total
                cells = new ArrayList();
                for (var i = startColNumber; i < totalComlumns; i++)
                {
                    var total =
                        dataTable.AsEnumerable().Sum(
                            x => x.Field<int>(dataTable.Columns[i].ColumnName.Trim()));

                    cells.Add(total);
                }

                feedbackStatistic = new FeedbackStatistics()
                {
                    MainCategory = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "grandTotal"),
                    MainCategoryId = 0,
                    SubCategory = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "grandTotal"),
                    SubCategoryId = 0,
                    Cells = cells
                };
                grandTotal = feedbackStatistic;

                #endregion
                #endregion
            }

            return list;
        }

        private FeedbackStatisticsDTO SelAllFeedbackStatistic(Params param)
        {
            var result = new FeedbackStatisticsDTO();
            var columnNames = new ArrayList();
            var columnIds = new ArrayList();
            var grandTotal = new FeedbackStatistics();
            result.rows = GetFeedbackStatistic(param, ref columnNames, ref columnIds, ref grandTotal);
            result.ColumnNames = columnNames;
            result.ColumnIDs = columnIds;
            result.Total = result.rows.Count();
            result.GrandTotal = grandTotal;

            SessionExtension.AddSession(HttpContext.Current.Session, _FeedbackStatisticSessionKey, result);
            return result;
        }

        public FeedbackStatisticsDTO GetFeedbackStatistic(Params param)
        {
            var result = new FeedbackStatisticsDTO();
            var pagingResult = new FeedbackStatisticsDTO();
            try
            {
                #region get data
                if (param.FromCache)
                    result = SessionExtension.GetSession(HttpContext.Current.Session, _FeedbackStatisticSessionKey) != null
                                 ? (FeedbackStatisticsDTO)
                                   SessionExtension.GetSession(HttpContext.Current.Session, _FeedbackStatisticSessionKey)
                                 : SelAllFeedbackStatistic(param);

                else
                    result = SelAllFeedbackStatistic(param);

                #endregion

                #region paging
                if (result != null && result.rows != null && result.rows.Count() > 0)
                {
                    pagingResult.rows = result.rows.Select(c => c).Skip(param.ItemPerPage * (param.CurrentPage - 1)).Take(param.ItemPerPage).ToList();
                    pagingResult.GrandTotal = result.GrandTotal;
                    pagingResult.PageIndex = result.PageIndex;
                    pagingResult.Total = result.Total;
                    pagingResult.ColumnIDs = result.ColumnIDs;
                    pagingResult.ColumnNames = result.ColumnNames;
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(
                    string.Format("(GetFeedbackStatistic) - Error: {0}<br> - MainId: {1}<br> - SubId: {2}<br> - ValueId: {3}<br> - Type: {4}<br> - Date: {5}->{6}", ex.Message,
                                  param.MainId, param.SubId, param.ValueId, param.Type, param.FromDate, param.ToDate));
            }
            return pagingResult;
        }
        #endregion

        #region get feedback statistic detail
        ////get all data - not set value yet
        private List<Feedback> SelAllFeedbackStatisticDetail(Params param)
        {
            var lstFeedback = new List<Feedback>();
           
            var totalRows = 0;
            lstFeedback = _repository.GetFeedbackStatisticDetail(param, ref totalRows).ToList();

            #region using background worker for mapping data

            Task taskMappingFeedbackStatisticDetailData = Task.Factory.StartNew(() =>
            {
                CacheService.Delete(_FeedbackStatisticDetailMappedSessionKey);
                var mappingData = MapperFeedbackStatisticReportDetail(lstFeedback);
                CacheService.Set(_FeedbackStatisticDetailMappedSessionKey, mappingData);
            });
            #endregion
            SessionExtension.AddSession(HttpContext.Current.Session, _FeedbackStatisticDetailSessionKey, lstFeedback);
            
            return lstFeedback;
        }
        private DataGridViewDTO MapperFeedbackStatisticReportDetail(List<Feedback> feedbackDetail)
        {
            var feedbackStatisticDetailDto = new DataGridViewDTO();

            var feedbackViewPaging = new List<FeedbackView>();
            foreach (var feedback in feedbackDetail)
            {
                var fbDate = CastAide.AsDateTime(feedback.FeedbackDate);
                var rsDate = CastAide.AsDateTime(feedback.ResolutionTime);
                feedbackViewPaging.Add(new FeedbackView()
                {
                    ID = feedback.Id,
                    Creator = feedback.Creator.Username,
                    Username = feedback.Customer.UserName,
                    FeedbackDate = feedback.FeedbackDate,
                    Subject = feedback.Subject,
                    MainCategoryId = feedback.Category.Id,
                    MainCategory = _settingService.GetById(feedback.Category.Id).Value,
                    SubCategoryId = feedback.SubCategory.Id,
                    SubCategory = _settingService.GetById(feedback.SubCategory.Id).Value,
                    StatusId = feedback.Status.Id,
                    Status = _settingService.GetById(feedback.Status.Id).Value,
                    Reviewer = feedback.Reviewer.Username,
                    ReviewDate = feedback.SolvedDate,
                    RelatedUsername = feedback.RelatedCustomer.UserName,
                    CreateDate = feedback.CreateDate,
                    DurationTime = feedback.ResolutionTime != null ? GetResolvingTime(feedback.ResolutionTime, feedback.FeedbackDate)  : ""
                });
            }

            feedbackStatisticDetailDto.rows = feedbackViewPaging;
            feedbackStatisticDetailDto.total = feedbackViewPaging.Count;
            feedbackStatisticDetailDto.pageindex = 1;
            
            return feedbackStatisticDetailDto;
            
        }
        public DataGridViewDTO GetFeedbackStatisticDetail(Params param)
        {
            var result = new List<Feedback>();
            var pagingResult = new DataGridViewDTO();
            try
            {
                #region get data
                if (param.FromCache)
                {
                    var getMappedResult = CacheService.Get<DataGridViewDTO>(_FeedbackStatisticDetailMappedSessionKey);
                    if (getMappedResult != null && getMappedResult.rows != null && getMappedResult.rows.Count > 0)
                    {
                        var pagingMappedResult = new DataGridViewDTO()
                        {
                            rows = getMappedResult.rows.Select(c => c).Skip(param.ItemPerPage * (param.CurrentPage - 1)).Take(param.ItemPerPage).ToList(),
                            pageindex = param.CurrentPage,
                            total = getMappedResult.total
                        };
                        return pagingMappedResult;
                    }
                    else
                    {
                        result = SessionExtension.GetSession(HttpContext.Current.Session, _FeedbackStatisticDetailSessionKey) !=
                                 null
                                     ? (List<Feedback>)
                                       SessionExtension.GetSession(HttpContext.Current.Session,
                                                                   _FeedbackStatisticDetailSessionKey)
                                     : SelAllFeedbackStatisticDetail(param);
                    }
                }
                else
                    result = SelAllFeedbackStatisticDetail(param);

                #endregion

                #region paging

                var lstFeedbackPaging = new List<Feedback>();
                if (result != null && result.Count > 0)
                {
                    lstFeedbackPaging = result.Select(c => c).Skip(param.ItemPerPage * (param.CurrentPage - 1)).Take(param.ItemPerPage).ToList();

                    var feedbackViewPaging = new List<FeedbackView>();
                    foreach (var feedback in lstFeedbackPaging)
                    {
                        var fbDate = CastAide.AsDateTime(feedback.FeedbackDate);
                        var rsDate = CastAide.AsDateTime(feedback.ResolutionTime);
                        feedbackViewPaging.Add(new FeedbackView()
                        {
                            ID = feedback.Id,
                            Creator = feedback.Creator.Username,
                            Username = feedback.Customer.UserName,
                            FeedbackDate = feedback.FeedbackDate,
                            Subject = feedback.Subject,
                            MainCategoryId = feedback.Category.Id,
                            SubCategoryId = feedback.SubCategory.Id,
                            StatusId = feedback.Status.Id,
                            Reviewer = feedback.Reviewer.Username,
                            ReviewDate = feedback.SolvedDate,
                            RelatedUsername = feedback.RelatedCustomer.UserName,
                            CreateDate = feedback.CreateDate,
                            DurationTime = feedback.ResolutionTime != null ? GetResolvingTime(feedback.ResolutionTime, feedback.FeedbackDate) : ""
                        });
                    }

                    Parallel.ForEach(feedbackViewPaging, feedback =>
                    {
                        feedback.MainCategory = _settingService.GetById(feedback.MainCategoryId).Value;
                        feedback.SubCategory = _settingService.GetById(feedback.SubCategoryId).Value;
                        feedback.Status = _settingService.GetById(feedback.StatusId).Value;
                    });

                    pagingResult.rows = feedbackViewPaging;
                    pagingResult.pageindex = param.CurrentPage;
                    pagingResult.total = result.Count;
                }

                #endregion
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);                
                Helper.SendMailErrorNotice(
                    string.Format("(GetFeedbackStatisticDetail) - Error: {0}<br> - MainId: {1}<br> - SubId: {2}<br> - ValueId: {3}<br> - Type: {4}<br> - Date: {5}->{6}", ex.Message,
                                  param.MainId, param.SubId, param.ValueId, param.Type, param.FromDate, param.ToDate));
            }
            return pagingResult;
        }
        #endregion

        #region get deleted feedback
        private List<FeedbackView> SelAllDeletedFeedback(string fromDate, string toDate, string userName, int isGuest, int communicationID, int mainCategoryID, int subCategoryID, int isReviewed, int responseId)
        {
            var lstFeedback = _repository.GetDeletedFeedback(CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate), userName, isGuest, communicationID, mainCategoryID, subCategoryID, isReviewed, responseId).ToList();
            var result = new List<FeedbackView>();

            if (lstFeedback != null && lstFeedback.Count > 0)
            {                
                foreach (Feedback feedback in lstFeedback)
                {
                    result.Add(new FeedbackView()
                    {
                        ID = feedback.Id,
                        Creator = feedback.Creator.Username,
                        Username = feedback.Customer.UserName,
                        FeedbackDate = feedback.FeedbackDate,
                        Subject = feedback.Subject,
                        MainCategoryId = feedback.Category.Id,
                        SubCategoryId = feedback.SubCategory.Id,
                        StatusId = feedback.Status.Id,
                        Reviewer = feedback.Reviewer.Username,
                        ReviewDate = feedback.SolvedDate,
                        RelatedUsername = feedback.RelatedCustomer.UserName,
                        CreateDate = feedback.CreateDate,
                        DurationTime = feedback.ResolutionTime != null ? (CastAide.AsDateTime(feedback.ResolutionTime) - CastAide.AsDateTime(feedback.FeedbackDate)).ToString() : ""
                    });
                }
            }

            SessionExtension.AddSession(HttpContext.Current.Session, _DeletedFeedback, result);
            return result;
        }

        public DataGridViewDTO GetDeletedFeedback(string fromDate, string toDate, string userName, int isGuest, int communicationID, int mainCategoryID, int subCategoryID, int isReviewed, int responseId, int pageIndex, int pageSize, bool fromCache)
        {
            var feedbackdto = new DataGridViewDTO();
            //var feedbackPaging = new DataGridViewDTO();
            try
            {
                List<FeedbackView> lstFeedback = null;
                if (fromCache)
                {
                    lstFeedback = (List<FeedbackView>)SessionExtension.GetSession(HttpContext.Current.Session, _DeletedFeedback) ??
                        SelAllDeletedFeedback(fromDate, toDate, userName, isGuest, communicationID, mainCategoryID, subCategoryID, isReviewed, responseId);
                }
                else
                    lstFeedback = SelAllDeletedFeedback(fromDate, toDate, userName, isGuest, communicationID, mainCategoryID, subCategoryID, isReviewed, responseId);

                var lstFeedbackPaging = new List<FeedbackView>();
                if (lstFeedback != null && lstFeedback.Count() > 0)
                {
                    lstFeedbackPaging = lstFeedback.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                    Parallel.ForEach(lstFeedbackPaging, feedback =>
                    {
                        feedback.MainCategory = _settingService.GetById(feedback.MainCategoryId).Value;
                        feedback.SubCategory = _settingService.GetById(feedback.SubCategoryId).Value;
                        feedback.Status = _settingService.GetById(feedback.StatusId).Value;
                    });
                }

                feedbackdto.rows = lstFeedbackPaging;//.OrderBy(s => s.Id);
                feedbackdto.total = lstFeedback != null ? lstFeedback.Count() : 0;
                feedbackdto.pageindex = pageIndex;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                
                Helper.SendMailErrorNotice(
                    string.Format(
                        "(GetDeletedFeedback) - Error: {0}<br> - Username: {1}<br> - Date: {2} -> {3}<br> - IsGuest: {4}<br> - MainId: {5}<br> - SubId: {6}<br> - Review: {7}<br> - Response: {8}",
                        ex.Message, userName, fromDate, toDate, isGuest, mainCategoryID, subCategoryID,
                        isReviewed, responseId));
            }
            return feedbackdto;
        }
        #endregion

        #region Get Feedback Subject Items
        public List<FeedbackTemplate> GetFeedbackSubjectTemplateItems(int templateId = 0)
        {   
            List<FeedbackTemplate> listFeedbackTemplate = new List<FeedbackTemplate>();
            try
            {
                var dsResult = _repository.GetFeedbackSubjectTemplateItems(templateId);
                if (dsResult != null && dsResult.Tables.Count > 0)
                {
                    foreach (DataRow dr in dsResult.Tables[0].Rows)
                    {
                        listFeedbackTemplate.Add(new FeedbackTemplate
                                                     {
                                                         TemplateId = CastAide.AsInt32(dr["templateID"]),
                                                         Subject = CastAide.AsString(dr["subject"])
                                                     }
                                                );
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return listFeedbackTemplate;
        }
        public int GetNumOfFeedbackSubjectTemplateItem()
        {
            List<FeedbackTemplate> listFeedbackTemplate = GetFeedbackSubjectTemplateItems(0);
            try
            {
                CacheService.Set("AllFeedbackTemplate", listFeedbackTemplate);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return listFeedbackTemplate.Count;
        }
        public List<FeedbackTemplate> GetFeedbackSubjectTemplateItemsRange(int range)
        {
            int multipleNum = 100000;
            var result = new List<FeedbackTemplate>();
            try
            {
                List<FeedbackTemplate> listFeedbackTemplate = CacheService.Get<List<FeedbackTemplate>>("AllFeedbackTemplate");
                if(listFeedbackTemplate!=null && listFeedbackTemplate.Count > 0)
                    result = listFeedbackTemplate.Skip((range - 1) * multipleNum).Take(range * multipleNum).ToList();                
            }
            catch(Exception ex){
                LogExtensions.ExceptionHandler(ex);                
            }

            return result;
        }
         
        #endregion

        #region Handle Feedback Subject Items
        public Result AddSubjectFeedbackTemplate(string subject)
        {
            Result result = new Result(0, string.Empty);
            try
            {
                if (!_permissionService.GetHostPermission(Enumerations.Functions.AdminTool.FBSubjectTemplateManagement))
                {
                    result.ErrCode = 500;
                    result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblNoInsertFBSubjectTemplate");
                    return result;
                }
                result = _repository.AddSubjectFeedbackTemplate(subject);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(
                    string.Format(
                        "(Subject Feedback Template Insert) - Error: {0}<br> - Subject Template: {1}<br> ",
                        ex.Message, subject));
            }
            return result;
        }
        public Result EditSubjectFeedbackTemplate(int id, string newSubject)
        {
            Result result = new Result(0, string.Empty);
            try
            {
                if (!_permissionService.GetHostPermission(Enumerations.Functions.AdminTool.FBSubjectTemplateManagement))
                {
                    result.ErrCode = 500;
                    result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblNoUpdateFBSubjectTemplate");
                    return result;
                }
                result = _repository.EditSubjectFeedbackTemplate(id, newSubject);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(
                    string.Format(
                        "(Subject Feedback Template Insert) - Error: {0}<br> - Subject Template ID: {1}<br> Subject Template : {2} <br> ",
                        ex.Message, id, newSubject));
            }
            return result;
        }
        public Result DeleteSubjectFeedbackTemplate(int id)
        {
            Result result = new Result(0, string.Empty);
            try
            {
                if (!_permissionService.GetHostPermission(Enumerations.Functions.AdminTool.FBSubjectTemplateManagement))
                {
                    result.ErrCode = 500;
                    result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblNoDeleteFBSubjectTemplate");
                    return result;
                }
                result = _repository.DeleteSubjectFeedbackTemplate(id);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(
                    string.Format(
                        "(Subject Feedback Template Delete) - Error: {0}<br> - Subject Template Id: {1}<br> ",
                        ex.Message, id));
            }
            return result;
        }
        public FeedbackTemplateDGViewDTO SelFeedbackTemplatePageWithSearch(string searchString, int pageIndex, int pageSize)
        {
            FeedbackTemplateDGViewDTO tempDGView = new FeedbackTemplateDGViewDTO();
            try
            {

                List<FeedbackTemplate> lstFeedbackTemplate = GetFeedbackSubjectTemplateItems(0).Where(c => c.Subject.ToLower().Contains(searchString.ToLower())).ToList();

                var lstFeedbackPaging = new List<FeedbackTemplate>();
                if (lstFeedbackTemplate != null && lstFeedbackTemplate.Count > 0)
                    lstFeedbackPaging = lstFeedbackTemplate.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

                tempDGView.rows = lstFeedbackPaging;//.OrderBy(s => s.Id);
                tempDGView.total = lstFeedbackTemplate != null ? lstFeedbackTemplate.Count() : 0;
                tempDGView.pageindex = pageIndex;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return tempDGView;
        }
        #endregion

        #region Select top feedback
        private List<FeedbackView> SelAllTopFeedbackByCustId(int custId, int selectTop)
        {
            var lstFeedback = _repository.SelTopFeedbackByCustId(custId, selectTop).ToList();
            var result = new List<FeedbackView>();

            if (lstFeedback != null && lstFeedback.Count > 0)
            {
                foreach (var feedback in lstFeedback)
                {
                    result.Add(new FeedbackView()
                    {
                        ID = feedback.Id,
                        Creator = feedback.Creator.Username,
                        Username = feedback.Customer.UserName,
                        FeedbackDate = feedback.FeedbackDate,
                        Subject = feedback.Subject,
                        MainCategoryId = feedback.Category.Id,
                        SubCategoryId = feedback.SubCategory.Id,
                        StatusId = feedback.Status.Id,
                        Reviewer = feedback.Reviewer.Username,
                        ReviewDate = feedback.SolvedDate,
                        RelatedUsername = feedback.RelatedCustomer.UserName,
                        CreateDate = feedback.CreateDate,
                        DurationTime = feedback.ResolutionTime != null ? GetResolvingTime(feedback.ResolutionTime, feedback.FeedbackDate) : "",
                        Description = feedback.Description
                    });
                }
            }

            SessionExtension.AddSession(HttpContext.Current.Session, _LastedTopFeedbackByCustId, result);
            return result;
        }

        public DataGridViewDTO SelTopFeedbackByCustId(int custId, int selectTop, int pageIndex, int pageSize, bool fromCache)
        {
            List<FeedbackView> lstFeedback = null;
            var feedbackdto = new DataGridViewDTO();
            try
            {
                if (fromCache)
                    lstFeedback = (List<FeedbackView>)SessionExtension.GetSession(HttpContext.Current.Session, _LastedTopFeedbackByCustId) ??
                            SelAllTopFeedbackByCustId(custId, selectTop);
                else
                    lstFeedback = SelAllTopFeedbackByCustId(custId, selectTop);


                var lstFeedbackPaging = new List<FeedbackView>();
                if (lstFeedback != null && lstFeedback.Count() > 0)
                {
                    lstFeedbackPaging = lstFeedback.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

                    Parallel.ForEach(lstFeedbackPaging, feedback =>
                    {
                        feedback.MainCategory = _settingService.GetById(feedback.MainCategoryId).Value;
                        feedback.SubCategory = _settingService.GetById(feedback.SubCategoryId).Value;
                        feedback.Status = _settingService.GetById(feedback.StatusId).Value;
                    });
                }

                feedbackdto.rows = lstFeedbackPaging;//.OrderBy(s => s.Id);
                feedbackdto.total = lstFeedback != null ? lstFeedback.Count() : 0;
                feedbackdto.pageindex = pageIndex;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(
                    string.Format("(SelTopFeedbackByCustId) - Error: {0}<br> - CustId: {1}<br> - Top: {2}", ex.Message,
                                  custId, selectTop));
            }
            return feedbackdto;
        }
        
        private List<FeedbackView> SelAllTopFeedback(int selectTop)
        {
            var lstFeedback = _repository.SelTopFeedback(selectTop).ToList();
            var result = new List<FeedbackView>();

            if (lstFeedback != null && lstFeedback.Count > 0)
            {
                foreach (var feedback in lstFeedback)
                {
                    result.Add(new FeedbackView()
                    {
                        ID = feedback.Id,
                        Creator = feedback.Creator.Username,
                        Username = feedback.Customer.UserName,
                        FeedbackDate = feedback.FeedbackDate,
                        Subject = feedback.Subject,
                        MainCategoryId = feedback.Category.Id,
                        SubCategoryId = feedback.SubCategory.Id,
                        StatusId = feedback.Status.Id,
                        Reviewer = feedback.Reviewer.Username,
                        ReviewDate = feedback.SolvedDate,
                        RelatedUsername = feedback.RelatedCustomer.UserName,
                        CreateDate = feedback.CreateDate,
                        DurationTime = feedback.ResolutionTime != null ? GetResolvingTime(feedback.ResolutionTime, feedback.FeedbackDate) : "",
                        Description = feedback.Description
                    });
                }
            }

            SessionExtension.AddSession(HttpContext.Current.Session, _LastedTopFeedback, result);
            return result;
        }

        public DataGridViewDTO SelTopFeedback(int selectTop, int pageIndex, int pageSize, bool fromCache)
        {
            List<FeedbackView> lstFeedback = null;
            var feedbackdto = new DataGridViewDTO();
            try
            {
                if (fromCache)
                    lstFeedback = (List<FeedbackView>)SessionExtension.GetSession(HttpContext.Current.Session, _LastedTopFeedback) ??
                            SelAllTopFeedback(selectTop);
                else
                    lstFeedback = SelAllTopFeedback(selectTop);


                var lstFeedbackPaging = new List<FeedbackView>();
                if (lstFeedback != null && lstFeedback.Count() > 0)
                {
                    lstFeedbackPaging = lstFeedback.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                    
                    Parallel.ForEach(lstFeedbackPaging, feedback =>
                    {
                        feedback.MainCategory = _settingService.GetById(feedback.MainCategoryId).Value;
                        feedback.SubCategory = _settingService.GetById(feedback.SubCategoryId).Value;
                        feedback.Status = _settingService.GetById(feedback.StatusId).Value;
                    });
                }

                feedbackdto.rows = lstFeedbackPaging;//.OrderBy(s => s.Id);
                feedbackdto.total = lstFeedback != null ? lstFeedback.Count() : 0;
                feedbackdto.pageindex = pageIndex;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(SelTopFeedback) - Error: {0}<br> - Top: {1}", ex.Message, selectTop));
            }
            return feedbackdto;
        }
        #endregion

        #region ibc daily report
        public DailySummaryBO GetIBCDailySummaryReport(string fromDate, string toDate)
        {
            var dailySummary = new DailySummaryBO();
            try
            {
                var dsReport = _repository.GetIBCDailySummaryReport(
                    CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate), Enumerations.ReportTypes.CreditSite);

                dailySummary.DailySummaries = new List<DailySummaryGroup>();
                dailySummary.ReportDay = CastAide.AsDateTime(fromDate);
                if (dsReport != null && dsReport.Tables != null && dsReport.Tables.Count > 0)
                {
                    var startCol = 4;
                    var tbData = dsReport.Tables[0];
                    var totalCols = tbData.Columns.Count;

                    var oldMainId = 0;
                    var oldMain = string.Empty;
                    var subTotal = new int[totalCols];
                    var grandTotal = new int[totalCols];

                    #region set data for subtotal & grand total
                    var dailyGroup = new DailySummaryGroup();
                    dailyGroup.DailySummaries = new List<DailySummaryDetail>();
                    foreach (DataRow dr in tbData.Rows)
                    {
                        var currentMainId = CastAide.AsInt32(dr["MainCategoryID"].ToString());
                        var currentMain = CastAide.AsString(dr["MainCategory"]);
                        var currentSummary = new DailySummaryDetail();
                        currentSummary.Cells = new List<DailySummaryType>();
                        currentSummary.MainCategory = CastAide.AsString(dr["SubCategory"]);
                        #region assign data
                        if (currentMainId != oldMainId && oldMainId != 0)
                        {
                            var subTotalCells = new List<DailySummaryType>();
                            var cellsTotal = 0;
                            for (int j = startCol; j < totalCols; j++)
                            {
                                subTotalCells.Add(new DailySummaryType()
                                {
                                    Title = CastAide.AsString(dr.Table.Columns[j].ColumnName),
                                    Value = subTotal[j]
                                });
                                cellsTotal += subTotal[j];
                                subTotal[j] = 0;
                            }
                            var summaryDetail = new DailySummaryDetail()
                            {
                                MainCategory = oldMain,
                                MainCategoryId = oldMainId,
                                Cells = subTotalCells,
                                Total = cellsTotal
                            };
                            dailyGroup.Total = summaryDetail;
                            dailySummary.DailySummaries.Add(dailyGroup);
                            dailyGroup = new DailySummaryGroup();
                            dailyGroup.DailySummaries = new List<DailySummaryDetail>();
                        }
                        #endregion

                        for (int i = startCol; i < totalCols; i++)
                        {
                            subTotal[i] = subTotal[i] + CastAide.AsInt32(dr[i]);
                            grandTotal[i] = grandTotal[i] + CastAide.AsInt32(dr[i]);

                            currentSummary.Cells.Add(new DailySummaryType()
                            {
                                Value = CastAide.AsInt32(dr[i])
                            });
                            currentSummary.Total += CastAide.AsInt32(dr[i]);

                        }
                        dailyGroup.DailySummaries.Add(currentSummary);
                        oldMainId = currentMainId;
                        oldMain = currentMain;
                    }
                    #endregion

                    #region assign data for last subtotal & grand total
                    var lastSubTotalCells = new List<DailySummaryType>();
                    var grandTotalCells = new List<DailySummaryType>();
                    var lastCellsTotal = 0;
                    var cellsGrandTotal = 0;
                    for (int j = startCol; j < totalCols; j++)
                    {
                        var cellName = tbData.Columns[j].ColumnName;
                        lastSubTotalCells.Add(new DailySummaryType()
                        {
                            Title = CastAide.AsString(cellName),
                            Value = subTotal[j]
                        });
                        grandTotalCells.Add(new DailySummaryType()
                        {
                            Title = CastAide.AsString(cellName),
                            Value = grandTotal[j]
                        });
                        lastCellsTotal += subTotal[j];
                        cellsGrandTotal += grandTotal[j];
                    }

                    dailyGroup.Total = new DailySummaryDetail()
                    {
                        MainCategory = oldMain,
                        MainCategoryId = oldMainId,
                        Cells = lastSubTotalCells,
                        Total = lastCellsTotal
                    };
                    dailySummary.DailySummaries.Add(dailyGroup);
                    dailySummary.GrandTotal = new DailySummaryDetail()
                    {
                        MainCategory = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "grandTotal"),
                        MainCategoryId = 0,
                        Cells = grandTotalCells,
                        Total = cellsGrandTotal
                    };
                    #endregion
                }

                SessionExtension.AddSession(HttpContext.Current.Session, _IbcDailySummary, dailySummary);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format(
                    "(GetIBCDailySummaryReport) - Error {0}<br> - Date: {1} -> {2}", ex.Message, fromDate, toDate));
            }
            return dailySummary;
        }

        public DailyComplaintsDTO GetIBCDailyDetailReport(string fromDate, string toDate, bool fromCache, int itemPerPage, int currentPage)
        {
            var dailyComplaintsData = new DailyComplaintsDTO();
            dailyComplaintsData.DailyComplaints = new List<DailyComplaintsDetail>();
            try
            {
                if (fromCache)
                {
                    var getMappedResult = CacheService.Get<DailyComplaintsDTO>(_IbcDailyDetail);
                    if (getMappedResult != null && getMappedResult.DailyComplaints != null && getMappedResult.DailyComplaints.Count > 0)
                    {
                        var pagingMappedResult = new DailyComplaintsDTO()
                        {
                            DailyComplaints = getMappedResult.DailyComplaints.Select(c => c).Skip(itemPerPage * (currentPage - 1)).Take(itemPerPage).ToList(),

                        };
                        pagingMappedResult.pageindex = currentPage;
                        pagingMappedResult.total = getMappedResult.DailyComplaints.Count;
                        return pagingMappedResult;
                    }
                    else
                    {
                        var sessionData = (DataTable)SessionExtension.GetSession(HttpContext.Current.Session, _IbcDailyDetailNotMapped);
                        if (sessionData != null)
                        {
                            var paging = sessionData.AsEnumerable().Select(c => c).Skip(itemPerPage * (currentPage - 1)).Take(itemPerPage);
                            dailyComplaintsData = MapperIBCDailyDetailReport(paging);
                            dailyComplaintsData.pageindex = currentPage;
                            dailyComplaintsData.total = sessionData.Rows.Count;

                            return dailyComplaintsData;
                        }
                    }
                }

                CacheService.Delete(_IbcDailyDetail);
                CacheService.Delete(_IbcDailyDetailNotMapped);
                var dsReport = _repository.GetIBCDailyDetailReport(CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate), Enumerations.ReportTypes.CreditSite);
                if (dsReport != null && dsReport.Tables != null && dsReport.Tables.Count > 0)
                {
                    var paginOnNewData = dsReport.Tables[0].AsEnumerable().Select(c => c).Skip(itemPerPage * (currentPage - 1)).Take(itemPerPage);
                    dailyComplaintsData = MapperIBCDailyDetailReport(paginOnNewData);
                    if (dsReport.Tables[0].Rows.Count <= itemPerPage)
                        CacheService.Set(_IbcDailyDetail, dailyComplaintsData);
                    else
                    {
                        SessionExtension.AddSession(HttpContext.Current.Session, _IbcDailyDetailNotMapped, dsReport.Tables[0]);

                        Task taskMappingIbcDetailData = Task.Factory.StartNew(() =>
                                {                                    
                                    var mappingData = MapperIBCDailyDetailReport(dsReport.Tables[0].AsEnumerable().Select(c => c));
                                    CacheService.Set(_IbcDailyDetail, mappingData);
                                });
                    }
                    dailyComplaintsData.pageindex = currentPage;
                    dailyComplaintsData.total = dsReport.Tables[0].Rows.Count;
                }
            }
            catch(Exception ex) 
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format(
                    "(GetIBCDailyDetailReport) - Error: {0}<br> - Date: {1} -> {2}", ex.Message, fromDate, toDate));
            }
            return dailyComplaintsData;
        }
        #region mapping ibc daily report      

        private DailyComplaintsDTO MapperIBCDailyDetailReport(IEnumerable<DataRow> dt) 
        {
            var dailyComplaintsData = new DailyComplaintsDTO();
            dailyComplaintsData.DailyComplaints = new List<DailyComplaintsDetail>();
            foreach (DataRow dr in dt)
            {
                var complaint = Helper.RemoveSpecialHtmlTag(CastAide.AsString(dr["Description"]));
                var solution = Helper.RemoveSpecialHtmlTag(CastAide.AsString(dr["Solution"]));

                dailyComplaintsData.DailyComplaints.Add(
                    new DailyComplaintsDetail()
                        {
                            FeedbackDate = DateTimeUtilize.ChangeTimeZoneToClientDate(dr["FeedbackDate"].ToString()),
                            FeedbackTime = DateTimeUtilize.ChangeTimeZoneToClientTime(dr["FeedbackDate"].ToString()),
                            CustomerName = CastAide.AsString(dr["UserName"]),
                            Licensee = _settingService.GetById(CastAide.AsInt32(dr["SiteID"])).Value,
                            CommunicationTool =
                                new CommunicationTool()
                                    {
                                        ContactBy = _settingService.GetById(CastAide.AsInt32(dr["CommunicationID"])).Value,
                                        Address =
                                            string.Format(
                                                "MSN: {0}\n<br> Skype: {1}\n<br> Email: {2}\n<br> Phone: {3}\n<br> Mobile: {4}",
                                                CastAide.AsString(dr["msn"]),
                                                CastAide.AsString(dr["Skype"]),
                                                CastAide.AsString(dr["Email"]),
                                                CastAide.AsString(dr["Phone"]),
                                                CastAide.AsString(dr["MobilePhone"]))
                                    },
                            ProductName = _settingService.GetById(CastAide.AsInt32(dr["ProductID"])).Value,
                            ExtNo = _settingService.GetById(CastAide.AsInt32(dr["StationID"])).Value,
                            Country = _countryService.GetById(CastAide.AsInt32(dr["CountryID"])).CountryName,
                            Category = _settingService.GetById(CastAide.AsInt32(dr["MainCategoryID"])).Value,
                            Complaints = complaint,
                            Solution = solution,
                            Status = _settingService.GetById(CastAide.AsInt32(dr["StatusID"])).Value,
                            Response = _settingService.GetById(CastAide.AsInt32(dr["ResponseID"])).Value,
                            HandlyBy = CastAide.AsString(dr["CreatorName"]),
                            CreationTime = DateTimeUtilize.ChangeTimeZoneToClient(dr["CreateDate"].ToString())
                        });
            }
            return dailyComplaintsData;
        }
        #endregion
        #endregion

        #region licensee report
        public DailySummaryBO GetLicenseeDailySummaryReport(string fromDate, string toDate)
        {
            var dailySummary = new DailySummaryBO();
            try
            {
                var dsReport = _repository.GetIBCDailySummaryReport(
                    CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate), Enumerations.ReportTypes.Licensee);

                dailySummary.DailySummaries = new List<DailySummaryGroup>();
                dailySummary.ReportDay = CastAide.AsDateTime(fromDate);
                if (dsReport != null && dsReport.Tables != null && dsReport.Tables.Count > 0)
                {
                    var startCol = 4;
                    var tbData = dsReport.Tables[0];
                    var totalCols = tbData.Columns.Count;

                    var oldMainId = 0;
                    var oldMain = string.Empty;
                    var subTotal = new int[totalCols];
                    var grandTotal = new int[totalCols];

                    #region set data for subtotal & grand total
                    var dailyGroup = new DailySummaryGroup();
                    dailyGroup.DailySummaries = new List<DailySummaryDetail>();
                    foreach (DataRow dr in tbData.Rows)
                    {
                        var currentMainId = CastAide.AsInt32(dr["MainCategoryID"].ToString());
                        var currentMain = CastAide.AsString(dr["MainCategory"]);
                        var currentSummary = new DailySummaryDetail();
                        currentSummary.Cells = new List<DailySummaryType>();
                        currentSummary.MainCategory = CastAide.AsString(dr["SubCategory"]);
                        #region assign data
                        if (currentMainId != oldMainId && oldMainId != 0)
                        {
                            var subTotalCells = new List<DailySummaryType>();
                            var cellsTotal = 0;
                            for (int j = startCol; j < totalCols; j++)
                            {
                                subTotalCells.Add(new DailySummaryType()
                                {
                                    Title = CastAide.AsString(dr.Table.Columns[j].ColumnName),
                                    Value = subTotal[j]
                                });
                                cellsTotal += subTotal[j];
                                subTotal[j] = 0;
                            }
                            var summaryDetail = new DailySummaryDetail()
                            {
                                MainCategory = oldMain,
                                MainCategoryId = oldMainId,
                                Cells = subTotalCells,
                                Total = cellsTotal
                            };
                            dailyGroup.Total = summaryDetail;
                            dailySummary.DailySummaries.Add(dailyGroup);
                            dailyGroup = new DailySummaryGroup();
                            dailyGroup.DailySummaries = new List<DailySummaryDetail>();
                        }
                        #endregion

                        for (int i = startCol; i < totalCols; i++)
                        {
                            subTotal[i] = subTotal[i] + CastAide.AsInt32(dr[i]);
                            grandTotal[i] = grandTotal[i] + CastAide.AsInt32(dr[i]);

                            currentSummary.Cells.Add(new DailySummaryType()
                            {
                                Value = CastAide.AsInt32(dr[i])
                            });
                            currentSummary.Total += CastAide.AsInt32(dr[i]);

                        }
                        dailyGroup.DailySummaries.Add(currentSummary);
                        oldMainId = currentMainId;
                        oldMain = currentMain;
                    }
                    #endregion

                    #region assign data for last subtotal & grand total
                    var lastSubTotalCells = new List<DailySummaryType>();
                    var grandTotalCells = new List<DailySummaryType>();
                    var lastCellsTotal = 0;
                    var cellsGrandTotal = 0;
                    for (int j = startCol; j < totalCols; j++)
                    {
                        var cellName = tbData.Columns[j].ColumnName;
                        lastSubTotalCells.Add(new DailySummaryType()
                        {
                            Title = CastAide.AsString(cellName),
                            Value = subTotal[j]
                        });
                        grandTotalCells.Add(new DailySummaryType()
                        {
                            Title = CastAide.AsString(cellName),
                            Value = grandTotal[j]
                        });
                        lastCellsTotal += subTotal[j];
                        cellsGrandTotal += grandTotal[j];
                    }

                    dailyGroup.Total = new DailySummaryDetail()
                    {
                        MainCategory = oldMain,
                        MainCategoryId = oldMainId,
                        Cells = lastSubTotalCells,
                        Total = lastCellsTotal
                    };
                    dailySummary.DailySummaries.Add(dailyGroup);
                    dailySummary.GrandTotal = new DailySummaryDetail()
                    {
                        MainCategory = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "grandTotal"),
                        MainCategoryId = 0,
                        Cells = grandTotalCells,
                        Total = cellsGrandTotal
                    };
                    #endregion
                }

                SessionExtension.AddSession(HttpContext.Current.Session, _licenseeDailySummary, dailySummary);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format(
                    "(GetLicenseeDailySummaryReport) - Error {0}<br> - Date: {1} -> {2}", ex.Message, fromDate, toDate));
            }
            return dailySummary;
        }
               
        public DailyComplaintsDTO GetLicenseeDailyDetailReport(string fromDate, string toDate, bool fromCache, int itemPerPage, int currentPage)
        {
            var dailyComplaintsData = new DailyComplaintsDTO();
            dailyComplaintsData.DailyComplaints = new List<DailyComplaintsDetail>();
            try
            {
                if (fromCache)
                {
                    var getMappedResult = CacheService.Get<DailyComplaintsDTO>(_licenseeDailyDetail);
                    if (getMappedResult != null && getMappedResult.DailyComplaints != null && getMappedResult.DailyComplaints.Count > 0)
                    {
                        var pagingMappedResult = new DailyComplaintsDTO()
                        {
                            DailyComplaints = getMappedResult.DailyComplaints.Select(c => c).Skip(itemPerPage * (currentPage - 1)).Take(itemPerPage).ToList(),

                        };
                        pagingMappedResult.pageindex = currentPage;
                        pagingMappedResult.total = getMappedResult.DailyComplaints.Count;
                        return pagingMappedResult;
                    }
                    else
                    {
                        var sessionData = (DataTable)SessionExtension.GetSession(HttpContext.Current.Session, _LicenseeDailyDetailNotMapped);
                        if (sessionData != null)
                        {
                            var paging = sessionData.AsEnumerable().Select(c => c).Skip(itemPerPage * (currentPage - 1)).Take(itemPerPage);
                            dailyComplaintsData = MapperLicenseeDailyDetailReport(paging);
                            dailyComplaintsData.pageindex = currentPage;
                            dailyComplaintsData.total = sessionData.Rows.Count;

                            return dailyComplaintsData;
                        }
                    }
                }

                CacheService.Delete(_licenseeDailyDetail);
                var dsReport = _repository.GetIBCDailyDetailReport(CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate), Enumerations.ReportTypes.Licensee);
                if (dsReport != null && dsReport.Tables != null && dsReport.Tables.Count > 0)
                {
                    var paginOnNewData = dsReport.Tables[0].AsEnumerable().Select(c => c).Skip(itemPerPage * (currentPage - 1)).Take(itemPerPage);
                    dailyComplaintsData = MapperLicenseeDailyDetailReport(paginOnNewData);
                    if (dsReport.Tables[0].Rows.Count <= itemPerPage)
                    {
                        CacheService.Set(_licenseeDailyDetail, dailyComplaintsData);
                    }
                    else
                    {
                        SessionExtension.AddSession(HttpContext.Current.Session, _LicenseeDailyDetailNotMapped, dsReport.Tables[0]);
                        
                        Task taskMappingLicenseeDetailData = Task.Factory.StartNew(() =>
                        {
                            CacheService.Delete(_licenseeDailyDetail);
                            var mappingData = MapperLicenseeDailyDetailReport(dsReport.Tables[0].AsEnumerable().Select(c => c));
                            CacheService.Set(_licenseeDailyDetail, mappingData);
                        });                        
                    }
                    dailyComplaintsData.pageindex = currentPage;
                    dailyComplaintsData.total = dsReport.Tables[0].Rows.Count;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format(
                    "(GetLicenseeDailyDetailReport) - Error: {0}<br> - Date: {1} -> {2}", ex.Message, fromDate, toDate));
            }
            return dailyComplaintsData;
        }
        #region mapping licensee daily report
        
        private DailyComplaintsDTO MapperLicenseeDailyDetailReport(IEnumerable<DataRow> dt)
        {

            var dailyComplaintsData = new DailyComplaintsDTO();
            dailyComplaintsData.DailyComplaints = new List<DailyComplaintsDetail>();
            foreach (DataRow dr in dt)
            {
                var complaint = Helper.RemoveSpecialHtmlTag(CastAide.AsString(dr["Description"]));
                var solution = Helper.RemoveSpecialHtmlTag(CastAide.AsString(dr["Solution"]));

                dailyComplaintsData.DailyComplaints.Add(new DailyComplaintsDetail()
                {
                    FeedbackDate = DateTimeUtilize.ChangeTimeZoneToClientDate(dr["FeedbackDate"].ToString()),
                    FeedbackTime = DateTimeUtilize.ChangeTimeZoneToClientTime(dr["FeedbackDate"].ToString()),
                    CustomerName = CastAide.AsString(dr["UserName"]),
                    Licensee = _settingService.GetById(CastAide.AsInt32(dr["SiteID"])).Value,
                    CommunicationTool = new CommunicationTool()
                    {
                        ContactBy = _settingService.GetById(CastAide.AsInt32(dr["CommunicationID"])).Value,
                        Address = string.Format("MSN: {0}\n<br> Skype: {1}\n<br> Email: {2}\n<br> Phone: {3}\n<br> Mobile: {4}", CastAide.AsString(dr["msn"]),
                                        CastAide.AsString(dr["Skype"]), CastAide.AsString(dr["Email"]),
                                        CastAide.AsString(dr["Phone"]), CastAide.AsString(dr["MobilePhone"]))
                    },
                    ExtNo = _settingService.GetById(CastAide.AsInt32(dr["StationID"])).Value,
                    ProductName = _settingService.GetById(CastAide.AsInt32(dr["ProductID"])).Value,
                    Country = _countryService.GetById(CastAide.AsInt32(dr["CountryID"])).CountryName,
                    Category = _settingService.GetById(CastAide.AsInt32(dr["MainCategoryID"])).Value,
                    Complaints = complaint,
                    Solution = solution,
                    Status = _settingService.GetById(CastAide.AsInt32(dr["StatusID"])).Value,
                    Response = _settingService.GetById(CastAide.AsInt32(dr["ResponseID"])).Value,
                    HandlyBy = CastAide.AsString(dr["CreatorName"]),
                    CreationTime = DateTimeUtilize.ChangeTimeZoneToClient(dr["CreateDate"].ToString())
                });
            }
            return dailyComplaintsData;
        }
        #endregion
        #endregion
        
        public CasinoDailySummary GetCasinoDailySummaryReport(string fromDate, string toDate)
        {
            var result = new CasinoDailySummary();
            try
            {
                result.DailySummary = new List<CasinoDailySummaryDetail>();
                result.SumAllFeedbacks = new List<int>();
                result.ReportDate = CastAide.AsDateTime(fromDate);
                var getData = _repository.GetCasinoDailySummaryReport(CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate), 1);

                if (getData != null && getData.Tables != null && getData.Tables.Count > 0)
                {
                    var cPrefix = CasinoSection.Instance[CasinoSection.SystemConfigCollection.Systems];
                    var cSubfix = CasinoSection.Instance[CasinoSection.SystemConfigCollection.Communications];
                    var tbData = getData.Tables[0];
                    var oldMainId = 0;
                    var oldMain = string.Empty;
                    var systemSubTotal = new List<SystemDailySummary>();
                    var systemGrandTotal = new List<SystemDailySummary>();
                    #region add systems group

                    for (var i = 0; i < cPrefix.Count; i++)
                    {
                        result.SumAllFeedbacks.Add(new int());
                        var cells = new List<DailySummaryType>();
                        for (var j = 0; j < cSubfix.Count; j++)
                        {
                            cells.Add(new DailySummaryType() { Value = 0 });
                        }

                        systemSubTotal.Add(new SystemDailySummary()
                        {
                            Sytem = cPrefix[i].Name,
                            SystemId = cPrefix[i].Id,
                            Cells = cells
                        });
                        var cells2 = new List<DailySummaryType>();
                        for (var j = 0; j < cSubfix.Count; j++)
                        {
                            cells2.Add(new DailySummaryType() { Value = 0 });
                        }
                        systemGrandTotal.Add(new SystemDailySummary()
                        {
                            Sytem = cPrefix[i].Name,
                            SystemId = cPrefix[i].Id,
                            Cells = cells2
                        });
                    }

                    #endregion

                    #region assign data for each item
                    foreach (DataRow dr in tbData.Rows)
                    {
                        var currentMainId = CastAide.AsInt32(dr["MainCategoryID"].ToString());
                        var currentMain = CastAide.AsString(dr["MainCategory"]);
                        #region assign data

                        if (currentMainId != oldMainId && oldMainId != 0)
                        {
                            var summary = new CasinoDailySummaryDetail()
                            {
                                MainCategory = oldMain,
                                MainCategoryId = oldMainId,
                                SystemSummary = systemSubTotal
                            };

                            result.DailySummary.Add(summary);

                            systemSubTotal = new List<SystemDailySummary>();
                            for (var i = 0; i < cPrefix.Count; i++)
                            {
                                var cells = new List<DailySummaryType>();
                                for (var j = 0; j < cSubfix.Count; j++)
                                    cells.Add(new DailySummaryType() { Value = 0 });

                                systemSubTotal.Add(new SystemDailySummary()
                                {
                                    Sytem = cPrefix[i].Name,
                                    SystemId = cPrefix[i].Id,
                                    Cells = cells
                                });
                            }
                        }
                        #endregion

                        for (var j = 0; j < cPrefix.Count; j++)
                        {
                            for (var k = 0; k < cSubfix.Count; k++)
                            {
                                var currentVal =
                                    CastAide.AsInt32(dr[string.Format("{0}{1}", cPrefix[j].Prefix, cSubfix[k].Subfix)]);

                                systemSubTotal[j].Cells[k].Title = cSubfix[k].Subfix;
                                systemSubTotal[j].Cells[k].Value = systemSubTotal[j].Cells[k].Value + currentVal;

                                systemGrandTotal[j].Cells[k].Title = cSubfix[k].Subfix;
                                systemGrandTotal[j].Cells[k].Value = systemGrandTotal[j].Cells[k].Value + currentVal;

                                result.SumAllFeedbacks[j] += currentVal;
                            }
                        }

                        oldMainId = currentMainId;
                        oldMain = currentMain;
                    }
                    #endregion

                    #region last subtotal & grand total
                    result.DailySummary.Add(new CasinoDailySummaryDetail()
                    {
                        MainCategory = oldMain,
                        MainCategoryId = oldMainId,
                        SystemSummary = systemSubTotal
                    });


                    result.GrandTotal = new CasinoDailySummaryDetail()
                    {
                        MainCategory = "GrandTotal",
                        MainCategoryId = 0,
                        SystemSummary = systemGrandTotal
                    };

                    #endregion
                }

                SessionExtension.AddSession(HttpContext.Current.Session, _CasinoDailySummary, result);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format(
                    "(GetCasinoDailySummaryReport) - Error: {0}<br> - Date: {1} -> {2}", ex.Message, fromDate, toDate));
            }
            return result;
        }

        #region casino daily detail
        public DailyComplaintsDTO GetCasinoDailyDetailReport(string fromDate, string toDate, bool fromCache, int itemPerPage, int currentPage)
        {
            var dailyComplaintsData = new DailyComplaintsDTO();
            dailyComplaintsData.DailyComplaints = new List<DailyComplaintsDetail>();
            try
            {
                if (fromCache)
                {
                    var getMappedResult = CacheService.Get<DailyComplaintsDTO>(_CasinoDailyDetail);
                    if (getMappedResult != null && getMappedResult.DailyComplaints != null && getMappedResult.DailyComplaints.Count > 0)
                    {
                        var pagingMappedResult = new DailyComplaintsDTO()
                        {
                            DailyComplaints = getMappedResult.DailyComplaints.Select(c => c).Skip(itemPerPage * (currentPage - 1)).Take(itemPerPage).ToList(),

                        };
                        pagingMappedResult.pageindex = currentPage;
                        pagingMappedResult.total = getMappedResult.DailyComplaints.Count;
                        return pagingMappedResult;
                    }
                    else
                    {
                        var sessionData = (DataTable)SessionExtension.GetSession(HttpContext.Current.Session, _CasinoDailyDetailNotMapped);
                        if (sessionData != null)
                        {
                            var paging = sessionData.AsEnumerable().Select(c => c).Skip(itemPerPage * (currentPage - 1)).Take(itemPerPage);
                            dailyComplaintsData = MapperIBCDailyDetailReport(paging);
                            dailyComplaintsData.pageindex = currentPage;
                            dailyComplaintsData.total = sessionData.Rows.Count;

                            return dailyComplaintsData;
                        }
                    }
                }

                CacheService.Delete(_CasinoDailyDetail);
                var dsReport = _repository.GetCasinoDailyDetailReport(CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate));
                if (dsReport != null && dsReport.Tables != null && dsReport.Tables.Count > 0)
                {
                    var paginOnNewData = dsReport.Tables[0].AsEnumerable().Select(c => c).Skip(itemPerPage * (currentPage - 1)).Take(itemPerPage);
                    dailyComplaintsData = MapperCasinoDailyDetailReport(paginOnNewData);
                    if (dsReport.Tables[0].Rows.Count <= itemPerPage)
                    {
                        CacheService.Set(_CasinoDailyDetail, dailyComplaintsData);
                    }
                    else
                    {
                        SessionExtension.AddSession(HttpContext.Current.Session, _CasinoDailyDetailNotMapped, dsReport.Tables[0]);
                        
                        Task taskMappingCasinoDetailData = Task.Factory.StartNew(() =>
                        {
                            CacheService.Delete(_CasinoDailyDetail);
                            var mappingData = MapperCasinoDailyDetailReport(dsReport.Tables[0].AsEnumerable().Select(c => c));
                            CacheService.Set(_CasinoDailyDetail, mappingData);
                        });        
                    }
                    dailyComplaintsData.pageindex = currentPage;
                    dailyComplaintsData.total = dsReport.Tables[0].Rows.Count;
                }
            }
            catch (Exception ex)
            {                
                Helper.SendMailErrorNotice(string.Format(
                    "(GetCasinoDailyDetailReport) - Error: {0}<br> - Date: {1} -> {2}", ex.Message, fromDate, toDate));
            }
            return dailyComplaintsData;
        }
        #region mapping casino daily report       

        private DailyComplaintsDTO MapperCasinoDailyDetailReport(IEnumerable<DataRow> dt)
        {
            var dailyComplaintsData = new DailyComplaintsDTO();
            dailyComplaintsData.DailyComplaints = new List<DailyComplaintsDetail>();
            foreach (DataRow dr in dt)
            {
                var complaint = Helper.RemoveSpecialHtmlTag(CastAide.AsString(dr["Description"]));
                var solution = Helper.RemoveSpecialHtmlTag(CastAide.AsString(dr["Solution"]));

                dailyComplaintsData.DailyComplaints.Add(
                    new DailyComplaintsDetail()
                        {
                            FeedbackDate = DateTimeUtilize.ChangeTimeZoneToClientDate(dr["FeedbackDate"].ToString()),
                            FeedbackTime = DateTimeUtilize.ChangeTimeZoneToClientTime(dr["FeedbackDate"].ToString()),
                            CustomerName = CastAide.AsString(dr["UserName"]),
                            Licensee = _settingService.GetById(CastAide.AsInt32(dr["SiteID"])).Value,
                            CommunicationTool =
                                new CommunicationTool()
                                    {
                                        ContactBy = _settingService.GetById(CastAide.AsInt32(dr["CommunicationID"])).Value,
                                        Address =
                                            string.Format(
                                                "MSN: {0}\n<br> Skype: {1}\n<br> Email: {2}\n<br> Phone: {3}\n<br> Mobile: {4}",
                                                CastAide.AsString(dr["msn"]),
                                                CastAide.AsString(dr["Skype"]),
                                                CastAide.AsString(dr["Email"]),
                                                CastAide.AsString(dr["Phone"]),
                                                CastAide.AsString(dr["MobilePhone"]))
                                    },
                            ExtNo = _settingService.GetById(CastAide.AsInt32(dr["StationID"])).Value,
                            ProductName = _settingService.GetById(CastAide.AsInt32(dr["ProductID"])).Value,
                            Country = _countryService.GetById(CastAide.AsInt32(dr["CountryID"])).CountryName,
                            Category = _settingService.GetById(CastAide.AsInt32(dr["MainCategoryID"])).Value,
                            Complaints = complaint,
                            Solution = solution,
                            Status = _settingService.GetById(CastAide.AsInt32(dr["StatusID"])).Value,
                            Response = _settingService.GetById(CastAide.AsInt32(dr["ResponseID"])).Value,
                            HandlyBy = CastAide.AsString(dr["CreatorName"]),
                            CreationTime = DateTimeUtilize.ChangeTimeZoneToClient(dr["CreateDate"].ToString())
                        });
            }
            return dailyComplaintsData;
        }
        #endregion
        #endregion
        
        public DailySummaryBO GetCasinoWeeklySummaryReport(string fromDate, string toDate)
        {
            var dailySummary = new DailySummaryBO();
            try
            {
                var dsReport = _repository.GetCasinoDailySummaryReport(
                    CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate), 3);

                dailySummary.DailySummaries = new List<DailySummaryGroup>();
                dailySummary.ReportDay = CastAide.AsDateTime(fromDate);
                if (dsReport != null && dsReport.Tables != null && dsReport.Tables.Count > 0)
                {
                    var startCol = 2;
                    var tbData = dsReport.Tables[0];
                    var totalCols = tbData.Columns.Count;

                    var oldMainId = 0;
                    var oldMain = string.Empty;
                    var subTotal = new int[totalCols];
                    var grandTotal = new int[totalCols];

                    #region set data for subtotal & grand total
                    var dailyGroup = new DailySummaryGroup();
                    dailyGroup.DailySummaries = new List<DailySummaryDetail>();
                    foreach (DataRow dr in tbData.Rows)
                    {
                        var currentMainId = CastAide.AsInt32(dr["MainCategoryID"].ToString());
                        var currentMain = CastAide.AsString(dr["MainCategory"]);
                        var currentSummary = new DailySummaryDetail();
                        currentSummary.Cells = new List<DailySummaryType>();
                        #region assign data
                        if (currentMainId != oldMainId && oldMainId != 0)
                        {
                            var subTotalCells = new List<DailySummaryType>();
                            var cellsTotal = 0;
                            for (int j = startCol; j < totalCols; j++)
                            {
                                subTotalCells.Add(new DailySummaryType()
                                {
                                    Title = CastAide.AsString(dr.Table.Columns[j].ColumnName),
                                    Value = subTotal[j]
                                });
                                cellsTotal += subTotal[j];
                                subTotal[j] = 0;
                            }
                            var summaryDetail = new DailySummaryDetail()
                            {
                                MainCategory = oldMain,
                                MainCategoryId = oldMainId,
                                Cells = subTotalCells,
                                Total = cellsTotal
                            };
                            dailyGroup.Total = summaryDetail;
                            dailySummary.DailySummaries.Add(dailyGroup);
                            dailyGroup = new DailySummaryGroup();
                            dailyGroup.DailySummaries = new List<DailySummaryDetail>();
                        }
                        #endregion

                        for (int i = startCol; i < totalCols; i++)
                        {
                            subTotal[i] = subTotal[i] + CastAide.AsInt32(dr[i]);
                            grandTotal[i] = grandTotal[i] + CastAide.AsInt32(dr[i]);


                            currentSummary.Cells.Add(new DailySummaryType()
                            {
                                Value = CastAide.AsInt32(dr[i])
                            });
                        }
                        dailyGroup.DailySummaries.Add(currentSummary);
                        oldMainId = currentMainId;
                        oldMain = currentMain;
                    }
                    #endregion

                    #region assign data for last subtotal & grand total
                    var lastSubTotalCells = new List<DailySummaryType>();
                    var grandTotalCells = new List<DailySummaryType>();
                    var lastCellsTotal = 0;
                    var cellsGrandTotal = 0;
                    for (int j = startCol; j < totalCols; j++)
                    {
                        var cellName = tbData.Columns[j].ColumnName;
                        lastSubTotalCells.Add(new DailySummaryType()
                        {
                            Title = CastAide.AsString(cellName),
                            Value = subTotal[j]
                        });
                        grandTotalCells.Add(new DailySummaryType()
                        {
                            Title = CastAide.AsString(cellName),
                            Value = grandTotal[j]
                        });
                        lastCellsTotal += subTotal[j];
                        cellsGrandTotal += grandTotal[j];
                    }
                    dailyGroup.Total = new DailySummaryDetail()
                    {
                        MainCategory = oldMain,
                        MainCategoryId = oldMainId,
                        Cells = lastSubTotalCells,
                        Total = lastCellsTotal
                    };
                    dailySummary.DailySummaries.Add(dailyGroup);
                    dailySummary.GrandTotal = new DailySummaryDetail()
                    {
                        MainCategory = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "grandTotal"),
                        MainCategoryId = 0,
                        Cells = grandTotalCells,
                        Total = cellsGrandTotal
                    };
                    #endregion
                }

                SessionExtension.AddSession(HttpContext.Current.Session, _CasinoWeeklySummary, dailySummary);
            }
            catch (Exception ex) 
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return dailySummary;
        }

        private static string GetResolvingTime(string resolutionDate, string feedbackDate)
        {
            var resolDate = CastAide.AsDateTime(resolutionDate);
            var fbDate = CastAide.AsDateTime(feedbackDate);
            var totalDays = (resolDate - fbDate);
            var resolvingTime = string.Format("{0}:{1}:{2}:{3}", totalDays.Days, totalDays.Hours,
                                              totalDays.Minutes, totalDays.Seconds);
            return resolvingTime;
        }

        public DataTable GetAutoSuggestionCustomer(string userName)
        {
            var result = new DataTable();
            var getData = _repository.AutoSuggestionCustomer(userName);
            if (getData != null && getData.Tables.Count > 0)
                result = getData.Tables[0];
            return result;
        }
    }
}