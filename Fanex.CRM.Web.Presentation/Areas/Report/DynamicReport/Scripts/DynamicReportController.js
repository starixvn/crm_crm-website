(function () {
    "use strict";
    /* global hotKeyManager,
    SettingManager,
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
        $scope.subProductOptions = null;
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
        $scope.TextSettings = {
            Filter: "Search",
            NoItemChecked: "N/A"
        };

        $scope.searchCriteria = {
            ReportType: null,
            FromDate: new Date(initFromDate),
            ToDate: new Date(currentDate),

            Product: null,
            SubProduct: null,
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

        $scope.getAllSetting = function () {
            $scope.BlockUI();
            $scope.isLoadedAllSite = false;
            $dynamicReportService.getAllSetting()
            .then(function (data) {
                var allSetting = data.Result;
                $scope.reportTypeOptions.DataList = allSetting.PeriodTypeList;

                $scope.productOptions.DataList = allSetting.ProductList;
                $scope.updateSelectedProduct($scope.productOptions.DataList[0]);

                $scope.siteOptions.DataList = allSetting.SiteList;
                $scope.statusOptions.DataList = allSetting.StatusList;
                $scope.responseOptions.DataList = allSetting.ResponseList;
                $scope.roleOptions.DataList = allSetting.RoleList;
                $scope.communicationToolOptions.DataList = allSetting.CommunicationToolList;
                $scope.mainCategoryOptions.DataList = allSetting.MainCategoryList;
                $scope.subCategoryOptions.DataList = $scope.mainCategoryOptions.DataList[0].ChildSettings;
                $scope.extensionOptions.DataList = allSetting.ExtensionList;
                $scope.isLoadedAllSetting = true;

                $scope.siteDropdownSettings = {
                    idMember: "Id",
                    nameMember: "Name",
                    sourceData: allSetting.SiteList,
                    childrenMember: "ChildSites"
                };

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

            var maxToDate = new Date(selectedDate.getTime());
            maxToDate.setMonth(selectedDate.getMonth() + 12);
            maxToDate.setDate(selectedDate.getDate() - 1);
            $scope.toDateOptions.MinDate = selectedDate;
            $scope.toDateOptions.MaxDate = (maxToDate < currentTime) ? maxToDate : currentTime;
            $scope.updateToDatePicker();           
        };
        $scope.updateToDate = function (selectedDate) {
            $scope.searchCriteria.ToDate = selectedDate;
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
            
            var maxToDate = new Date(fromDate.getTime());
            maxToDate.setMonth(fromDate.getMonth() + 12);
            maxToDate.setDate(fromDate.getDate() - 1);
            var maxToDateEndWeek = new Date(maxToDate.getFullYear(), maxToDate.getMonth(), 1);
            maxToDateEndWeek.setDate(maxToDate.getDate() - maxToDate.getDay() + 6);
            maxToDate = maxToDateEndWeek;

            var currentToDate = $scope.searchCriteria.ToDate;
            if (currentToDate < fromDate) {
                currentToDate = fromDate;
            }
            else if (currentToDate > maxToDate) {
                currentToDate = maxToDate;
            }
            $scope.searchCriteria.ToDate = currentToDate;

            $scope.toWeekOptions = {
                CurrentDate: currentToDate,
                InitDate: initFromDate,
                MinDate: fromDate,
                MaxDate: (maxToDate < currentTime) ? maxToDate : currentTime,
                IsRerenderPicker: false,
                OnChangeDateEvent: $scope.updateToWeek
            };

            $scope.toWeekOptions.IsRerenderPicker = true;
            if (!isAlreadyApply) {
                $scope.$apply();
            }
        };
        $scope.updateToWeek = function (fromDate, toDate, picker, isAlreadyApply) {
            console.log("update to week");
            /* jshint unused: false */
            $scope.searchCriteria.ToDate = toDate;
            if (!isAlreadyApply) {
                $scope.$apply();
            }
        };
       
        $scope.updateFromMonth = function (month, year, isAlreadyApply) {
            var selectedDate = new Date(year, month - 1, 1);
            var selectedYear = selectedDate.getFullYear();

            $scope.fromMonthOptions.SelectedYear = year;
            $scope.fromMonthOptions.SelectedMonth = month;
            
            var maxToDate = new Date(selectedDate.getTime());
            maxToDate.setMonth(selectedDate.getMonth() + 12);
            maxToDate.setDate(selectedDate.getDate() - 1);            
            $scope.toMonthOptions = {
                CurrentDate: $scope.searchCriteria.ToDate,
                InitDate: currentDate,
                MaxDate: (maxToDate < currentTime) ? maxToDate : currentTime,
                MinDate: selectedDate,
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

            $scope.toMonthOptions.SelectedYear = year;
            $scope.toMonthOptions.SelectedMonth = month;            

            $scope.searchCriteria.ToDate = selectedDate;
            if (!isAlreadyApply) {
                $scope.$apply();
            }
        };
        $scope.updateFromYear = function (fromYear, isAlreadyApply) {
            var fromDate = new Date(fromYear, 0, 1);  
            $scope.fromYearOptions.SelectedYear = fromYear;
            $scope.toYearOptions = {
                CurrentDate: fromDate,
                InitDate: currentDate,
                MaxDate: fromDate,
                MinDate: fromDate,
                IsRerenderPicker: true,
                OnChangeEvent: $scope.updateToYear,
                SelectedYear: fromYear
            };

            $scope.searchCriteria.FromDate = fromDate;
            if (!isAlreadyApply) {
                $scope.toYearOptions.IsRerenderPicker = true;
                $scope.$apply();
            }
        };
        $scope.updateToYear = function (toYear, isAlreadyApply) {
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
            $scope.searchCriteria.Product = { Id: product.Id };
            $scope.subProductOptions.DataList = product.ChildSettings || [];

            if (!SettingManager.hasSetting(0, $scope.subProductOptions.DataList)) {
                var itemAll = { Id: 0, Value: viewResources.All };
                $scope.subProductOptions.DataList.splice(0, 0, itemAll);
            }

            $scope.updateSelectedSubProduct($scope.subProductOptions.DataList[0]);
        };
        $scope.updateSelectedSubProduct = function (subProduct) {          
            $scope.searchCriteria.SubProduct = { Id: subProduct.Id };
            if (!$scope.$$phase) {
                $scope.$apply();
            }
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
                SubProductId: $scope.searchCriteria.SubProduct.Id,
                SiteId: $scope.selectedSite ? $scope.selectedSite.Id : crmConstants.Enums.Site.NotApply,
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
                $scope.dynamicReportOptions.DataList = data.Result.Logs;
                $scope.dynamicReportOptions.mainCategory = data.Result.MainCategories;
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
                $scope.dynamicReportOptions.DataList = data.Result.Logs;
                $scope.dynamicReportOptions.mainCategory = data.Result.MainCategories;
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
            registerOptions($scope);
            $scope.initController();
        });
    } ]);
})();