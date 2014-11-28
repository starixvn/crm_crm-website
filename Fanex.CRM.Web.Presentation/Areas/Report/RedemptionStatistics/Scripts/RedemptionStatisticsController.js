"use strict";
/* global redemptionStatisticsApplication,
hotKeyManager,
viewResources,
crmConstants,
currentDate: true,
initFromDate: true,
weekPickerManager,
moment,
sharedScriptManager */
/* jshint camelcase: false */
/* jshint unused: vars */
/* jshint maxstatements: 412 */
redemptionStatisticsApplication.controller('redemptionStatisticsController', ['$scope', '$http', '$redemptionStatisticsService',
function (
    $scope,
    $http,
    $redemptionStatisticsService) {

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

    $scope.selectedStatusIds = '';

    $scope.searchCriteria = {
        FromDate: new Date(initFromDate),
        ToDate: new Date(currentDate),

        Subject: '',
        Status: '',
        Currency: '',
        ReportBy: '',
        PageIndex: 0,
        PageSize: sharedScriptManager.cachedData.defaultItemsPerPage,
        IsGetFromCached: false
    };

    $scope.searchDetailCriteria = {
        Month: 0,
        Year: 0,
        Subject: null,
        Currency: null,
        ReportBy: null,
        PageIndex: 0,
        PageSize: sharedScriptManager.cachedData.defaultItemsPerPage,
        IsGetFromCached: false
    };
    $scope.redemptionStatisticsOptions = {
        SummaryDataList: null,
        DetailDataList: null
    };
    $scope.lastSelectedParams = null;

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
            NoneSelectedText: viewResources.No_status_found,
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
        function (data) {
            $scope.isLoadedAllSetting = true;
            $scope.completedLoadFilterInfo();
        });
    };

    $scope.completedLoadFilterInfo = function () {
        $scope.searchCriteria.Currency = $scope.currencyOptions.DataList[0];
        $scope.searchCriteria.ReportBy = $scope.reportByOptions.DataList[0];

        $scope.unblockUI();
    };

    $scope.updateSelectedStatus = function (statusIdList) {
        $scope.selectedStatusIds = statusIdList;
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
        $scope.searchCriteria.Currency = currency;
        $scope.$apply();
    };

    $scope.updateSelectedReportBy = function (reportBy) {
        $scope.searchCriteria.ReportBy = reportBy;
        $scope.$apply();
    };

    $scope.getRedemptionStatisticsReport = function () {
        $scope.BlockUI();
        var getReportCriteria = {
            FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
            ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),

            StatusId: $scope.selectedStatusIds,
            CurrencyId: $scope.searchCriteria.Currency.Id,
            ReportBy: $scope.searchCriteria.ReportBy.Id,
            Subject: $scope.searchCriteria.Subject,
            PageIndex: $scope.searchCriteria.PageIndex,
            PageSize: $scope.searchCriteria.PageSize,
            IsGetFromCached: false
        };

        $scope.lastSelectedParams = getReportCriteria;
        $redemptionStatisticsService.getRedemptionStatisticsReport(getReportCriteria)
        .then(function (data) {
            $scope.redemptionStatisticsOptions.SummaryDataList = data.Result;
            $scope.isSubmitClick = true;

            $scope.isShowSummary = true;

            $scope.unblockUI();
        },
        function (data) {
            $scope.unblockUI();
        });
    };

    $scope.getRedemptionStatisticsDetailReport = function (month, year, subject, currency) {
        $scope.BlockUI();
        var getReportDetailCriteria = {
            Month: month,
            Year: year,
            StatusId: $scope.selectedStatusIds,
            CurrencyId: currency,
            ReportBy: $scope.searchCriteria.ReportBy.Id,
            Subject: subject,
            PageIndex: $scope.searchCriteria.PageIndex,
            PageSize: $scope.searchCriteria.PageSize,
            IsGetFromCached: false
        };

        $redemptionStatisticsService.getRedemptionStatisticsDetailReport(getReportDetailCriteria)
        .then(function (data) {
            $scope.redemptionStatisticsOptions.DetailDataList = data.Result;

            $scope.isShowSummary = false;

            $scope.unblockUI();
        },
        function (data) {
            $scope.unblockUI();
        });
    };

    $scope.viewBack = function () {
        $scope.isShowSummary = true;
        $scope.$apply();
    };

    $scope.exportSummaryReport = function () {
        $scope.downloadFileOptions = {
            URL: window.rootUrl + "RedemptionStatisticsReport/ExportReport",
            Params: $scope.lastSelectedParams
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