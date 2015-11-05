(function () {
    "use strict";
    /* global deletedFeedbackResources,
    sharedScriptManager,
    moment,
    crmConstants,
    currentDate,
    yesterday,
    currentTime */
    /* jshint camelcase: false */
    /* jshint unused: vars */
    angular.module('deleted-feedback-application').controller('deletedFeedbackController', ['$scope', 'deletedFeedbackService', '$uiBlockerService', function (
    $scope, deletedFBService, $uiBlockerService) {
        $scope.fromDateOptions = null;
        $scope.toDateOptions = null;
        $scope.subCategoryOptions = null;
        $scope.mainCategoryOptions = null;
        $scope.searchCriteria = {
            FromDate: yesterday,
            ToDate: currentTime,
            Username: '',
            IsGuest: null,
            IsReviewed: null,
            Response: null,
            MainCategory: null,
            SubCategory: null,
            Communication: null,
            PageIndex: 0,
            ItemsPerPage: sharedScriptManager.cachedData.largeDefaultItemsPerPage
        };

        $scope.initDatePicker = function () {
            $scope.fromDateOptions = {
                CurrentDate: currentDate,
                InitDate: yesterday,
                MaxDate: currentDate,
                IsRerenderPicker: false,
                OnChangeDateEvent: $scope.updateFromDate
            };

            $scope.toDateOptions = {
                CurrentDate: currentDate,
                InitDate: currentDate,
                MaxDate: currentDate,
                IsRerenderPicker: false,
                OnChangeDateEvent: $scope.updateToDate
            };

            $scope.updateFromDatePicker();
            $scope.updateToDatePicker();
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

        $scope.updateFromDate = function (selectedDate) {
            $scope.searchCriteria.FromDate = selectedDate;
            $scope.toDateOptions.MinDate = selectedDate;
            $scope.updateToDatePicker(true);
        };

        $scope.updateToDate = function (selectedDate) {
            $scope.searchCriteria.ToDate = selectedDate;
            $scope.fromDateOptions.MaxDate = selectedDate;
            $scope.updateFromDatePicker(true);
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

    $scope.updateFromDate = function (selectedDate) {
        $scope.searchCriteria.FromDate = selectedDate;
        $scope.toDateOptions.MinDate = selectedDate;
        $scope.updateToDatePicker(true);
    };

    $scope.updateToDate = function (selectedDate) {
        $scope.searchCriteria.ToDate = selectedDate;
        $scope.fromDateOptions.MaxDate = selectedDate;
        $scope.updateFromDatePicker(true);
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

    $scope.getAllSetting = function () {
        deletedFBService.getAllSetting()
            .then(function (data) {
                var settings = data.Result;
                $scope.customerTypes = settings.CustomerType;
                $scope.reviews = settings.Review;
                $scope.responses = settings.CustomerResponse;
                $scope.communications = settings.Communication;

                $scope.mainCategoryOptions = {
                    DataList: [],
                    OnChangeEvent: $scope.updateSelectedMainCategory
                };
                $scope.subCategoryOptions = {
                    DataList: [],
                    OnChangeEvent: $scope.updateSelectedSubCategory
                };

                $scope.mainCategoryOptions.DataList = settings.MainCategory;
                $scope.subCategoryOptions.DataList = $scope.mainCategoryOptions.DataList[0].ChildSettings;

                // Set default item of <select>.
                $scope.searchCriteria.IsReviewed = $scope.reviews[0];
                $scope.searchCriteria.Response = $scope.responses[0];
                $scope.searchCriteria.Communication = $scope.communications[0];
                $scope.searchCriteria.MainCategory = $scope.mainCategoryOptions.DataList[0];
                $scope.searchCriteria.SubCategory = $scope.subCategoryOptions.DataList[0];
                $scope.searchCriteria.IsGuest = $scope.customerTypes[0];

                $uiBlockerService.unblockUI();
            },
            function () {
                $uiBlockerService.unblockUI();
            });
        };

        $scope.filterManageFeedback = function () {
            $scope.searchCriteria.PageIndex = 0; // always reset PageIndex when submit form.
            $scope.searchDeletedFeedback();
        };

        $scope.searchDeletedFeedback = function () {
            $uiBlockerService.blockUI();
            var criteria = {
                FeedbackId: $scope.searchCriteria.FeedbackId,
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                Username: $scope.searchCriteria.Username,
                IsGuest: $scope.searchCriteria.IsGuest.Id,
                CommunicationId: $scope.searchCriteria.Communication.Id,
                MainCategoryId: $scope.searchCriteria.MainCategory.Id,
                SubCategoryId: $scope.searchCriteria.SubCategory.Id,
                IsReviewed: $scope.searchCriteria.IsReviewed.Id,
                ResponseId: $scope.searchCriteria.Response.Id,
                HandledBy: $scope.searchCriteria.Handler,
                PageIndex: $scope.searchCriteria.PageIndex + 1,
                PageSize: $scope.searchCriteria.ItemsPerPage
            };
            criteria.FromCache = criteria.PageIndex !== 1;

            deletedFBService.searchDeletedFeedback(criteria)
            .then(function (data) {
                $scope.report = data.Result;
                $uiBlockerService.unblockUI();
            },
            function () {
                $uiBlockerService.unblockUI();
            });
        };

    $scope.openEditFeedbackPopup = function (id) {
        var url = window.rootUrl + 'UpdateCustomerFeedback/Index?id=' + id + '&isDeletedFeedbackView=true';
        var maxWidthOfPopUp = 987;
        var maxHeightOfPopUp = $(parent.document).height();
        parent.OpenPopup(url, deletedFeedbackResources.UpdateFeedbackTitle, maxWidthOfPopUp, maxHeightOfPopUp, true, false, window, 2000, null, function () {
            $scope.searchDeletedFeedback();
        });
    };

        angular.element(document).ready(function () {
            $uiBlockerService.blockUI();

            $scope.initDatePicker();
            $scope.getAllSetting();
        });
    } ]);
})();