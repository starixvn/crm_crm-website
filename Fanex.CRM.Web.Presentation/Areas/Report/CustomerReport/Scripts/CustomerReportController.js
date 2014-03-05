"use strict";
/* global customerReportApplication,
hotKeyManager,
viewResources,
crmConstants,
currentDate: true,
initFromDate: true,
moment,
sharedScriptManager */
/* jshint camelcase: false */
/* jshint unused: vars */
/* jshint maxstatements: 412 */
customerReportApplication.controller('customerReportController', ['$scope', '$http', '$customerReportService',
    function (
        $scope,
        $http,
        $customerReportService) {
        $scope.fromDateOptions = null;
        $scope.toDateOptions = null;
        $scope.siteOptions = null;
        $scope.titleOptions = null;
        $scope.customerTypeOptions = null;
        $scope.languageOptions = null;
        $scope.countryOptions = null;
        $scope.defaultFocusOptions = null;
        $scope.downloadFileOptions = null;
        $scope.vipTypeOptions = null;
        $scope.customerReport = null;

        $scope.isUpdatingDate = false;
        $scope.searchCriteria = {
            FromDate: initFromDate,
            ToDate: new Date(currentDate),
            FirstName: '',
            LastName: '',

            CountryIds: '',
            LanguageIds: '',
            SiteIds: '',
            Title: null,
            CustomerType: null,
            VIPType: null,
            PageIndex: 0,
            PageSize: sharedScriptManager.cachedData.defaultItemsPerPage,
            SortField: '',
            SortDirection: ''
        };
        $scope.lastSelectedParams = angular.copy($scope.searchCriteria);

        $scope.initController = function () {
            $scope.getAllSetting();
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "Report_CustomerReport"
            });

            $scope.initPicker();

            $scope.defaultFocusOptions = {
                IsSetFocus: false,
                OnChangeEvent: $scope.completeSetFocus
            };

            $scope.defaultFocusOptions.IsSetFocus = true;
            $scope.$apply();
        };
        $scope.initPicker = function () {
            $scope.fromDateOptions = {
                CurrentDate: currentDate,
                InitDate: initFromDate,
                MaxDate: currentDate,
                IsRerenderPicker: false,
                OnChangeDateEvent: $scope.updateFromDate,
                IsAllowNullDate: true
            };
            $scope.updateFromDatePicker();

            $scope.toDateOptions = {
                CurrentDate: currentDate,
                InitDate: currentDate,
                MaxDate: currentDate,
                IsRerenderPicker: false,
                OnChangeDateEvent: $scope.updateToDate,
                IsAllowNullDate: true
            };
            $scope.updateToDatePicker();

            $scope.siteOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedSite,
                AllText: viewResources.All
            };

            $scope.titleOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedTitle,
                AllText: viewResources.All
            };

            $scope.customerTypeOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedCustomerType,
                AllText: viewResources.All
            };

            $scope.vipTypeOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedVIPType,
                AllText: viewResources.All
            };

            $scope.languageOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedLanguage,
                AllText: viewResources.All_Languages,
                MinWidth: 250,
                MaxHeight: 450,
                NoItemText: viewResources.No_languages_found,
                NoneSelectedText: viewResources.Symbol_NotApply,
                NumberOfItemShowShortText: 3
            };

            $scope.siteOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedSite,
                AllText: viewResources.All_Sites,
                MinWidth: 250,
                MaxHeight: 450,
                NoItemText: viewResources.No_site_found,
                NoneSelectedText: viewResources.Symbol_NotApply,
                NumberOfItemShowShortText: 0,

                SubOptionsName: 'ChildSites',
                NumberOfSelectedLevel: 2
            };

            $scope.countryOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedCountry,
                AllText: viewResources.All_Countries,
                MinWidth: 250,
                MaxHeight: 450,
                NoItemText: viewResources.No_country_found,
                NoneSelectedText: viewResources.Symbol_NotApply,
                NumberOfItemShowShortText: 3
            };
        };
        $scope.updateFromDatePicker = function (isOnApply) {
            $scope.fromDateOptions.IsRerenderPicker = true;
            if (!isOnApply) {
                $scope.$apply();
            }
        };
        $scope.updateToDatePicker = function (isOnApply) {
            $scope.toDateOptions.IsRerenderPicker = true;
            if (!isOnApply) {
                $scope.$apply();
            }
        };
        $scope.getAllSetting = function () {
            $scope.BlockUI();
            $scope.isLoadedAllSite = false;
            $customerReportService.getAllSetting()
                .then(function (data) {
                    var allSetting = data.Result;
                    $scope.siteOptions.DataList = allSetting.SiteList;
                    $scope.titleOptions.DataList = allSetting.TitleList;
                    $scope.customerTypeOptions.DataList = allSetting.CustomerTypeList;
                    $scope.vipTypeOptions.DataList = allSetting.VIPTypeList;
                    $scope.languageOptions.DataList = allSetting.LanguageList;
                    $scope.countryOptions.DataList = allSetting.CountryList;
                    $scope.completedLoadFilterInfo();
                },
                function (data) {
                    $scope.completedLoadFilterInfo();
                });
        };
        $scope.completedLoadFilterInfo = function () {
            $scope.searchCriteria.Title = $scope.titleOptions.DataList[0];
            $scope.searchCriteria.CustomerType = $scope.customerTypeOptions.DataList[0];
            $scope.searchCriteria.VIPType = $scope.vipTypeOptions.DataList[0];
            $scope.unblockUI();
        };
        $scope.updateSelectedCountry = function (countryIdList) {
            $scope.searchCriteria.CountryIds = countryIdList;
        };
        $scope.updateSelectedLanguage = function (languageIdList) {
            $scope.searchCriteria.LanguageIds = languageIdList;
        };
        $scope.updateSelectedSite = function (siteIdList) {
            $scope.searchCriteria.SiteIds = siteIdList;
        };
        $scope.updateSelectedTitle = function (title) {
            $scope.searchCriteria.Title = title;
        };
        $scope.updateFromDate = function (selectedDate) {
            $scope.searchCriteria.FromDate = selectedDate;
            $scope.toDateOptions.MinDate = selectedDate && selectedDate !== "" ? selectedDate : null;
            $scope.updateToDatePicker(true);
        };
        $scope.updateToDate = function (selectedDate) {
            $scope.searchCriteria.ToDate = selectedDate;
            $scope.fromDateOptions.MaxDate = selectedDate && selectedDate !== "" ? selectedDate : null;
            $scope.updateFromDatePicker(true);
        };

        $scope.updateSelectedVIPType = function (vipType) {
            $scope.searchCriteria.VIPType = vipType;
        };
        $scope.invokeHotKey_CtrlS = function () {
            $scope.getCustomerReport();
        };
        $scope.getCustomerReport = function () {
            $scope.BlockUI();
            var getReportCriteria = {
                FromDateBirthDay: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDateBirthDay: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),

                FirstName: $scope.searchCriteria.FirstName,
                LastName: $scope.searchCriteria.LastName,

                CountryIds: $scope.searchCriteria.CountryIds,
                LanguageIds: $scope.searchCriteria.LanguageIds,
                SiteIds: $scope.searchCriteria.SiteIds,
                TitleId: $scope.searchCriteria.Title.Id,
                CustomerTypeId: $scope.searchCriteria.CustomerType.Id,
                VIPTypeId: $scope.searchCriteria.VIPType.Id,
                PageIndex: $scope.searchCriteria.PageIndex,
                PageSize: $scope.searchCriteria.PageSize,
                SortField: $scope.searchCriteria.SortField,
                SortDirection: $scope.searchCriteria.SortDirection
            };

            $scope.lastSelectedParams = getReportCriteria;
            $customerReportService.getCustomerReport(getReportCriteria)
                .then(function (data) {
                    $scope.customerReport = data.Result;
                    $scope.unblockUI();
                },
                function (data) {
                    $scope.unblockUI();
                });
        };
        $scope.pagingCustomerReport = function (pageIndex, pageSize) {
            $scope.BlockUI();
            $scope.lastSelectedParams.PageIndex = pageIndex;
            $scope.lastSelectedParams.PageSize = pageSize;
            $customerReportService.getCustomerReport($scope.lastSelectedParams)
                .then(function (data) {
                    $scope.customerReport = data.Result;
                    $scope.unblockUI();
                },
                function (data) {
                    $scope.unblockUI();
                });
        };
        $scope.sortCustomerReport = function () {
            $scope.BlockUI();
            $customerReportService.getCustomerReport($scope.lastSelectedParams)
                .then(function (data) {
                    $scope.customerReport = data.Result;
                    $scope.unblockUI();
                },
                function (data) {
                    $scope.unblockUI();
                });
        };
        $scope.exportReport = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "CustomerReport/ExportCustomerReport",
                Params: $scope.lastSelectedParams
            };

            $scope.$apply();
        };

        $scope.completeSetFocus = function () {
            $scope.defaultFocusOptions.IsSetFocus = false;
        };
        $scope.unblockUI = function () {
            try {
                parent.$.unblockUI();
            } catch (e) {
            }
        };
        $scope.BlockUI = function () {
            try {
                parent.BlockUI();
            } catch (e) {
            }
        };
        angular.element(document).ready(function () {
            $scope.initController();
        });
    } ]);