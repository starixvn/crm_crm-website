(function () {
    "use strict";
    /* global manageFeedbackResources,
    sharedScriptManager,
    moment,
    crmConstants,
    currentDate,
    yesterday,
    currentTime,
    username,
    createUpdateFeedbackPermission,
    isGuest */
    /* jshint camelcase: false */
    angular.module('feedback-manage-application').controller('manageFeedbackController',['$scope', 'manageFeedbackService', '$uiBlockerService', '$resultMessageService', function (
    $scope, manageFBService, $uiBlockerService, $resultMessageService) {
        $scope.isSetFocus = false;
        $scope.fromDateOptions = null;
        $scope.toDateOptions = null;
        $scope.subCategoryOptions = null;
        $scope.mainCategoryOptions = null;
        $scope.createUpdateFeedbackPermission = createUpdateFeedbackPermission;
        $scope.searchCriteria = {
            FeedbackId: '',
            FromDate: yesterday,
            ToDate: currentTime,
            Username: username,
            IsGuest: null,
            IsReviewed: null,
            Response: null,
            MainCategory: null,
            SubCategory: null,
            Communication: null,
            Status: null,
            Handler: null,
            PageIndex: 0,
            ItemsPerPage: sharedScriptManager.cachedData.largeDefaultItemsPerPage
        };
        $scope.downloadFileOptions = null;
        $scope.isCheckedAll = false;

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

        $scope.getAllSetting = function () {
            manageFBService.getAllSetting()
            .then(function (data) {
                var settings = data.Result;
                $scope.customerTypes = settings.CustomerType;
                $scope.reviews = settings.Review;
                $scope.responses = settings.CustomerResponse;
                $scope.communications = settings.Communication;
                $scope.statusList = settings.Status;

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
                $scope.searchCriteria.Status = $scope.statusList[0];
                $scope.searchCriteria.MainCategory = $scope.mainCategoryOptions.DataList[0];
                $scope.searchCriteria.SubCategory = $scope.subCategoryOptions.DataList[0];

                if (username && username.length !== 0) {
                    $scope.searchCriteria.IsGuest = $scope.customerTypes[isGuest + 1];
                }
                else {
                    $scope.searchCriteria.IsGuest = $scope.customerTypes[0];
                }

                $uiBlockerService.unblockUI();
            },
            function () {
                $uiBlockerService.unblockUI();
            });
        };

        $scope.filterManageFeedback = function () {
            $scope.searchCriteria.PageIndex = 0; // always reset PageIndex when submit form.
            $scope.searchManageFeedback();
        };

        $scope.searchManageFeedback = function () {
            $uiBlockerService.blockUI();
            $scope.isCheckAll = false;
            var criteria = {
                FeedbackId: $scope.searchCriteria.FeedbackId,
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                Username: $scope.searchCriteria.Username,
                IsGuest: $scope.searchCriteria.IsGuest.Id,
                CommunicationId: $scope.searchCriteria.Communication.Id,
                MainCategoryId: $scope.searchCriteria.MainCategory.Id,
                SubCategoryId: $scope.searchCriteria.SubCategory.Id,
                StatusId: $scope.searchCriteria.Status.Id,
                IsReviewed: $scope.searchCriteria.IsReviewed.Id,
                ResponseId: $scope.searchCriteria.Response.Id,
                HandledBy: $scope.searchCriteria.Handler,
                PageIndex: $scope.searchCriteria.PageIndex + 1,
                ItemsPerPage: $scope.searchCriteria.ItemsPerPage
            };
            manageFBService.searchManageFeedback(criteria)
            .then(function (data) {
                $scope.report = data.Result;
                $uiBlockerService.unblockUI();
            },
            function () {
                $uiBlockerService.unblockUI();
            });
        };
        $scope.openEditFeedbackPopup = function (id) {
            var url = window.rootUrl + 'UpdateCustomerFeedback/Index?id=' + id;
            var maxWidthOfPopUp = 987;
            var maxHeightOfPopUp = $(parent.document).height();
            parent.OpenPopup(url, manageFeedbackResources.UpdateFeedbackTitle, maxWidthOfPopUp, maxHeightOfPopUp, true, false, window, 2000, null, function () {
                $scope.searchManageFeedback();
            });
        };

        $scope.closeEditView = function () {
            var popUpResult = parent.homeManager.CachedData.shareInfoBetweenPopup;
            if (popUpResult && popUpResult.Code === crmConstants.Enums.Error.None) {
                $scope.searchManageFeedback();
            }
        };

        $scope.openEditMultipleLogs = function () {
            var selectedFeedbacks = $scope.getSelectedFeedbacks();
            if(selectedFeedbacks.length === 0) {
                return;
            }

            parent.homeManager.CachedData.feedbacks = selectedFeedbacks;
            parent.homeManager.CachedData.shouldReloadView = false;
            var url = window.rootUrl + 'MultipleLogs/Edit?feedbackid=' + parent.homeManager.CachedData.feedbacks[0].FeedbackId;
            var maxWidthOfPopUp = 987;
            var maxHeightOfPopUp = $(parent.document).height() - 30;

            parent.OpenPopup(url, manageFeedbackResources.EditMultipleLogsTitle, maxWidthOfPopUp, maxHeightOfPopUp, true, false, parent.window, 2000, null, $scope.closeEditView);
        };

        $scope.updateSiteLicenseeType = function () {
            var selectedFeedbacks = $scope.getSelectedFeedbacks();
            if (selectedFeedbacks.length === 0) {
                return;
            }

            var criteria = {
                ListFeedbackID: selectedFeedbacks.map(function (e) { return e.FeedbackId; }).join(",")
            };

            $uiBlockerService.blockUI();
            manageFBService.updateSiteLicenseeType(criteria)
              .then(
                function (data) {
                  $resultMessageService.showMessage({
                      messageType: crmConstants.Enums.MessageType.Success,
                      message: data.Result.Message
                  });                  
                  $scope.searchManageFeedback();
                },
                function (data) {
                    $uiBlockerService.unblockUI();
                });
        };

        $scope.checkActionItem = function (id) {
            var isCheckedAll = true;
            angular.forEach($scope.report.Feedbacks, function (feedback) {
                if (feedback.FeedbackId === id) {
                    feedback.IsChecked = !feedback.IsChecked;
                }

                if (!feedback.IsChecked) {
                    isCheckedAll = false;
                }
            });

            $scope.isCheckedAll = isCheckedAll;
            $scope.$apply();
        };

        $scope.checkActionAll = function () {
            $scope.isCheckedAll = !$scope.isCheckedAll;
            angular.forEach($scope.report.Feedbacks, function (feedback) {
                feedback.IsChecked = $scope.isCheckedAll;
            });
            $scope.$apply();
        };

        $scope.getCheckAllStatus = function () {
            return $scope.isCheckedAll;
        };

        $scope.getSelectedFeedbacks = function() {
            var selectedFeedbacks = [];
            angular.forEach($scope.report.Feedbacks, function (feedback) {
                if(feedback.IsChecked) {
                    this.push({ FeedbackId: feedback.FeedbackId, UserName: feedback.Username });
                }
            }, selectedFeedbacks);

            return selectedFeedbacks;
        };

        $scope.exportFeedbackExcel = function () {
            var criteria = {
                FeedbackId: $scope.searchCriteria.FeedbackId,
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                Username: $scope.searchCriteria.Username,
                IsGuest: $scope.searchCriteria.IsGuest.Id,
                CommunicationId: $scope.searchCriteria.Communication.Id,
                MainCategoryId: $scope.searchCriteria.MainCategory.Id,
                SubCategoryId: $scope.searchCriteria.SubCategory.Id,
                StatusId: $scope.searchCriteria.Status.Id,
                IsReviewed: $scope.searchCriteria.IsReviewed.Id,
                ResponseId: $scope.searchCriteria.Response.Id,
                HandledBy: $scope.searchCriteria.Handler,
                PageIndex: $scope.searchCriteria.PageIndex + 1,
                ItemsPerPage: $scope.searchCriteria.ItemsPerPage
            };
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "Feedbacks/ExportManageFeedbacks",
                Params: criteria
            };

            $scope.$apply();
        };

        angular.element(document).ready(function () {
            $uiBlockerService.blockUI();

            $scope.initDatePicker();
            $scope.getAllSetting();
        });
    } ]);
})();