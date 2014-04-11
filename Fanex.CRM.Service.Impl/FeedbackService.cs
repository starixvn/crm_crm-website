// --------------------------------------------------------------------------
// <copyright file="FeedbackService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Service.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
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
    using Fanex.CRM.Infrastructure.Service.Implements;
    using Fanex.CRM.Service.Interface;
    using Fanex.NAP.Core.Business;

    /// <summary>
    /// Feedback service.
    /// </summary>
    public partial class FeedbackService : IFeedbackService
    {
        #region private variables
        private readonly IFeedbackDataAccess _repository;
        private readonly ISettingService _settingService;
        private readonly ICountryService _countryService;
        private readonly IPermissionService _permissionService;
        private string _feedbackStatisticDetailSessionKey = "crm.feedbackstatisticdetailsessionkey";
        private string _feedbackStatisticDetailMappedSessionKey = "crm.FeedbackStatisticDetailMappedSessionKey";
        private string _lastedFeedback = "crm.lastedfeedback";
        private string _lastedFeedbackByCustId = "crm.lastedfeedbackbycustid";
        private string _deletedFeedback = "crm.deletedfeedbacksessionkey";
        private string _lastedTopFeedbackByCustId = "crm.lastedtopfeedbackbycustid";
        private string _feedbackStatisticSessionKey = "crm.feedbackstatisticsessionkey";
        private string _lastedTopFeedback = "crm.lastedtopfeedback";
        private string _ibcDailySummary = "crm.ibcdailysummary";
        private string _ibcDailyDetail = "crm.ibcdailydetail";
        private string _licenseeDailyDetail = "crm.licenseedailydetail";
        private string _licenseeDailySummary = "crm.licenseedailysummary";
        private string _casinoDailySummary = "crm.casinodailysummarystatistics";
        private string _casinoDailyDetail = "crm.casinoweeklydetail";
        private string _casinoDailyDetailNotMapped = "crm.casinoweeklydetailnotmapped";
        private string _casinoWeeklySummary = "crm.casinoweeklysummary";

        private string _ibcDailyDetailNotMapped = "crm.ibcdailydetailnotmapped";
        private string _licenseeDailyDetailNotMapped = "crm.ibcdailydetailnotmapped";

        #endregion

        public FeedbackService()
            : this(null, null, null, null)
        {
        }

        public FeedbackService(IFeedbackDataAccess repository, ISettingService settingService, ICountryService countryService, IPermissionService permissionService)
        {
            _repository = repository ?? new FeedbackDataAccess();
            _settingService = settingService ?? new SettingService();
            _countryService = countryService ?? new CountryService();
            _permissionService = permissionService ?? new PermissionService();
        }

        #region get feedback by id
        /// <summary>
        /// Getting feedback by id.
        /// </summary>
        /// <param name="id">Feedback id.</param>
        /// <returns>Return result.</returns>
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
            }

            return feedback;
        }

        #endregion

        #region update feedback

        /// <summary>
        /// Updating feedback.
        /// </summary>
        /// <param name="feedback">Feedback info.</param>
        /// <returns>Return result.</returns>
        public Result EditFeedback(Feedback feedback)
        {
            Result result = new Result();
            try
            {
                var entity = feedback;
                var ip = Helper.GetClientIP();
                Check.Argument.IsValidIP(ip, "Ip");

                if (!_permissionService.HasPermission(FunctionPermission.EditFeedback, NAP.Core.Business.Constant.Policy.Host))
                {
                    return ReturnErrorResult((int)Enumerations.ErrorCode.UpdateFeedback, "lblNoUpdateFeedbackPermission");
                }

                if (!_permissionService.HasPermission(FunctionPermission.ReviewFeedback, NAP.Core.Business.Constant.Policy.Host) &&
                    (feedback.UpdateReviewMode == (int)Enumerations.Functions.RevewMode.Update || feedback.UpdateReviewMode == (int)Enumerations.Functions.RevewMode.Remove))
                {
                    return ReturnErrorResult((int)Enumerations.ErrorCode.UpdateCustomerInfo, "lblNoUpdateReviewFeedbackPermission");
                }

                if (feedback.Status.Id.Equals((int)Enumerations.Functions.CRMStatus.Deleted) &&
                    !_permissionService.HasPermission(FunctionPermission.DeleteFeedback, NAP.Core.Business.Constant.Policy.Host))
                {
                    return ReturnErrorResult((int)Enumerations.ErrorCode.DeleteFeedback, "lblNoDeletedFeedbackPermission");
                }

                if (feedback.IsOverrideCrmInfo &&
                !_permissionService.HasPermission(FunctionPermission.CustomerInfo, NAP.Core.Business.Constant.Policy.Host))
                {
                    return ReturnErrorResult((int)Enumerations.ErrorCode.DeleteFeedback, "lblNoUpdateCustomerPermission");
                }

                _repository.EditFeedback(entity, feedback.Creator.Id, ip, ref result);
            }
            catch (Exception ex)
            {
                result = new Result(-100, ex.Message);
            }

            return result;
        }

        #endregion

        #region select feedback by custid

        /// <summary>
        /// Select feedback by customer id.
        /// </summary>
        /// <param name="custId">Customer id.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="fromCache">From cache.</param>
        /// <returns>Return result.</returns>
        public DataGridViewDTO SelFeedbackByCustId(int custId, string fromDate, string toDate, int pageIndex, int pageSize, bool fromCache)
        {
            IEnumerable<FeedbackView> lstFeedback = null;
            DataGridViewDTO feedbackdto = new DataGridViewDTO();
            try
            {
                if (fromCache)
                {
                    lstFeedback = (List<FeedbackView>)SessionExtension.GetSession(HttpContext.Current.Session, _lastedFeedbackByCustId) ??
                            SelAllFeedbackByCustId(custId, fromDate, toDate);
                }
                else
                {
                    lstFeedback = SelAllFeedbackByCustId(custId, fromDate, toDate);
                }

                var lstFeedbackPaging = new List<FeedbackView>();
                if (lstFeedback != null && lstFeedback.Count() > 0)
                {
                    lstFeedbackPaging = lstFeedback.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                }

                feedbackdto.Rows = lstFeedbackPaging;
                feedbackdto.TotalItems = lstFeedback != null ? lstFeedback.Count() : 0;
                feedbackdto.PageIndex = pageIndex;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return feedbackdto;
        }

        #endregion

        #region get latest feedback

        /// <summary>
        /// Getting feedback.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="runningNumber">The running number.</param>
        /// <param name="userName">User name.</param>
        /// <param name="isGuest">Is guest customer.</param>
        /// <param name="communicationID">Communication ID.</param>
        /// <param name="mainCategoryID">MainCategory ID.</param>
        /// <param name="subCategoryID">SubCategory ID.</param>
        /// <param name="statusID">Status ID.</param>
        /// <param name="isReviewed">Is Reviewed.</param>
        /// <param name="responseId">Response Id.</param>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="fromCache">From cache.</param>
        /// <returns>
        /// Return result.
        /// </returns>
        public DataGridViewDTO GetFeedback(
            string fromDate,
            string toDate,
            int runningNumber,
            string userName,
            int isGuest,
            int communicationID,
            int mainCategoryID,
            int subCategoryID,
            int statusID,
            int isReviewed,
            int responseId,
            int pageIndex,
            int pageSize,
            bool fromCache)
        {
            var feedbackdto = new DataGridViewDTO();
            try
            {
                List<FeedbackView> listFeedback = null;
                if (fromCache)
                {
                    listFeedback = (List<FeedbackView>)SessionExtension.GetSession(HttpContext.Current.Session, _lastedFeedback) ??
                        SelAllLastedFeedback(fromDate, toDate, runningNumber, userName, isGuest, communicationID, mainCategoryID, subCategoryID, statusID, isReviewed, responseId);
                }
                else
                {
                    listFeedback = SelAllLastedFeedback(
                        fromDate,
                        toDate,
                        runningNumber,
                        userName,
                        isGuest,
                        communicationID,
                        mainCategoryID,
                        subCategoryID,
                        statusID,
                        isReviewed,
                        responseId);
                }

                var lstFeedbackPaging = new List<FeedbackView>();
                if (listFeedback != null && listFeedback.Count > 0)
                {
                    lstFeedbackPaging = listFeedback.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                }

                feedbackdto.Rows = lstFeedbackPaging;
                feedbackdto.TotalItems = listFeedback != null ? listFeedback.Count : 0;
                feedbackdto.PageIndex = pageIndex;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return feedbackdto;
        }

        #endregion

        #region get feedbackstatistic

        /// <summary>
        /// Getting feedback statistic.
        /// </summary>
        /// <param name="param">Input parameters.</param>
        /// <returns>Return result.</returns>
        public FeedbackStatisticsDTO GetFeedbackStatistic(Params param)
        {
            var result = new FeedbackStatisticsDTO();
            var pagingResult = new FeedbackStatisticsDTO();
            try
            {
                if (param.FromCache)
                {
                    result = SessionExtension.GetSession(HttpContext.Current.Session, _feedbackStatisticSessionKey) != null
                                 ? (FeedbackStatisticsDTO)
                                   SessionExtension.GetSession(HttpContext.Current.Session, _feedbackStatisticSessionKey)
                                 : SelAllFeedbackStatistic(param);
                }
                else
                {
                    result = SelAllFeedbackStatistic(param);
                }

                if (result != null && result.Rows != null && result.Rows.Count() > 0)
                {
                    pagingResult.Rows = result.Rows.Select(c => c).Skip(param.ItemPerPage * (param.CurrentPage - 1)).Take(param.ItemPerPage).ToList();
                    pagingResult.GrandTotal = result.GrandTotal;
                    pagingResult.PageIndex = result.PageIndex;
                    pagingResult.Total = result.Total;
                    pagingResult.ColumnIDs = result.ColumnIDs;
                    pagingResult.ColumnNames = result.ColumnNames;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return pagingResult;
        }

        #endregion

        #region get feedback statistic detail

        /// <summary>
        /// Getting Feedback Statistic Detail.
        /// </summary>
        /// <param name="param">Input parameters.</param>
        /// <returns>Return result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public DataGridViewDTO GetFeedbackStatisticDetail(Params param)
        {
            var result = new List<Feedback>();
            var pagingResult = new DataGridViewDTO();
            try
            {
                if (param.FromCache)
                {
                    var getMappedResult = CacheService.Get<DataGridViewDTO>(_feedbackStatisticDetailMappedSessionKey);
                    if (getMappedResult != null && getMappedResult.Rows != null && getMappedResult.Rows.Count > 0)
                    {
                        var pagingMappedResult = new DataGridViewDTO()
                        {
                            Rows = getMappedResult.Rows.Select(c => c).Skip(param.ItemPerPage * (param.CurrentPage - 1)).Take(param.ItemPerPage).ToList(),
                            PageIndex = param.CurrentPage,
                            TotalItems = getMappedResult.TotalItems
                        };
                        return pagingMappedResult;
                    }
                    else
                    {
                        result = SessionExtension.GetSession(HttpContext.Current.Session, _feedbackStatisticDetailSessionKey) != null ?
                                 (List<Feedback>)SessionExtension.GetSession(HttpContext.Current.Session, _feedbackStatisticDetailSessionKey) :
                                 SelAllFeedbackStatisticDetail(param);
                    }
                }
                else
                {
                    result = SelAllFeedbackStatisticDetail(param);
                }

                var lstFeedbackPaging = new List<Feedback>();
                if (result != null && result.Count > 0)
                {
                    lstFeedbackPaging = result.Select(c => c).Skip(param.ItemPerPage * (param.CurrentPage - 1)).Take(param.ItemPerPage).ToList();
                    var feedbackViewPaging = new List<FeedbackView>();
                    foreach (var feedback in lstFeedbackPaging)
                    {
                        feedbackViewPaging.Add(CreateFeedbackView(feedback));
                    }

                    pagingResult.Rows = feedbackViewPaging;
                    pagingResult.PageIndex = param.CurrentPage;
                    pagingResult.TotalItems = result.Count;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return pagingResult;
        }
        #endregion

        #region Get Feedback Subject Items
        /// <summary>
        /// Get Feedback Subject TemplateItems.
        /// </summary>
        /// <param name="templateId">Template Id.</param>
        /// <returns>Return result.</returns>
        public List<FeedbackTemplate> GetFeedbackSubjectTemplateItems(int templateId = 0)
        {
            List<FeedbackTemplate> listFeedbackTemplate = new List<FeedbackTemplate>();
            try
            {
                var datasetResult = _repository.GetFeedbackSubjectTemplateItems(templateId);
                if (datasetResult != null && datasetResult.Tables.Count > 0)
                {
                    foreach (DataRow dr in datasetResult.Tables[0].Rows)
                    {
                        listFeedbackTemplate.Add(new FeedbackTemplate
                        {
                            TemplateId = CastAide.AsInt32(dr["templateID"]),
                            Subject = CastAide.AsString(dr["subject"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return listFeedbackTemplate;
        }

        /// <summary>
        /// Get Number Of Feedback Subject Template Item.
        /// </summary>
        /// <returns>Return result.</returns>
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

        /// <summary>
        /// Get Feedback Subject Template Items Range.
        /// </summary>
        /// <param name="range">Item range.</param>
        /// <returns>Return result.</returns>
        public List<FeedbackTemplate> GetFeedbackSubjectTemplateItemsRange(int range)
        {
            int multipleNum = 100000;
            var result = new List<FeedbackTemplate>();
            try
            {
                List<FeedbackTemplate> listFeedbackTemplate = CacheService.Get<List<FeedbackTemplate>>("AllFeedbackTemplate");
                if (listFeedbackTemplate != null && listFeedbackTemplate.Count > 0)
                {
                    result = listFeedbackTemplate.Skip((range - 1) * multipleNum).Take(range * multipleNum).ToList();
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        #endregion

        #region Handle Feedback Subject Items

        /// <summary>
        /// Edit Subject Feedback Template.
        /// </summary>
        /// <param name="id">Feedback Template Id.</param>
        /// <param name="newSubject">New Subject.</param>
        /// <returns>Return result.</returns>
        public Result EditSubjectFeedbackTemplate(int id, string newSubject)
        {
            Result result = new Result(0, string.Empty);
            try
            {
                if (!_permissionService.HasPermission(FunctionPermission.FBSubjectTemplateManagement, Constant.Policy.Host))
                {
                    return ReturnErrorResult(500, "lblNoUpdateFBSubjectTemplate");
                }

                result = _repository.EditSubjectFeedbackTemplate(id, newSubject);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Select Feedback Template Page With Search.
        /// </summary>
        /// <param name="searchString">Search String.</param>
        /// <param name="pageIndex">Page Index.</param>
        /// <param name="pageSize">Page Size.</param>
        /// <returns>Return result.</returns>
        public FeedbackTemplateDGViewDTO SelFeedbackTemplatePageWithSearch(string searchString, int pageIndex, int pageSize)
        {
            FeedbackTemplateDGViewDTO tempDGView = new FeedbackTemplateDGViewDTO();
            try
            {
                List<FeedbackTemplate> lstFeedbackTemplate = GetFeedbackSubjectTemplateItems(0).Where(c => c.Subject.ToLower().Contains(searchString.ToLower())).ToList();
                var lstFeedbackPaging = new List<FeedbackTemplate>();
                if (lstFeedbackTemplate != null && lstFeedbackTemplate.Count > 0)
                {
                    lstFeedbackPaging = lstFeedbackTemplate.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                }

                tempDGView.Rows = lstFeedbackPaging;
                tempDGView.TotalItems = lstFeedbackTemplate != null ? lstFeedbackTemplate.Count() : 0;
                tempDGView.PageIndex = pageIndex;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return tempDGView;
        }
        #endregion

        #region Select top feedback

        /// <summary>
        /// Select Top Feedback By Customer Id.
        /// </summary>
        /// <param name="custId">Customer id.</param>
        /// <param name="selectTop">Total feedbacks.</param>
        /// <param name="pageIndex">Page Index.</param>
        /// <param name="pageSize">Page Size.</param>
        /// <param name="fromCache">From Cache.</param>
        /// <returns>Return result.</returns>
        public DataGridViewDTO SelTopFeedbackByCustId(int custId, int selectTop, int pageIndex, int pageSize, bool fromCache)
        {
            List<FeedbackView> lstFeedback = null;
            var feedbackdto = new DataGridViewDTO();
            try
            {
                if (fromCache)
                {
                    lstFeedback = (List<FeedbackView>)SessionExtension.GetSession(HttpContext.Current.Session, _lastedTopFeedbackByCustId) ??
                            SelAllTopFeedbackByCustId(custId, selectTop);
                }
                else
                {
                    lstFeedback = SelAllTopFeedbackByCustId(custId, selectTop);
                }

                var lstFeedbackPaging = new List<FeedbackView>();
                if (lstFeedback != null && lstFeedback.Count() > 0)
                {
                    lstFeedbackPaging = lstFeedback.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                }

                feedbackdto.Rows = lstFeedbackPaging;
                feedbackdto.TotalItems = lstFeedback != null ? lstFeedback.Count() : 0;
                feedbackdto.PageIndex = pageIndex;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return feedbackdto;
        }

        /// <summary>
        /// Select Top Feedback.
        /// </summary>
        /// <param name="selectTop">Total feedbacks.</param>
        /// <param name="pageIndex">Page Index.</param>
        /// <param name="pageSize">Page Size.</param>
        /// <param name="fromCache">From Cache.</param>
        /// <returns>Return result.</returns>
        public DataGridViewDTO SelTopFeedback(int selectTop, int pageIndex, int pageSize, bool fromCache)
        {
            List<FeedbackView> lstFeedback = null;
            var feedbackdto = new DataGridViewDTO();
            try
            {
                if (fromCache)
                {
                    lstFeedback = (List<FeedbackView>)SessionExtension.GetSession(HttpContext.Current.Session, _lastedTopFeedback) ??
                            SelAllTopFeedback(selectTop);
                }
                else
                {
                    lstFeedback = SelAllTopFeedback(selectTop);
                }

                var lstFeedbackPaging = new List<FeedbackView>();
                if (lstFeedback != null && lstFeedback.Count() > 0)
                {
                    lstFeedbackPaging = lstFeedback.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                }

                feedbackdto.Rows = lstFeedbackPaging;
                feedbackdto.TotalItems = lstFeedback != null ? lstFeedback.Count() : 0;
                feedbackdto.PageIndex = pageIndex;
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
        /// <summary>
        /// Get IBC Daily Summary Report.
        /// </summary>
        /// <param name="fromDate">From Date.</param>
        /// <param name="toDate">To Date value.</param>
        /// <returns>Return result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public DailySummaryBO GetIBCDailySummaryReport(string fromDate, string toDate)
        {
            var dailySummary = new DailySummaryBO();
            try
            {
                var datasetReport = _repository.GetIBCDailySummaryReport(
                    CastAide.AsDateTime(fromDate),
                    CastAide.AsDateTime(toDate),
                    Enumerations.ReportTypes.CreditSite);

                dailySummary.DailySummaries = new List<DailySummaryGroup>();
                dailySummary.ReportDay = CastAide.AsDateTime(fromDate);
                if (datasetReport != null && datasetReport.Tables != null && datasetReport.Tables.Count > 0)
                {
                    var startCol = 4;
                    var tableData = datasetReport.Tables[0];
                    var totalCols = tableData.Columns.Count;

                    var oldMainId = 0;
                    var oldMain = string.Empty;
                    var subTotal = new int[totalCols];
                    var grandTotal = new int[totalCols];

                    var dailyGroup = new DailySummaryGroup();
                    dailyGroup.DailySummaries = new List<DailySummaryDetail>();
                    foreach (DataRow dr in tableData.Rows)
                    {
                        var currentMainId = CastAide.AsInt32(dr["MainCategoryID"].ToString());
                        var currentMain = CastAide.AsString(dr["MainCategory"]);
                        var currentSummary = new DailySummaryDetail();
                        currentSummary.Cells = new List<DailySummaryType>();
                        currentSummary.MainCategory = CastAide.AsString(dr["SubCategory"]);

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

                    var lastSubTotalCells = new List<DailySummaryType>();
                    var grandTotalCells = new List<DailySummaryType>();
                    var lastCellsTotal = 0;
                    var cellsGrandTotal = 0;
                    for (int j = startCol; j < totalCols; j++)
                    {
                        var cellName = tableData.Columns[j].ColumnName;
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
                }

                SessionExtension.AddSession(HttpContext.Current.Session, _ibcDailySummary, dailySummary);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                var errorMessageFormat = "(GetIBCDailySummaryReport) - Error {0}<br> - Date: {1} -> {2}";
                Helper.SendMailErrorNotice(string.Format(errorMessageFormat, ex.Message, fromDate, toDate));
            }

            return dailySummary;
        }

        /// <summary>
        /// Get IBC Daily Detail Report.
        /// </summary>
        /// <param name="fromDate">From Date.</param>
        /// <param name="toDate">To Date value.</param>
        /// <param name="fromCache">From Cache.</param>
        /// <param name="itemPerPage">Item Per Page.</param>
        /// <param name="currentPage">Current Page.</param>
        /// <returns>Return result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public DailyComplaintsDTO GetIBCDailyDetailReport(string fromDate, string toDate, bool fromCache, int itemPerPage, int currentPage)
        {
            var dailyComplaintsData = new DailyComplaintsDTO();
            dailyComplaintsData.DailyComplaints = new List<DailyComplaintsDetail>();
            try
            {
                if (fromCache)
                {
                    var getMappedResult = (DailyComplaintsDTO)SessionExtension.GetSession(HttpContext.Current.Session, _ibcDailyDetail);
                    if (getMappedResult != null && getMappedResult.DailyComplaints != null && getMappedResult.DailyComplaints.Count > 0)
                    {
                        var pagingMappedResult = new DailyComplaintsDTO()
                        {
                            DailyComplaints = getMappedResult.DailyComplaints.Select(c => c).Skip(itemPerPage * (currentPage - 1)).Take(itemPerPage).ToList(),
                            Pageindex = currentPage,
                            TotalItems = getMappedResult.DailyComplaints.Count
                        };

                        return pagingMappedResult;
                    }
                }

                SessionExtension.AddSession(HttpContext.Current.Session, _ibcDailyDetail, null);
                CacheService.Delete(_ibcDailyDetailNotMapped);
                var datasetReport = _repository.GetIBCDailyDetailReport(CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate), Enumerations.ReportTypes.CreditSite);
                if (datasetReport != null && datasetReport.Tables != null && datasetReport.Tables.Count > 0)
                {
                    SessionExtension.AddSession(HttpContext.Current.Session, _ibcDailyDetailNotMapped, datasetReport.Tables[0]);
                    var mappingData = MapperIBCDailyDetailReport(datasetReport.Tables[0].AsEnumerable().Select(c => c));
                    SessionExtension.AddSession(HttpContext.Current.Session, _ibcDailyDetail, mappingData);

                    dailyComplaintsData = new DailyComplaintsDTO()
                   {
                       DailyComplaints = mappingData.DailyComplaints.Select(c => c).Skip(itemPerPage * (currentPage - 1)).Take(itemPerPage).ToList(),
                       Pageindex = currentPage,
                       TotalItems = mappingData.DailyComplaints.Count
                   };
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                var errorMessageFormat = "(GetIBCDailyDetailReport) - Error {0}<br> - Date: {1} -> {2}";
                Helper.SendMailErrorNotice(string.Format(errorMessageFormat, ex.Message, fromDate, toDate));
            }

            return dailyComplaintsData;
        }

        #endregion

        #region licensee report

        /// <summary>
        /// Get Licensee Daily Summary Report.
        /// </summary>
        /// <param name="fromDate">From Date.</param>
        /// <param name="toDate">To Date value.</param>
        /// <returns>Return result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public DailySummaryBO GetLicenseeDailySummaryReport(string fromDate, string toDate)
        {
            var dailySummary = new DailySummaryBO();
            try
            {
                var datasetReport = _repository.GetIBCDailySummaryReport(
                    CastAide.AsDateTime(fromDate),
                    CastAide.AsDateTime(toDate),
                    Enumerations.ReportTypes.Licensee);

                dailySummary.DailySummaries = new List<DailySummaryGroup>();
                dailySummary.ReportDay = CastAide.AsDateTime(fromDate);
                if (datasetReport != null && datasetReport.Tables != null && datasetReport.Tables.Count > 0)
                {
                    var startCol = 4;
                    var tableData = datasetReport.Tables[0];
                    var totalCols = tableData.Columns.Count;

                    var oldMainId = 0;
                    var oldMain = string.Empty;
                    var subTotal = new int[totalCols];
                    var grandTotal = new int[totalCols];

                    var dailyGroup = new DailySummaryGroup();
                    dailyGroup.DailySummaries = new List<DailySummaryDetail>();
                    foreach (DataRow dr in tableData.Rows)
                    {
                        var currentMainId = CastAide.AsInt32(dr["MainCategoryID"].ToString());
                        var currentMain = CastAide.AsString(dr["MainCategory"]);
                        var currentSummary = new DailySummaryDetail();
                        currentSummary.Cells = new List<DailySummaryType>();
                        currentSummary.MainCategory = CastAide.AsString(dr["SubCategory"]);
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

                    var lastSubTotalCells = new List<DailySummaryType>();
                    var grandTotalCells = new List<DailySummaryType>();
                    var lastCellsTotal = 0;
                    var cellsGrandTotal = 0;
                    for (int j = startCol; j < totalCols; j++)
                    {
                        var cellName = tableData.Columns[j].ColumnName;
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
                }

                SessionExtension.AddSession(HttpContext.Current.Session, _licenseeDailySummary, dailySummary);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                var errorMessageFormat = "(GetLicenseeDailySummaryReport) - Error {0}<br> - Date: {1} -> {2}";
                Helper.SendMailErrorNotice(string.Format(errorMessageFormat, ex.Message, fromDate, toDate));
            }

            return dailySummary;
        }

        /// <summary>
        /// Get Licensee Daily Detail Report.
        /// </summary>
        /// <param name="fromDate">From Date.</param>
        /// <param name="toDate">To Date value.</param>
        /// <param name="fromCache">From Cache.</param>
        /// <param name="itemPerPage">Item Per Page.</param>
        /// <param name="currentPage">Current Page.</param>
        /// <returns>Return result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public DailyComplaintsDTO GetLicenseeDailyDetailReport(string fromDate, string toDate, bool fromCache, int itemPerPage, int currentPage)
        {
            var dailyComplaintsData = new DailyComplaintsDTO();
            dailyComplaintsData.DailyComplaints = new List<DailyComplaintsDetail>();
            try
            {
                if (fromCache)
                {
                    var getMappedResult = (DailyComplaintsDTO)SessionExtension.GetSession(HttpContext.Current.Session, _licenseeDailyDetail);
                    if (getMappedResult != null && getMappedResult.DailyComplaints != null && getMappedResult.DailyComplaints.Count > 0)
                    {
                        var pagingMappedResult = new DailyComplaintsDTO()
                        {
                            DailyComplaints = getMappedResult.DailyComplaints.Select(c => c).Skip(itemPerPage * (currentPage - 1)).Take(itemPerPage).ToList(),
                            Pageindex = currentPage,
                            TotalItems = getMappedResult.DailyComplaints.Count
                        };

                        return pagingMappedResult;
                    }
                }

                SessionExtension.AddSession(HttpContext.Current.Session, _licenseeDailyDetail, null);
                CacheService.Delete(_licenseeDailyDetailNotMapped);
                var datasetReport = _repository.GetIBCDailyDetailReport(CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate), Enumerations.ReportTypes.Licensee);
                if (datasetReport != null && datasetReport.Tables != null && datasetReport.Tables.Count > 0)
                {
                    SessionExtension.AddSession(HttpContext.Current.Session, _licenseeDailyDetailNotMapped, datasetReport.Tables[0]);
                    var mappingData = MapperLicenseeDailyDetailReport(datasetReport.Tables[0].AsEnumerable().Select(c => c));
                    SessionExtension.AddSession(HttpContext.Current.Session, _licenseeDailyDetail, mappingData);

                    dailyComplaintsData = new DailyComplaintsDTO()
                    {
                        DailyComplaints = mappingData.DailyComplaints.Select(c => c).Skip(itemPerPage * (currentPage - 1)).Take(itemPerPage).ToList(),
                        Pageindex = currentPage,
                        TotalItems = mappingData.DailyComplaints.Count
                    };
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                var errorMessageFormat = "(GetLicenseeDailyDetailReport) - Error {0}<br> - Date: {1} -> {2}";
                Helper.SendMailErrorNotice(string.Format(errorMessageFormat, ex.Message, fromDate, toDate));
            }

            return dailyComplaintsData;
        }

        #endregion

        /// <summary>
        /// Get Casino Daily Summary Report.
        /// </summary>
        /// <param name="fromDate">From Date.</param>
        /// <param name="toDate">To Date value.</param>
        /// <returns>Return result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
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
                    var casinoPrefix = CasinoSection.Instance[CasinoSection.SystemConfigCollection.Systems];
                    var casinoSubfix = CasinoSection.Instance[CasinoSection.SystemConfigCollection.Communications];
                    var tableData = getData.Tables[0];
                    var oldMainId = 0;
                    var oldMain = string.Empty;
                    var systemSubTotal = new List<SystemDailySummary>();
                    var systemGrandTotal = new List<SystemDailySummary>();

                    for (var i = 0; i < casinoPrefix.Count; i++)
                    {
                        result.SumAllFeedbacks.Add(new int());
                        var cells = new List<DailySummaryType>();
                        for (var j = 0; j < casinoSubfix.Count; j++)
                        {
                            cells.Add(new DailySummaryType() { Value = 0 });
                        }

                        systemSubTotal.Add(new SystemDailySummary()
                        {
                            Sytem = casinoPrefix[i].Name,
                            SystemId = casinoPrefix[i].Id,
                            Cells = cells
                        });
                        var cells2 = new List<DailySummaryType>();
                        for (var j = 0; j < casinoSubfix.Count; j++)
                        {
                            cells2.Add(new DailySummaryType() { Value = 0 });
                        }

                        systemGrandTotal.Add(new SystemDailySummary()
                        {
                            Sytem = casinoPrefix[i].Name,
                            SystemId = casinoPrefix[i].Id,
                            Cells = cells2
                        });
                    }

                    foreach (DataRow dr in tableData.Rows)
                    {
                        var currentMainId = CastAide.AsInt32(dr["MainCategoryID"].ToString());
                        var currentMain = CastAide.AsString(dr["MainCategory"]);
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
                            for (var i = 0; i < casinoPrefix.Count; i++)
                            {
                                var cells = new List<DailySummaryType>();
                                for (var j = 0; j < casinoSubfix.Count; j++)
                                {
                                    cells.Add(new DailySummaryType() { Value = 0 });
                                }

                                systemSubTotal.Add(new SystemDailySummary()
                                {
                                    Sytem = casinoPrefix[i].Name,
                                    SystemId = casinoPrefix[i].Id,
                                    Cells = cells
                                });
                            }
                        }

                        for (var j = 0; j < casinoPrefix.Count; j++)
                        {
                            for (var k = 0; k < casinoSubfix.Count; k++)
                            {
                                var currentVal =
                                    CastAide.AsInt32(dr[string.Format("{0}{1}", casinoPrefix[j].Prefix, casinoSubfix[k].Subfix)]);

                                systemSubTotal[j].Cells[k].Title = casinoSubfix[k].Subfix;
                                systemSubTotal[j].Cells[k].Value = systemSubTotal[j].Cells[k].Value + currentVal;
                                systemGrandTotal[j].Cells[k].Title = casinoSubfix[k].Subfix;
                                systemGrandTotal[j].Cells[k].Value = systemGrandTotal[j].Cells[k].Value + currentVal;
                                result.SumAllFeedbacks[j] += currentVal;
                            }
                        }

                        oldMainId = currentMainId;
                        oldMain = currentMain;
                    }

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
                }

                SessionExtension.AddSession(HttpContext.Current.Session, _casinoDailySummary, result);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                var errorMessageFormat = "(GetCasinoDailySummaryReport) - Error {0}<br> - Date: {1} -> {2}";
                Helper.SendMailErrorNotice(string.Format(errorMessageFormat, ex.Message, fromDate, toDate));
            }

            return result;
        }

        #region casino daily detail
        /// <summary>
        /// Get Casino Daily Detail Report.
        /// </summary>
        /// <param name="fromDate">From Date.</param>
        /// <param name="toDate">To Date value.</param>
        /// <param name="fromCache">From Cache.</param>
        /// <param name="itemPerPage">Item Per Page.</param>
        /// <param name="currentPage">Current Page.</param>
        /// <returns>Return result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public DailyComplaintsDTO GetCasinoDailyDetailReport(string fromDate, string toDate, bool fromCache, int itemPerPage, int currentPage)
        {
            var dailyComplaintsData = new DailyComplaintsDTO();
            dailyComplaintsData.DailyComplaints = new List<DailyComplaintsDetail>();
            try
            {
                if (fromCache)
                {
                    var getMappedResult = CacheService.Get<DailyComplaintsDTO>(_casinoDailyDetail);
                    if (getMappedResult != null && getMappedResult.DailyComplaints != null && getMappedResult.DailyComplaints.Count > 0)
                    {
                        var pagingMappedResult = new DailyComplaintsDTO()
                        {
                            DailyComplaints = getMappedResult.DailyComplaints.Select(c => c).Skip(itemPerPage * (currentPage - 1)).Take(itemPerPage).ToList(),
                        };
                        pagingMappedResult.Pageindex = currentPage;
                        pagingMappedResult.TotalItems = getMappedResult.DailyComplaints.Count;
                        return pagingMappedResult;
                    }
                    else
                    {
                        var sessionData = (DataTable)SessionExtension.GetSession(HttpContext.Current.Session, _casinoDailyDetailNotMapped);
                        if (sessionData != null)
                        {
                            var paging = sessionData.AsEnumerable().Select(c => c).Skip(itemPerPage * (currentPage - 1)).Take(itemPerPage);
                            dailyComplaintsData = MapperIBCDailyDetailReport(paging);
                            dailyComplaintsData.Pageindex = currentPage;
                            dailyComplaintsData.TotalItems = sessionData.Rows.Count;

                            return dailyComplaintsData;
                        }
                    }
                }

                CacheService.Delete(_casinoDailyDetail);
                var datasetReport = _repository.GetCasinoDailyDetailReport(CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate));
                if (datasetReport != null && datasetReport.Tables != null && datasetReport.Tables.Count > 0)
                {
                    var paginOnNewData = datasetReport.Tables[0].AsEnumerable().Select(c => c).Skip(itemPerPage * (currentPage - 1)).Take(itemPerPage);
                    dailyComplaintsData = MapperCasinoDailyDetailReport(paginOnNewData);
                    if (datasetReport.Tables[0].Rows.Count <= itemPerPage)
                    {
                        CacheService.Set(_casinoDailyDetail, dailyComplaintsData);
                    }
                    else
                    {
                        SessionExtension.AddSession(HttpContext.Current.Session, _casinoDailyDetailNotMapped, datasetReport.Tables[0]);

                        Task taskMappingCasinoDetailData = Task.Factory.StartNew(() =>
                        {
                            CacheService.Delete(_casinoDailyDetail);
                            var mappingData = MapperCasinoDailyDetailReport(datasetReport.Tables[0].AsEnumerable().Select(c => c));
                            CacheService.Set(_casinoDailyDetail, mappingData);
                        });
                    }

                    dailyComplaintsData.Pageindex = currentPage;
                    dailyComplaintsData.TotalItems = datasetReport.Tables[0].Rows.Count;
                }
            }
            catch (Exception ex)
            {
                Helper.SendMailErrorNotice(string.Format(
                    "(GetCasinoDailyDetailReport) - Error: {0}<br> - Date: {1} -> {2}", ex.Message, fromDate, toDate));
            }

            return dailyComplaintsData;
        }

        #endregion

        /// <summary>
        /// Get Casino Weekly Summary Report.
        /// </summary>
        /// <param name="fromDate">From Date.</param>
        /// <param name="toDate">To Date value.</param>
        /// <returns>Return result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public DailySummaryBO GetCasinoWeeklySummaryReport(string fromDate, string toDate)
        {
            var dailySummary = new DailySummaryBO();
            try
            {
                var datasetReport = _repository.GetCasinoDailySummaryReport(
                    CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate), 3);

                dailySummary.DailySummaries = new List<DailySummaryGroup>();
                dailySummary.ReportDay = CastAide.AsDateTime(fromDate);
                if (datasetReport != null && datasetReport.Tables != null && datasetReport.Tables.Count > 0)
                {
                    var startCol = 2;
                    var tableData = datasetReport.Tables[0];
                    var totalCols = tableData.Columns.Count;

                    var oldMainId = 0;
                    var oldMain = string.Empty;
                    var subTotal = new int[totalCols];
                    var grandTotal = new int[totalCols];

                    var dailyGroup = new DailySummaryGroup();
                    dailyGroup.DailySummaries = new List<DailySummaryDetail>();
                    foreach (DataRow dr in tableData.Rows)
                    {
                        var currentMainId = CastAide.AsInt32(dr["MainCategoryID"].ToString());
                        var currentMain = CastAide.AsString(dr["MainCategory"]);
                        var currentSummary = new DailySummaryDetail();
                        currentSummary.Cells = new List<DailySummaryType>();
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

                    var lastSubTotalCells = new List<DailySummaryType>();
                    var grandTotalCells = new List<DailySummaryType>();
                    var lastCellsTotal = 0;
                    var cellsGrandTotal = 0;
                    for (int j = startCol; j < totalCols; j++)
                    {
                        var cellName = tableData.Columns[j].ColumnName;
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
                }

                SessionExtension.AddSession(HttpContext.Current.Session, _casinoWeeklySummary, dailySummary);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return dailySummary;
        }

        /// <summary>
        /// Get Auto Suggestion Customer.
        /// </summary>
        /// <param name="userName">User Name.</param>
        /// <returns>Return result.</returns>
        public DataTable GetAutoSuggestionCustomer(string userName)
        {
            var result = new DataTable();
            var getData = _repository.AutoSuggestionCustomer(userName);
            if (getData != null && getData.Tables.Count > 0)
            {
                result = getData.Tables[0];
            }

            return result;
        }

        /// <summary>
        /// Get Resolving Time.
        /// </summary>
        /// <param name="strResolutionDate">Resolution Date.</param>
        /// <param name="strFeedbackDate">Feedback Date.</param>
        /// <returns>Return result.</returns>
        private static string GetResolvingTime(string strResolutionDate, string strFeedbackDate)
        {
            var resolDate = CastAide.AsDateTime(strResolutionDate);
            var feedbackDate = CastAide.AsDateTime(strFeedbackDate);
            var totalDays = resolDate - feedbackDate;
            var resolvingTime = string.Format("{0}:{1}:{2}:{3}", totalDays.Days, totalDays.Hours, totalDays.Minutes, totalDays.Seconds);
            return resolvingTime;
        }

        /// <summary>
        /// Select all feedback by customer id.
        /// </summary>
        /// <param name="custId">Customer id.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <returns>Return result.</returns>
        private List<FeedbackView> SelAllFeedbackByCustId(int custId, string fromDate, string toDate)
        {
            var lstFeedback = _repository.GetFeedbackByCustId(custId, CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate)).ToList().ToList();
            var result = new List<FeedbackView>();

            if (lstFeedback != null && lstFeedback.Count > 0)
            {
                foreach (var feedback in lstFeedback)
                {
                    result.Add(CreateFeedbackView(feedback));
                }
            }

            SessionExtension.AddSession(HttpContext.Current.Session, _lastedFeedbackByCustId, result);
            return result;
        }

        /// <summary>
        /// Getting latest feedback.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="runningNumber">The running number.</param>
        /// <param name="userName">User name.</param>
        /// <param name="isGuest">Is guest customer.</param>
        /// <param name="communicationID">Communication ID.</param>
        /// <param name="mainCategoryID">MainCategory ID.</param>
        /// <param name="subCategoryID">SubCategory ID.</param>
        /// <param name="statusID">Status ID.</param>
        /// <param name="isReviewed">Is Reviewed.</param>
        /// <param name="responseId">Response Id.</param>
        /// <returns>
        /// Return result.
        /// </returns>
        private List<FeedbackView> SelAllLastedFeedback(
            string fromDate,
            string toDate,
            int runningNumber,
            string userName,
            int isGuest,
            int communicationID,
            int mainCategoryID,
            int subCategoryID,
            int statusID,
            int isReviewed,
            int responseId)
        {
            var lstFeedback = _repository.GetFeedback(
                CastAide.AsDateTime(fromDate),
                CastAide.AsDateTime(toDate),
                runningNumber,
                userName,
                isGuest,
                communicationID,
                mainCategoryID,
                subCategoryID,
                statusID,
                isReviewed,
                responseId).ToList();
            var result = new List<FeedbackView>();
            if (lstFeedback != null && lstFeedback.Count > 0)
            {
                foreach (var feedback in lstFeedback)
                {
                    result.Add(CreateFeedbackView(feedback));
                }
            }

            SessionExtension.AddSession(HttpContext.Current.Session, _lastedFeedback, result);
            return result;
        }

        /// <summary>
        /// Getting feedback statistic.
        /// </summary>
        /// <param name="param">Input parameters.</param>
        /// <param name="columnName">Column name.</param>
        /// <param name="columnId">Column Id.</param>
        /// <param name="grandTotal">Grand total.</param>
        /// <returns>Return result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private IEnumerable<FeedbackStatisticsGroup> GetFeedbackStatistic(
            Params param,
            ref ArrayList columnName,
            ref ArrayList columnId,
            ref FeedbackStatistics grandTotal)
        {
            const int StartColNumber = 4;
            var dataTables = _repository.GetFeedbackStatistic(param);
            var list = new List<FeedbackStatisticsGroup>();
            if (dataTables.Count < 2)
            {
                return list;
            }

            var dataTable = dataTables[0];
            var dataTableId = dataTables[1];
            var totalComlumns = dataTable.Columns.Count;
            var totalItems = dataTable.Rows.Count;
            if (totalItems > 0)
            {
                for (var i = StartColNumber; i < totalComlumns; i++)
                {
                    columnId.Add(CastAide.AsInt32(dataTableId.Rows[i - StartColNumber][0].ToString()));
                    columnName.Add(System.Web.HttpUtility.HtmlEncode(dataTable.Columns[i].ColumnName));
                    dataTable.Columns[i].ColumnName = "ColumnName" + dataTable.Columns[i].ColumnName.Trim().Replace(" ", string.Empty);
                }

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

                    float totalCellsByMainCategoryID = 0;
                    foreach (var feedbackGroup in getFeedbackByCategoryId)
                    {
                        mainCategory = CastAide.AsString(feedbackGroup["MainCategory"]);

                        float totalCellsBySubId = 0;
                        cells = new ArrayList();
                        for (var i = StartColNumber; i < totalComlumns; i++)
                        {
                            cells.Add(CastAide.AsSingle(feedbackGroup[i]));
                            totalCellsBySubId += CastAide.AsSingle(feedbackGroup[i]);
                            totalCellsByMainCategoryID += CastAide.AsSingle(feedbackGroup[i]);
                        }

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
                    cells = new ArrayList();
                    for (var i = StartColNumber; i < totalComlumns; i++)
                    {
                        var total = getFeedbackByCategoryId.AsEnumerable().Sum(x => x.Field<int>(dataTable.Columns[i].ColumnName.Trim()));

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
                    list.Add(feedbackStatisticGroup);
                }

                cells = new ArrayList();
                for (var i = StartColNumber; i < totalComlumns; i++)
                {
                    var total = dataTable.AsEnumerable().Sum(x => x.Field<int>(dataTable.Columns[i].ColumnName.Trim()));
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
            }

            return list;
        }

        /// <summary>
        /// Select All Feedback Statistic.
        /// </summary>
        /// <param name="param">Input parameters.</param>
        /// <returns>Return result.</returns>
        private FeedbackStatisticsDTO SelAllFeedbackStatistic(Params param)
        {
            var result = new FeedbackStatisticsDTO();
            var columnNames = new ArrayList();
            var columnIds = new ArrayList();
            var grandTotal = new FeedbackStatistics();
            result.Rows = GetFeedbackStatistic(param, ref columnNames, ref columnIds, ref grandTotal);
            result.ColumnNames = columnNames;
            result.ColumnIDs = columnIds;
            result.Total = result.Rows.Count();
            result.GrandTotal = grandTotal;
            SessionExtension.AddSession(HttpContext.Current.Session, _feedbackStatisticSessionKey, result);
            return result;
        }

        /// <summary>
        /// Select All Feedback Statistic Detail.
        /// </summary>
        /// <param name="param">Input parameters.</param>
        /// <returns>Return result.</returns>
        private List<Feedback> SelAllFeedbackStatisticDetail(Params param)
        {
            var lstFeedback = new List<Feedback>();
            var totalRows = 0;
            lstFeedback = _repository.GetFeedbackStatisticDetail(param, ref totalRows).ToList();
            Task taskMappingFeedbackStatisticDetailData = Task.Factory.StartNew(() =>
            {
                CacheService.Delete(_feedbackStatisticDetailMappedSessionKey);
                var mappingData = MapperFeedbackStatisticReportDetail(lstFeedback);
                CacheService.Set(_feedbackStatisticDetailMappedSessionKey, mappingData);
            });

            SessionExtension.AddSession(HttpContext.Current.Session, _feedbackStatisticDetailSessionKey, lstFeedback);
            return lstFeedback;
        }

        /// <summary>
        /// Mapper Feedback Statistic Report Detail.
        /// </summary>
        /// <param name="feedbackDetail">Feedback Detail.</param>
        /// <returns>Return result.</returns>
        private DataGridViewDTO MapperFeedbackStatisticReportDetail(List<Feedback> feedbackDetail)
        {
            var feedbackStatisticDetailDto = new DataGridViewDTO();
            var feedbackViewPaging = new List<FeedbackView>();
            foreach (var feedback in feedbackDetail)
            {
                feedbackViewPaging.Add(CreateFeedbackView(feedback));
            }

            feedbackStatisticDetailDto.Rows = feedbackViewPaging;
            feedbackStatisticDetailDto.TotalItems = feedbackViewPaging.Count;
            feedbackStatisticDetailDto.PageIndex = 1;
            return feedbackStatisticDetailDto;
        }

        /// <summary>
        /// Select All Top Feedback By Customer Id.
        /// </summary>
        /// <param name="custId">Customer id.</param>
        /// <param name="selectTop">Total feedbacks.</param>
        /// <returns>Return result.</returns>
        private List<FeedbackView> SelAllTopFeedbackByCustId(int custId, int selectTop)
        {
            var lstFeedback = _repository.SelTopFeedbackByCustId(custId, selectTop).ToList();
            var result = new List<FeedbackView>();

            if (lstFeedback != null && lstFeedback.Count > 0)
            {
                foreach (var feedback in lstFeedback)
                {
                    result.Add(CreateFeedbackView(feedback));
                }
            }

            SessionExtension.AddSession(HttpContext.Current.Session, _lastedTopFeedbackByCustId, result);
            return result;
        }

        /// <summary>
        /// Select All Top Feedback.
        /// </summary>
        /// <param name="selectTop">Total feedbacks.</param>
        /// <returns>Return result.</returns>
        private List<FeedbackView> SelAllTopFeedback(int selectTop)
        {
            var lstFeedback = _repository.SelTopFeedback(selectTop).ToList();
            var result = new List<FeedbackView>();
            if (lstFeedback != null && lstFeedback.Count > 0)
            {
                foreach (var feedback in lstFeedback)
                {
                    result.Add(CreateFeedbackView(feedback));
                }
            }

            SessionExtension.AddSession(HttpContext.Current.Session, _lastedTopFeedback, result);
            return result;
        }

        /// <summary>
        /// Mapper IBC Daily Detail Report.
        /// </summary>
        /// <param name="dt">Data rows.</param>
        /// <returns>Return result.</returns>
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
                        Licensee = CastAide.AsString(dr["SiteName"]),
                        CommunicationTool = new CommunicationTool()
                        {
                            ContactBy = CastAide.AsString(dr["Communication"]),
                            Address =
                                string.Format(
                                    "MSN: {0}\n<br> Skype: {1}\n<br> Email: {2}\n<br> Phone: {3}\n<br> Mobile: {4}",
                                    CastAide.AsString(dr["msn"]),
                                    CastAide.AsString(dr["Skype"]),
                                    CastAide.AsString(dr["Email"]),
                                    CastAide.AsString(dr["Phone"]),
                                    CastAide.AsString(dr["MobilePhone"]))
                        },
                        ProductName = CastAide.AsString(dr["Product"]),
                        ExtNo = CastAide.AsString(dr["Station"]),
                        Country = CastAide.AsString(dr["CountryName"]),
                        Category = CastAide.AsString(dr["MainCategory"]),
                        Complaints = complaint,
                        Solution = solution,
                        Status = CastAide.AsString(dr["Status"]),
                        Response = CastAide.AsString(dr["Response"]),
                        HandlyBy = CastAide.AsString(dr["CreatorName"]),
                        CreationTime = DateTimeUtilize.ChangeTimeZoneToClient(dr["CreateDate"].ToString()),
                        ResolutionDate = DateTimeUtilize.ChangeTimeZoneToClient(dr["ResolutionDate"].ToString())
                    });
            }

            return dailyComplaintsData;
        }

        /// <summary>
        /// Mapper Licensee Daily Detail Report.
        /// </summary>
        /// <param name="dt">Data rows.</param>
        /// <returns>Return result.</returns>
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
                    Licensee = CastAide.AsString(dr["SiteName"]),
                    CommunicationTool = new CommunicationTool()
                    {
                        ContactBy = CastAide.AsString(dr["Communication"]),
                        Address = string.Format(
                        "MSN: {0}\n<br> Skype: {1}\n<br> Email: {2}\n<br> Phone: {3}\n<br> Mobile: {4}",
                        CastAide.AsString(dr["msn"]),
                        CastAide.AsString(dr["Skype"]),
                        CastAide.AsString(dr["Email"]),
                        CastAide.AsString(dr["Phone"]),
                        CastAide.AsString(dr["MobilePhone"]))
                    },
                    ExtNo = CastAide.AsString(dr["Station"]),
                    ProductName = CastAide.AsString(dr["Product"]),
                    Country = CastAide.AsString(dr["CountryName"]),
                    Category = CastAide.AsString(dr["MainCategory"]),
                    Complaints = complaint,
                    Solution = solution,
                    Status = CastAide.AsString(dr["Status"]),
                    Response = CastAide.AsString(dr["Response"]),
                    HandlyBy = CastAide.AsString(dr["CreatorName"]),
                    CreationTime = DateTimeUtilize.ChangeTimeZoneToClient(dr["CreateDate"].ToString()),
                    ResolutionDate = DateTimeUtilize.ChangeTimeZoneToClient(dr["ResolutionDate"].ToString())
                });
            }

            return dailyComplaintsData;
        }

        /// <summary>
        /// Mapper Casino Daily Detail Report.
        /// </summary>
        /// <param name="dt">Data rows.</param>
        /// <returns>Return result.</returns>
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
                        CreationTime = DateTimeUtilize.ChangeTimeZoneToClient(dr["CreateDate"].ToString()),
                        ResolutionDate = DateTimeUtilize.ChangeTimeZoneToClient(dr["ResolutionDate"].ToString())
                    });
            }

            return dailyComplaintsData;
        }

        private Result ReturnErrorResult(int resultId, string resultMessageResourceId)
        {
            var message = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, resultMessageResourceId);
            return new Result(resultId, message);
        }

        /// <summary>
        /// Checks the feedback permission.
        /// </summary>
        /// <param name="feedbackStatusId">The status of feedback.</param>
        /// <param name="isOverrideCustomerInfo">If set to <c>true</c> [is override customer information].</param>
        /// <param name="isAddGuestFeedback">If set to <c>true</c> [is add guest feedback].</param>
        /// <returns>
        /// Check result.
        /// </returns>
        private Result CheckFeedbackPermission(int feedbackStatusId, bool isOverrideCustomerInfo, bool isAddGuestFeedback)
        {
            if (!_permissionService.HasPermission(FunctionPermission.CreateFeedback.ToString(), Constant.Policy.Host))
            {
                return ReturnErrorResult((int)Enumerations.ErrorCode.UpdateFeedback, "lblNoCreateFeedbackPermission");
            }

            if (feedbackStatusId.Equals((int)Enumerations.Functions.CRMStatus.Deleted) &&
                !_permissionService.HasPermission(FunctionPermission.DeleteFeedback.ToString(), Constant.Policy.Host))
            {
                return ReturnErrorResult((int)Enumerations.ErrorCode.DeleteFeedback, "lblNoDeletedFeedbackPermission");
            }

            if ((isOverrideCustomerInfo || isAddGuestFeedback) &&
                !_permissionService.HasPermission(FunctionPermission.CustomerInfo, Constant.Policy.Host))
            {
                return ReturnErrorResult((int)Enumerations.ErrorCode.DeleteFeedback, "lblNoUpdateCustomerPermission");
            }

            return new Result(0, string.Empty);
        }

        /// <summary>
        /// Creates the feedback view.
        /// </summary>
        /// <param name="feedback">The feedback.</param>
        /// <returns>The feedback view.</returns>
        private FeedbackView CreateFeedbackView(Feedback feedback)
        {
            return new FeedbackView()
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
                DurationTime = feedback.ResolutionTime != null ? GetResolvingTime(feedback.ResolutionTime, feedback.FeedbackDate) : string.Empty,
                Description = feedback.Description
            };
        }
    }
}