(function () {
    "use strict";
    /* global hotKeyManager,
    viewResources,
    crmConstants,
    currentDate: true,
    initFromDate: true,
    weekPickerManager,
    moment,
    sharedScriptManager */
    /* jshint camelcase: false */
    /* jshint maxstatements: 412 */
    angular.module('report-dynamicreport-application').controller('dynamicReportController', ['$scope', '$dynamicReportService',
    function ($scope, $dynamicReportService) {
        weekPickerManager.registerWeekPicker();

        $scope.isSetFocus = false;
        $scope.isShowFilter = true;
        $scope.isUpdatingDate = false;
        $scope.crmConstants = crmConstants;

        $scope.reportTypeOptions = null;
        $scope.fromDateOptions = null;
        $scope.toDateOptions = null;
        $scope.fromWeekOptions = null;
        $scope.toWeekOptions = null;
        $scope.fromMonthOptions = null;
        $scope.toMonthOptions = null;
        $scope.fromYearOptions = null;
        $scope.toYearOptions = null;
        $scope.productOptions = null;
        $scope.siteOptions = null;
        $scope.statusOptions = null;
        $scope.responseOptions = null;
        $scope.roleOptions = null;
        $scope.communicationToolOptions = null;
        $scope.mainCategoryOptions = null;
        $scope.subCategoryOptions = null;
        $scope.extensionOptions = null;
        $scope.defaultFocusOptions = null;
        $scope.downloadFileOptions = null;

        $scope.feedbackSubjectTemplateList = [];
        $scope.feedbackSubjectTemplateTotalPage = 0;
        $scope.isLoadedFeedbackSubjectTemplate = false;
        $scope.isLoadedAllSetting = false;
        $scope.subjectOptions = null;

        $scope.searchCriteria = {
            ReportType: null,
            FromDate: new Date(initFromDate),
            ToDate: new Date(currentDate),

            Product: null,
            Site: null,
            Status: null,
            Response: null,
            Role: null,
            CommunicationTool: null,
            MainCategory: null,
            SubCategory: null,
            Handler: null,
            Reviewer: null,
            Extension: null,
            Subject: null,
            PageIndex: 0,
            PageSize: sharedScriptManager.cachedData.defaultItemsPerPage,
            IsGetFromCached: false
        };
        $scope.dynamicReportOptions = {
            DataList: null,
            IsResize: false,
            IsUpdateGrid: false
        };
        $scope.lastSelectedParams = null;

        $scope.initController = function () {
            $scope.getNumberOfFeedbackSubjectTemplate();
            $scope.getAllSetting();

            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "Report_DynamicReport"
            });

            $scope.initPicker();

            $scope.defaultFocusOptions = {
                IsSetFocus: false,
                OnChangeEvent: $scope.completeSetFocus
            };

            $scope.defaultFocusOptions.IsSetFocus = true;
            $scope.$apply();
        };

        $scope.invokeHotKey_CtrlS = function () {
            $scope.getDynamicReport();
        };
        $scope.initPicker = function () {
            $scope.fromWeekOptions = {
                CurrentDate: currentDate,
                InitDate: initFromDate,
                MaxDate: currentDate,
                IsRerenderPicker: false,
                OnChangeDateEvent: $scope.updateFromWeek
            };

            $scope.toWeekOptions = {
                CurrentDate: currentDate,
                InitDate: initFromDate,
                MaxDate: currentDate,
                IsRerenderPicker: false,
                OnChangeDateEvent: $scope.updateToWeek
            };

            $scope.fromWeekOptions.IsRerenderPicker = true;
            $scope.toWeekOptions.IsRerenderPicker = true;
            $scope.$apply();

            $scope.reportTypeOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateReportType
            };

            $scope.$apply();

            currentDate = new Date(currentDate);
            initFromDate = new Date(initFromDate);
            $scope.fromMonthOptions = {
                CurrentDate: currentDate,
                InitDate: initFromDate,
                MaxDate: initFromDate,
                MinDate: new Date(initFromDate.getFullYear() - sharedScriptManager.cachedData.numberOfYearPerSelection,
                initFromDate.getMonth(),
                initFromDate.getDate()),
                IsRerenderPicker: false,
                OnChangeEvent: $scope.updateFromMonth,
                SelectedYear: initFromDate.getFullYear(),
                SelectedMonth: initFromDate.getMonth() + 1
            };

            $scope.fromMonthOptions.IsRerenderPicker = true;
            $scope.$apply();

            $scope.toMonthOptions = {
                CurrentDate: currentDate,
                InitDate: currentDate,
                MaxDate: currentDate,
                MinDate: currentDate,
                IsRerenderPicker: false,
                OnChangeEvent: $scope.updateToMonth,
                SelectedYear: currentDate.getFullYear(),
                SelectedMonth: currentDate.getMonth() + 1
            };

            $scope.toMonthOptions.IsRerenderPicker = true;
            $scope.$apply();

            $scope.fromYearOptions = {
                CurrentDate: currentDate,
                InitDate: initFromDate,
                MaxDate: initFromDate,
                MinDate: new Date(initFromDate.getFullYear() - sharedScriptManager.cachedData.numberOfYearPerSelection,
                initFromDate.getMonth(),
                initFromDate.getDate()),
                IsRerenderPicker: false,
                OnChangeEvent: $scope.updateFromYear,
                SelectedYear: initFromDate.getFullYear()
            };

            $scope.fromYearOptions.IsRerenderPicker = true;
            $scope.$apply();

            $scope.toYearOptions = {
                CurrentDate: currentDate,
                InitDate: currentDate,
                MaxDate: currentDate,
                MinDate: currentDate,
                IsRerenderPicker: false,
                OnChangeEvent: $scope.updateToYear,
                SelectedYear: currentDate.getFullYear()
            };

            $scope.toYearOptions.IsRerenderPicker = true;
            $scope.$apply();

            $scope.productOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedProduct
            };

            $scope.siteOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedSite,
                AllText: viewResources.All,
                IsIncludeNA: true
            };

            $scope.statusOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedStatus
            };
            $scope.responseOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedResponse
            };

            $scope.roleOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedRole
            };
            $scope.communicationToolOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedCommunicationTool
            };
            $scope.mainCategoryOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedMainCategory
            };
            $scope.subCategoryOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedSubCategory
            };
            $scope.extensionOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedExtension
            };

            $scope.$apply();

            $scope.fromDateOptions = {
                CurrentDate: currentDate,
                InitDate: initFromDate,
                MaxDate: currentDate,
                IsRerenderPicker: false,
                OnChangeDateEvent: $scope.updateFromDate,
                CompleteRender: $scope.CompleteRenderFromDate
            };

            $scope.toDateOptions = {
                CurrentDate: currentDate,
                InitDate: currentDate,
                MaxDate: currentDate,
                IsRerenderPicker: false,
                OnChangeDateEvent: $scope.updateToDate,
                CompleteRender: $scope.CompleteRenderToDate
            };

            $scope.updateFromDatePicker();
            $scope.updateToDatePicker();
        };
        $scope.getAllSetting = function () {
            $scope.BlockUI();
            $scope.isLoadedAllSite = false;
            $dynamicReportService.getAllSetting()
            .then(function (data) {
                var allSetting = data.Result;
                $scope.reportTypeOptions.DataList = allSetting.PeriodTypeList;
                $scope.productOptions.DataList = allSetting.ProductList;
                $scope.siteOptions.DataList = allSetting.SiteList;
                $scope.statusOptions.DataList = allSetting.StatusList;
                $scope.responseOptions.DataList = allSetting.ResponseList;
                $scope.roleOptions.DataList = allSetting.RoleList;
                $scope.communicationToolOptions.DataList = allSetting.CommunicationToolList;
                $scope.mainCategoryOptions.DataList = allSetting.MainCategoryList;
                $scope.subCategoryOptions.DataList = $scope.mainCategoryOptions.DataList[0].ChildSettings;
                $scope.extensionOptions.DataList = allSetting.ExtensionList;
                $scope.isLoadedAllSetting = true;
                $scope.completedLoadFilterInfo();
            },
            function () {
                $scope.isLoadedAllSetting = true;
                $scope.completedLoadFilterInfo();
            });
        };
        $scope.completedLoadFilterInfo = function () {
            $scope.searchCriteria.ReportType = $scope.reportTypeOptions.DataList[0];
            $scope.searchCriteria.Product = $scope.productOptions.DataList[0];
            $scope.searchCriteria.Site = {
                Id: crmConstants.Enums.Site.NotApply,
                Name: viewResources.All
            };
            $scope.searchCriteria.Status = $scope.statusOptions.DataList[0];
            $scope.searchCriteria.Response = $scope.responseOptions.DataList[0];
            $scope.searchCriteria.Role = $scope.roleOptions.DataList[0];
            $scope.searchCriteria.CommunicationTool = $scope.communicationToolOptions.DataList[0];
            $scope.searchCriteria.MainCategory = $scope.mainCategoryOptions.DataList[0];
            $scope.searchCriteria.SubCategory = $scope.subCategoryOptions.DataList[0];
            $scope.searchCriteria.Extension = {
                Id: crmConstants.Enums.Extension.NotApply,
                Name: viewResources.All
            };

            if ($scope.isLoadedFeedbackSubjectTemplate) {
                $scope.subjectOptions = {
                    NumberOfSuggestItems: 15,
                    SubjectList: $scope.feedbackSubjectTemplateList,
                    Subject: '',
                    IsUpdate: true,
                    OnChangeEvent: $scope.updateSelectedSubject
                };
            }

            if ($scope.isLoadedFeedbackSubjectTemplate &&
            $scope.isLoadedAllSetting) {
                $scope.unblockUI();
            }
        };
        $scope.updateFromDatePicker = function () {
            $scope.fromDateOptions.IsRerenderPicker = true;
            if (!$scope.$$phase) {
                $scope.$apply();
            }
        };
        $scope.updateToDatePicker = function () {
            $scope.toDateOptions.IsRerenderPicker = true;
            if (!$scope.$$phase) {
                $scope.$apply();
            }
        };
        $scope.CompleteRenderFromDate = function () {
            $scope.fromDateOptions.IsRerenderPicker = false;
        };
        $scope.CompleteRenderToDate = function () {
            $scope.toDateOptions.IsRerenderPicker = false;
        };

        $scope.updateFromDate = function (selectedDate) {
            $scope.searchCriteria.FromDate = selectedDate;
            $scope.toDateOptions.MinDate = selectedDate;
            $scope.updateToDatePicker();
        };
        $scope.updateToDate = function (selectedDate) {
            $scope.searchCriteria.ToDate = selectedDate;
            $scope.fromDateOptions.MaxDate = selectedDate;
            $scope.updateFromDatePicker();
        };
        $scope.updateReportType = function (reportType) {
            $scope.searchCriteria.ReportType = reportType;
            var reportTypeId = parseInt(reportType.Id);

            $scope.searchCriteria.FromDate = new Date(initFromDate);
            $scope.searchCriteria.ToDate = new Date(currentDate);
            switch (reportTypeId) {
                case crmConstants.Enums.PeriodType.Daily:
                    $scope.fromDateOptions.IsRerenderPicker = true;
                    $scope.toDateOptions.IsRerenderPicker = true;
                    break;
                case crmConstants.Enums.PeriodType.Weekly:
                    var date = new Date(currentDate);
                    var dat = date.getDate();
                    var day = date.getDay();

                    $scope.searchCriteria.FromDate = new Date(date.getFullYear(), date.getMonth(), 1);
                    $scope.searchCriteria.FromDate.setDate(dat - day);
                    $scope.searchCriteria.ToDate = new Date(currentDate);
                    break;
                case crmConstants.Enums.PeriodType.Monthly:
                    $scope.fromMonthOptions.IsRerenderPicker = true;
                    $scope.toMonthOptions.IsRerenderPicker = true;
                    break;
                case crmConstants.Enums.PeriodType.Yearly:
                    $scope.fromYearOptions.IsRerenderPicker = true;
                    $scope.toYearOptions.IsRerenderPicker = true;
                    break;
            }

            $scope.$apply();
        };
        $scope.updateFromWeek = function (fromDate, toDate, picker, isAlreadyApply) {
            /* jshint unused: false */
            $scope.searchCriteria.FromDate = fromDate;
            if (!isAlreadyApply) {
                $scope.$apply();
            }
        };
        $scope.updateToWeek = function (fromDate, toDate, picker, isAlreadyApply) {
            /* jshint unused: false */
            $scope.searchCriteria.ToDate = toDate;
            if (!isAlreadyApply) {
                $scope.$apply();
            }
        };
        $scope.updateFromMonth = function (month, year, isAlreadyApply) {
            var selectedDate = new Date(year, month - 1, 1);
            var selectedYear = selectedDate.getFullYear();
            var toDate = $scope.searchCriteria.ToDate;
            var maxFromYear = selectedYear + sharedScriptManager.cachedData.numberOfYearPerSelection;
            var maxFromMonth = 11;
            if (toDate.getFullYear() < maxFromYear) {
                maxFromYear = toDate.getFullYear();
                maxFromMonth = toDate.getMonth();
            }

            $scope.fromMonthOptions = {
                CurrentDate: selectedDate,
                InitDate: initFromDate,
                MaxDate: new Date(maxFromYear, maxFromMonth, 1),
                MinDate: new Date(selectedDate.getFullYear() - sharedScriptManager.cachedData.numberOfYearPerSelection, selectedDate.getMonth(), 1),
                IsRerenderPicker: false,
                OnChangeEvent: $scope.updateFromMonth,
                SelectedYear: year,
                SelectedMonth: month
            };

            var maxToYear = toDate.getFullYear() + sharedScriptManager.cachedData.numberOfYearPerSelection;
            var maxToMonth = 11;
            if (currentDate.getFullYear() < maxToYear) {
                maxToYear = currentDate.getFullYear();
                maxToMonth = currentDate.getMonth();
            }

            $scope.toMonthOptions = {
                CurrentDate: toDate,
                InitDate: currentDate,
                MaxDate: new Date(maxToYear, maxToMonth, 1),
                MinDate: new Date(selectedDate.getFullYear(), selectedDate.getMonth(), selectedDate.getDate()),
                IsRerenderPicker: false,
                OnChangeEvent: $scope.updateToMonth,
                SelectedYear: $scope.searchCriteria.ToDate.getFullYear(),
                SelectedMonth: $scope.searchCriteria.ToDate.getMonth() + 1
            };

            $scope.searchCriteria.FromDate = selectedDate;
            $scope.toMonthOptions.IsRerenderPicker = true;
            if (!isAlreadyApply) {
                $scope.$apply();
            }
        };
        $scope.updateToMonth = function (month, year, isAlreadyApply) {
            var selectedDate = new Date(year, month, 0);
            var selectedYear = selectedDate.getFullYear();
            var maxToYear = selectedYear + sharedScriptManager.cachedData.numberOfYearPerSelection;
            var maxToMonth = 11;
            if (currentDate.getFullYear() < maxToYear) {
                maxToYear = currentDate.getFullYear();
                maxToMonth = currentDate.getMonth();
            }

            var fromDate = $scope.searchCriteria.FromDate;
            $scope.toMonthOptions = {
                CurrentDate: selectedDate,
                InitDate: currentDate,
                MaxDate: new Date(maxToYear, maxToMonth, 1),
                MinDate: new Date(fromDate.getFullYear(), fromDate.getMonth(), fromDate.getDate()),
                IsRerenderPicker: false,
                OnChangeEvent: $scope.updateToMonth,
                SelectedYear: year,
                SelectedMonth: month
            };

            $scope.fromMonthOptions = {
                CurrentDate: fromDate,
                InitDate: currentDate,
                MaxDate: new Date(selectedDate.getFullYear(), selectedDate.getMonth(), selectedDate.getDate()),
                MinDate: new Date(fromDate.getFullYear() - sharedScriptManager.cachedData.numberOfYearPerSelection, fromDate.getMonth(), 1),
                IsRerenderPicker: false,
                OnChangeEvent: $scope.updateFromMonth,
                SelectedYear: $scope.searchCriteria.FromDate.getFullYear(),
                SelectedMonth: $scope.searchCriteria.FromDate.getMonth() + 1
            };

            $scope.searchCriteria.ToDate = selectedDate;
            $scope.fromMonthOptions.IsRerenderPicker = true;
            if (!isAlreadyApply) {
                $scope.$apply();
            }
        };
        $scope.updateFromYear = function (fromYear, isAlreadyApply) {
            var selectedDate = new Date(fromYear, 0, 1);
            var selectedYear = fromYear;
            var toDate = $scope.searchCriteria.ToDate;
            var maxFromYear = selectedYear + sharedScriptManager.cachedData.numberOfYearPerSelection;
            if (toDate.getFullYear() < maxFromYear) {
                maxFromYear = toDate.getFullYear();
            }

            $scope.fromYearOptions = {
                CurrentDate: selectedDate,
                InitDate: initFromDate,
                MaxDate: new Date(maxFromYear, 11, 1),
                MinDate: new Date(selectedDate.getFullYear() - sharedScriptManager.cachedData.numberOfYearPerSelection, 0, 1),
                IsRerenderPicker: false,
                OnChangeEvent: $scope.updateFromYear,
                SelectedYear: fromYear
            };

            var maxToYear = toDate.getFullYear() + sharedScriptManager.cachedData.numberOfYearPerSelection;
            if (currentDate.getFullYear() < maxToYear) {
                maxToYear = currentDate.getFullYear();
            }

            $scope.toYearOptions = {
                CurrentDate: toDate,
                InitDate: currentDate,
                MaxDate: new Date(maxToYear, 11, 1),
                MinDate: new Date(selectedDate.getFullYear(), 0, 1),
                IsRerenderPicker: true,
                OnChangeEvent: $scope.updateToYear,
                SelectedYear: $scope.searchCriteria.ToDate.getFullYear()
            };

            $scope.searchCriteria.FromDate = selectedDate;
            $scope.searchCriteria.FromDate = new Date(fromYear, 0, 1);
            if (!isAlreadyApply) {
                $scope.toYearOptions.IsRerenderPicker = true;
                $scope.$apply();
            }
        };
        $scope.updateToYear = function (toYear, isAlreadyApply) {
            var selectedDate = new Date(toYear, 0, 1);
            var selectedYear = toYear;
            var maxToYear = selectedYear + sharedScriptManager.cachedData.numberOfYearPerSelection;
            if (currentDate.getFullYear() < maxToYear) {
                maxToYear = currentDate.getFullYear();
            }

            var fromDate = $scope.searchCriteria.FromDate;

            $scope.toYearOptions = {
                CurrentDate: selectedDate,
                InitDate: currentDate,
                MaxDate: new Date(maxToYear, 12, 0),
                MinDate: new Date(fromDate.getFullYear(), 0, 1),
                IsRerenderPicker: false,
                OnChangeEvent: $scope.updateToYear,
                SelectedYear: toYear
            };

            $scope.fromYearOptions = {
                CurrentDate: fromDate,
                InitDate: currentDate,
                MaxDate: new Date(selectedDate.getFullYear(), 11, 1),
                MinDate: new Date(fromDate.getFullYear() - sharedScriptManager.cachedData.numberOfYearPerSelection, 0, 1),
                IsRerenderPicker: false,
                OnChangeEvent: $scope.updateFromYear,
                SelectedYear: $scope.searchCriteria.FromDate.getFullYear()
            };

            $scope.searchCriteria.ToDate = selectedDate;
            $scope.fromYearOptions.IsRerenderPicker = true;
            $scope.searchCriteria.ToDate = new Date(toYear, 12, 0);
            if (!isAlreadyApply) {
                $scope.$apply();
            }
        };
        $scope.completeSetFocus = function () {
            $scope.defaultFocusOptions.IsSetFocus = false;
        };
        $scope.getNumberOfFeedbackSubjectTemplate = function () {
            $scope.BlockUI();
            $dynamicReportService.getNumberOfFeedbackSubjectTemplate()
            .then(function (data) {
                var numberOfFeedbackItem = data.Result;
                $scope.feedbackSubjectTemplateList = [];
                var totalPage = Math.round((numberOfFeedbackItem / sharedScriptManager.cachedData.maxFeedbackSubjectTemplateNumberOnLoad) + 0.5);
                $scope.feedbackSubjectTemplateTotalPage = totalPage;
                $scope.isLoadedFeedbackSubjectTemplate = false;
                for (var i = 1; i <= totalPage; i++) {
                    $scope.getFeedbackSubjectTemplateByPagingIndex(i);
                }
            },
            function () {
                $scope.isLoadedFeedbackSubjectTemplate = true;
            });
        };
        $scope.getFeedbackSubjectTemplateByPagingIndex = function (pagingIndex) {
            $scope.BlockUI();
            $dynamicReportService.getFeedbackSubjectTemplateByPagingIndex({ PagingIndex: pagingIndex })
            .then(function (data) {
                var feedbackSubjectTemplate = data.Result;
                if (feedbackSubjectTemplate != null && feedbackSubjectTemplate.length > 0) {
                    $scope.feedbackSubjectTemplateList = $scope.feedbackSubjectTemplateList.concat(feedbackSubjectTemplate);
                }

                if ($scope.feedbackSubjectTemplateTotalPage === pagingIndex) {
                    $scope.isLoadedFeedbackSubjectTemplate = true;
                    $scope.completedLoadFilterInfo();
                }
            },
            function () {
                if ($scope.feedbackSubjectTemplateTotalPage === pagingIndex) {
                    $scope.isLoadedFeedbackSubjectTemplate = true;
                    $scope.completedLoadFilterInfo();
                }
            });
        };

        $scope.updateSelectedStatus = function (status) {
            $scope.searchCriteria.Status = status;
            $scope.$apply();
        };
        $scope.updateSelectedProduct = function (product) {
            $scope.searchCriteria.Product = product;
            $scope.$apply();
        };
        $scope.updateSelectedResponse = function (response) {
            $scope.searchCriteria.Response = response;
            $scope.$apply();
        };
        $scope.updateSelectedRole = function (role) {
            $scope.searchCriteria.Role = role;
            $scope.$apply();
        };
        $scope.updateSelectedCommunicationTool = function (communicationTool) {
            $scope.searchCriteria.CommunicationTool = communicationTool;
            $scope.$apply();
        };
        $scope.updateSelectedMainCategory = function (mainCategory) {
            $scope.searchCriteria.MainCategory = mainCategory;
            $scope.subCategoryOptions.DataList = mainCategory.ChildSettings;
            $scope.searchCriteria.SubCategory = mainCategory.ChildSettings[0];
            $scope.$apply();
        };
        $scope.updateSelectedSubCategory = function (subCategory) {
            $scope.searchCriteria.SubCategory = subCategory;
            $scope.$apply();
        };
        $scope.updateSelectedExtension = function (extension) {
            $scope.searchCriteria.Extension = extension;
            $scope.$apply();
        };
        $scope.updateSelectedSite = function (site) {
            $scope.searchCriteria.Site = site;
            $scope.$apply();
        };
        $scope.getDynamicReport = function () {
            $scope.BlockUI();
            var getReportCriteria = {
                ReportType: $scope.searchCriteria.ReportType.Id,
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),

                ProductId: $scope.searchCriteria.Product.Id,
                SiteId: $scope.searchCriteria.Site.Id,
                StatusId: $scope.searchCriteria.Status.Id,
                ResponseId: $scope.searchCriteria.Response.Id,
                RoleId: $scope.searchCriteria.Role.Id,
                CommunicationToolId: $scope.searchCriteria.CommunicationTool.Id,
                MainCategoryId: $scope.searchCriteria.MainCategory.Id,
                SubCategoryId: $scope.searchCriteria.SubCategory.Id,
                Handler: $scope.searchCriteria.Handler,
                Reviewer: $scope.searchCriteria.Reviewer,
                ExtensionId: $scope.searchCriteria.Extension.Id,
                Subject: $scope.searchCriteria.Subject,
                PageIndex: $scope.searchCriteria.PageIndex,
                PageSize: $scope.searchCriteria.PageSize,
                IsGetFromCached: false
            };

            $scope.lastSelectedParams = getReportCriteria;
            $dynamicReportService.getDynamicReport(getReportCriteria)
            .then(function (data) {
                $scope.dynamicReportOptions.DataList = data.Result;
                $scope.dynamicReportOptions.IsUpdateGrid = true;
                $scope.unblockUI();
            },
            function () {
                $scope.unblockUI();
            });
        };
        $scope.pagingDynamicReport = function (pageIndex, pageSize) {
            $scope.BlockUI();
            $scope.lastSelectedParams.PageIndex = pageIndex;
            $scope.lastSelectedParams.PageSize = pageSize;
            $scope.lastSelectedParams.IsGetFromCached = true;
            $dynamicReportService.getDynamicReport($scope.lastSelectedParams)
            .then(function (data) {
                $scope.dynamicReportOptions.DataList = data.Result;
                $scope.dynamicReportOptions.IsUpdateGrid = true;
                $scope.unblockUI();
            },
            function () {
                $scope.unblockUI();
            });
        };
        $scope.updateSelectedSubject = function (subject) {
            $scope.searchCriteria.Subject = subject;
            $scope.$apply();
        };

        $scope.viewCommunicationLog = function (communicationLog) {
            var url = window.rootUrl + "CommunicationLog/ViewDetailById?" + $.param({
                logId: communicationLog.RunningNumber
            });

            parent.OpenPopup(
            url,
            "Communication Log Detail",
            $(parent.document).width() - 300,
            $(parent.document).height() - 50,
            true,
            false,
            parent.window,
            3000,
            null,
            $scope.closeEditView,
            { my: "center top+50", at: "center top" },
            null);
        };
        $scope.setFocus = function () {
            $scope.$apply(function () {
                $scope.defaultFocusOptions.IsSetFocus = true;
            });
        };
        $scope.closeEditView = function () {
            $scope.setFocus();
        };
        $scope.exportReport = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "DynamicReport/ExportDynamicReport",
                Params: $scope.lastSelectedParams
            };

            $scope.$apply();
        };

        $scope.showHideFilterBox = function () {
            $scope.isShowFilter = !$scope.isShowFilter;
            $scope.dynamicReportOptions.IsUpdateGrid = true;
            $scope.dynamicReportOptions.IsResize = true;
        };

        $scope.unblockUI = function () {
            try {
                parent.$.unblockUI();
            } catch (e) {
            }

            hotKeyManager.CachedData.isBlockingUI = false;
        };
        $scope.BlockUI = function () {
            hotKeyManager.CachedData.isBlockingUI = true;
            try {
                parent.BlockUI();
            } catch (e) {
            }
        };
        angular.element(document).ready(function () {
            $scope.initController();
        });
    } ]);
})();