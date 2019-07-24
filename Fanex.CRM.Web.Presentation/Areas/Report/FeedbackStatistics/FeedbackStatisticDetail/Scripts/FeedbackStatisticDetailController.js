(function () {
    "use strict";
    /* global sharedScriptManager,
    criteria */
    angular.module('feedback-statistic-detail-application')
        .controller('feedbackStatisticDetailController', ['$scope', 'FeedbackStatisticDetailService', '$uiBlockerService',
            function ($scope, feedbackStatisticDetailService, $uiBlockerService) {
                criteria.PageIndex = 0;
                criteria.PageSize = sharedScriptManager.cachedData.defaultItemsPerPage;
                $scope.searchCriteria = criteria;

                $scope.feedbackStatisticDetail = null;
                $scope.downloadFileOptions = null;

                $scope.initController = function () {
                    $scope.getFeedbackStatisticDetail();
                };

                $scope.getFeedbackStatisticDetail = function (page, size) {
                    $uiBlockerService.blockUI();
                    $scope.searchCriteria.PageIndex = page || 0;
                    $scope.searchCriteria.PageSize = size || sharedScriptManager.cachedData.defaultItemsPerPage;
                    $scope.searchCriteria.FromCache = $scope.searchCriteria.PageIndex !== 0;

                    feedbackStatisticDetailService.getFeedbackStatisticDetail($scope.searchCriteria)
                        .then(function (data) {
                            $scope.feedbackStatisticDetail = data.Result.Report;
                            $scope.mainCategory = data.Result.MainCategories;
                            $uiBlockerService.unblockUI();
                        },
                            function () {
                                $uiBlockerService.unblockUI();
                            });
                };

                $scope.exportFeedbackStatisticDetail = function () {
                    $scope.downloadFileOptions = {
                        URL: window.rootUrl + 'FeedbackStatisticReport/ExportFeedbackStatisticDetail',
                        Params: $scope.searchCriteria
                    };

                    $scope.$apply();
                };

                $scope.showRelatedCustomer = function (feedbackId) {
                    var index = $scope.feedbackStatisticDetail.Feedbacks.findIndex(feedback => feedback.FeedbackId === feedbackId);
                    var popupElement = parent.$('#div-resultpopup');
                    popupElement.removeClass('display-none').html($scope.feedbackStatisticDetail.Feedbacks[index].RelatedCustomer.split(',').join(', '));
                    popupElement.dialog({
                        width: 900,
                        minHeight: 300,
                        maxHeight: 500,
                        zIndex: 500,
                        position: { my: 'center', at: 'center' },
                        resizable: true,
                        modal: true,
                        title: feedbackStatisticDetailResources.RelatedCustomer,
                        close: function (event, ui) {
                            popupElement.dialog('destroy');
                            popupElement.addClass('display-none').html('');
                        },
                        buttons: [{
                            text: feedbackStatisticDetailResources.Closed,
                            click: function () {
                                popupElement.dialog('close');
                            }
                        }]
                    });
                };

                angular.element(document).ready(function () {
                    $scope.initController();
                });
            }]);
})();