(function () {
    "use strict";
    /* global hotKeyManager,
    viewResources,
    crmConstants,
    currentDate: true,
    initFromDate: true,
    moment,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('report-redemption-statistics-application')
        .controller('redemptionStatisticsController', ['$scope', '$redemptionStatisticsService', 'alertBoxService',
    function ($scope, $redemptionStatisticsService, alertBoxService) {
        $scope.isSetFocus = false;
        $scope.isShowFilter = true;
        $scope.fromDateOptions = null;
        $scope.toDateOptions = null;
        $scope.currencyOptions = null;
        $scope.reportByOptions = null;
        $scope.statusOptions = {};
        $scope.defaultFocusOptions = null;
        $scope.downloadFileOptions = null;
        $scope.isLoadedAllSetting = false;
        $scope.isSubmitClick = false;

        $scope.isShowSummary = true;
        $scope.title = '';

        $scope.searchCriteria = {
            FromDate: new Date(initFromDate),
            ToDate: new Date(currentDate),
            Subject: '',
            Status: '',
            Currency: '',
            ReportBy: '',
            PageIndex: 0,
            PageSize: sharedScriptManager.cachedData.defaultItemsPerPage
        };

        $scope.searchDetailCriteria = {
            Month: 0,
            Year: 0,
            Subject: null,
            Currency: null,
            ReportBy: null,
            PageIndex: 0,
            PageSize: sharedScriptManager.cachedData.defaultItemsPerPage
        };
        $scope.redemptionStatisticsOptions = {
            SummaryDataList: null,
            DetailDataList: null
        };
        $scope.lastSelectedParams = null;
        $scope.lastSelectedDetailParams = null;

        $scope.initController = function () {
            $scope.getAllSetting();

            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "Report_RedemptionStatistics"
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
            $scope.getRedemptionStatisticsReport();
        };

        $scope.initPicker = function () {
            currentDate = new Date(currentDate);
            initFromDate = new Date(initFromDate);

            $scope.currencyOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedCurrency
            };

            $scope.reportByOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedReportBy
            };

            $scope.statusOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedStatus,
                AllText: viewResources.All,
                MaxHeight: 450,
                NoItemText: viewResources.Symbol_NotApply,
                NoneSelectedText: viewResources.Symbol_NotApply,
                NumberOfItemShowShortText: 0
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

            $redemptionStatisticsService.getAllSetting()
        .then(function (data) {
            var allSetting = data.Result;
            $scope.currencyOptions.DataList = allSetting.CurrencyList;
            $scope.reportByOptions.DataList = allSetting.ReportByList;
            $scope.statusOptions.DataList = allSetting.StatusList;
            $scope.completedLoadFilterInfo();
        },
        function () {
            $scope.isLoadedAllSetting = true;
            $scope.completedLoadFilterInfo();
        });
        };

        $scope.completedLoadFilterInfo = function () {
            $scope.searchCriteria.Currency = $scope.currencyOptions.DataList[0];
            $scope.searchCriteria.ReportBy = $scope.reportByOptions.DataList[0];
            $scope.updateSelectedCurrency($scope.currencyOptions.DataList[0]);

            $scope.unblockUI();
        };

        $scope.updateSelectedStatus = function (statusIdList) {
            $scope.searchCriteria.Status = statusIdList;
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

        $scope.updateSelectedCurrency = function (currency) {
            if (currency.Id === 0) { // all
                var currencyIds = [];
                angular.forEach($scope.currencyOptions.DataList, function (value, key) {
                    /* jshint unused: false */
                    if (value.Id !== 0) { this.push(value.Id); }
                }, currencyIds);
                $scope.searchCriteria.Currency = currencyIds.join();
            }
            else {
                $scope.searchCriteria.Currency = currency.Id;
            }
        };

        $scope.updateSelectedReportBy = function (reportBy) {
            $scope.searchCriteria.ReportBy = reportBy;
            $scope.$apply();
        };

        $scope.getRedemptionStatisticsReport = function () {
            alertBoxService.hide();
            $scope.BlockUI();
            var getReportCriteria = {
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                StatusId: $scope.searchCriteria.Status,
                CurrencyId: $scope.searchCriteria.Currency,
                ReportBy: $scope.searchCriteria.ReportBy.Id,
                Subject: $scope.searchCriteria.Subject
            };

            if (!getReportCriteria.StatusId) {
                alertBoxService.show({
                    type: crmConstants.Enums.MessageType.Error,
                    message: viewResources.No_status_found
                });
                $scope.unblockUI();
                return false;
            }

            $scope.lastSelectedParams = getReportCriteria;
            $redemptionStatisticsService.getRedemptionStatisticsReport(getReportCriteria)
        .then(function (data) {
            $scope.redemptionStatisticsOptions.SummaryDataList = data.Result;
            $scope.isSubmitClick = true;
            $scope.isShowSummary = true;

            $scope.unblockUI();
        },
        function () {
            $scope.unblockUI();
        });
        };

        $scope.getRedemptionStatisticsDetailReport = function (title, currency, year, month) {
            $scope.BlockUI();
            $scope.title = title;

            var fromdate = '', todate = '';
            if (year !== 0 && month !== 0) {
                fromdate = moment(new Date(year, month - 1, 1)).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment);
                todate = moment(new Date(year, month, 0)).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment);
            }
            else if (year !== 0 && month === 0) {
                fromdate = 'Jan 01, ' + year;
                todate = 'Dec 31, ' + year;
            }
            else {
                fromdate = moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment);
                todate = moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment);
            }

            if (currency === 0) {
                currency = $scope.searchCriteria.Currency;
            }

            var getReportDetailCriteria = {
                FromDate: fromdate,
                ToDate: todate,
                StatusId: $scope.searchCriteria.Status,
                CurrencyId: currency,
                ReportBy: $scope.searchCriteria.ReportBy.Id,
                Subject: $scope.searchCriteria.Subject
            };

            $scope.lastSelectedDetailParams = getReportDetailCriteria;
            $redemptionStatisticsService.getRedemptionStatisticsDetailReport(getReportDetailCriteria)
        .then(function (data) {
            $scope.redemptionStatisticsOptions.DetailDataList = data.Result;
            $scope.isShowSummary = false;
            $scope.unblockUI();
        },
        function () {
            $scope.unblockUI();
        });
        };

        $scope.getRedemptionStatisticsDetailReportWithPaging = function (pageIndex, pageSize) {
            $scope.BlockUI();

            $scope.lastSelectedDetailParams.PageIndex = pageIndex;
            $scope.lastSelectedDetailParams.ItemsPerPage = pageSize;
            $redemptionStatisticsService.getRedemptionStatisticsDetailReport($scope.lastSelectedDetailParams)
        .then(function (data) {
            $scope.redemptionStatisticsOptions.DetailDataList = data.Result;
            $scope.isShowSummary = false;
            $scope.unblockUI();
        },
        function () {
            $scope.unblockUI();
        });
        };

        $scope.viewBack = function () {
            $scope.isShowSummary = true;
            $scope.$apply();
        };

        $scope.exportSummaryReport = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "RedemptionStatisticsReport/ExportSummaryReport",
                Params: $scope.lastSelectedParams
            };

            $scope.$apply();
        };

        $scope.exportDetailReport = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "RedemptionStatisticsReport/ExportDetailReport",
                Params: $scope.lastSelectedDetailParams
            };

            $scope.$apply();
        };

        $scope.showHideFilterBox = function () {
            $scope.isShowFilter = !$scope.isShowFilter;
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