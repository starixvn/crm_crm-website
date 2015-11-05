(function () {
    "use strict";
    /* global feedbackStatisticDetailResources,
    sharedScriptManager */
    /* jshint camelcase: false */
    /* jshint unused: vars */
    angular.module('feedback-statistic-detail-application').directive('tableFeedbackStatisticDetailContainer', function () {
        var isShowSubCategory;
        var gridOptions, tableFeedbackStatisticDetail;

        function getGridOptions(scope, reportTitle, bodyData) {
            function ReRenderAfterPaged(page_index, itemsPerPage) {
                gridOptions.pagerOption.currentPage = page_index;
                gridOptions.pagerOption.itemsPerPage = itemsPerPage;
                tableFeedbackStatisticDetail.fanexGrid(gridOptions);

                scope.getFeedbackStatisticDetail(page_index, itemsPerPage);
                return false;
            }

            var columns = [
            { HeaderText: feedbackStatisticDetailResources.Feedback, Width: 380, Name: 'ColFeedback', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: feedbackStatisticDetailResources.Customer, Width: 130, Name: 'ColCustomer', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: feedbackStatisticDetailResources.Category, Width: 270, Name: 'ColMainCategory', HeaderAlign: 'center', CellAlign: 'left'}];
            if (isShowSubCategory) {
                columns.push({
                    HeaderText: feedbackStatisticDetailResources.SubCategory,
                    Width: 270, Name: 'ColSubCategory',
                    HeaderAlign: 'center',
                    CellAlign: 'left'
                });
            }

            columns.push({ HeaderText: feedbackStatisticDetailResources.Status, Width: 50, Name: 'ColStatus', HeaderAlign: 'center', CellAlign: 'center' });
            columns.push({
                HeaderText: feedbackStatisticDetailResources.RelatedCustomer,
                Width: 130, Name: 'ColRelatedCustomer',
                HeaderAlign: 'center',
                CellAlign: 'left'
            });
            return {
                columns: columns,
                showTitle: true,
                titleInfo: {
                    text: reportTitle,
                    showExcelButton: true,
                    exportExcelFunction: function () {
                        scope.exportFeedbackStatisticDetail();
                    }
                },
                bodyRows: bodyData,
                showFooter: false,
                fullWidth: true,
                fullHeight: true,
                gridExpandHeight: 35,
                showDistinctRow: false,
                showHighlightRow: false,
                numberOfFrozenColumn: 1,
                headerColumnHeight: 29,
                autoFixFrozenRowHeight: true,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: scope.searchCriteria.PageSize,
                    currentPage: scope.searchCriteria.PageIndex,
                    itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                    totalItem: scope.feedbackStatisticDetail.TotalItems,
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };
        }

        function getGridBody(scope, feedbacks) {
            var bodyData = [];
            if (feedbacks) {
                var feedback, status, currentRow, cssClass;
                for (var i = 0, fbLength = feedbacks.length; i < fbLength; i++) {
                    feedback = feedbacks[i];

                    status = feedback.Status ? feedback.Status.toLowerCase() : '';
                    cssClass = (status !== 'pending' && status !== 'closed') ? '' : status;
                    currentRow = [
                        { Name: 'ColFeedback', Value: renderFeedbackColumn(feedback) },
                        { Name: 'ColCustomer', Value: feedback.Username, CssClass: 'word-wrap'}];
                    if (isShowSubCategory) {
                        currentRow.push({ Name: 'ColMainCategory', Value: feedback.MainCategory });
                        currentRow.push({ Name: 'ColSubCategory', Value: feedback.SubCategory });
                    }
                    else {
                        currentRow.push({ Name: 'ColMainCategory', Value: renderCategoryColumn(feedback.MainCategory, feedback.SubCategory) });
                    }

                    currentRow.push({ Name: 'ColStatus', Value: feedback.Status });
                    currentRow.push({ Name: 'ColRelatedCustomer', 
                        Value: feedback.IsRelated ? feedback.Username : feedback.RelatedCustomer, CssClass: 'word-wrap' });

                    bodyData.push({ Columns: currentRow, CssClass: cssClass });
                }
            }

            return bodyData;
        }

        function renderFeedbackColumn(feedback) {
            var html = '<div style="margin:2px 0 0 2px;"> \
            <div class="lnksubject">\
                <a class="desTooltip" href="javascript:;">\
                    <span class="b">' + sharedScriptManager.encodeHtml(feedback.Subject) + '</span>\
                </a>\
            </div>\
            <div class="description">\
                <div>Feedback date <span class="b">' + feedback.FeedbackDateString + '</span></div>\
                <div>By <span class="b">' + feedback.CreatorName + '</span> on ' + feedback.CreateDateString + '</div>\
                <div>Solving Duration: ' + feedback.DurationTime + '</div>';

            var reviewer = (feedback.ReviewerName && feedback.ReviewerName.length > 0) ?
        '<div>Reviewed by <span class="b">' + feedback.ReviewerName + '</span> on ' + feedback.SolveDateString + '</div>' :
        '';
            return html + reviewer + '</div></div>';
        }

        function renderCategoryColumn(main, sub) {
            return '<div class="main-category">' + main + '</div><span class="sub-category">' + sub + '</span></div>';
        }

        function renderFeedbackStatisticDetail(scope, element) {
            isShowSubCategory = scope.searchCriteria.MainCategoryId === 0 &&
            scope.searchCriteria.SubCategoryId === 0 &&
            scope.searchCriteria.ValueId === 0;
            var report = scope.feedbackStatisticDetail;
            var bodyData = getGridBody(scope, report.Feedbacks);

            if (!gridOptions) {
                gridOptions = getGridOptions(scope, report.Title, bodyData);
            }
            else {
                gridOptions.bodyRows = bodyData;
            }

            if (!tableFeedbackStatisticDetail) {
                tableFeedbackStatisticDetail = element.find('#table-feedback-statistic-detail');
            }

            tableFeedbackStatisticDetail.empty().removeClass('hide').fanexGrid(gridOptions);
        }

        return {
            restrict: 'A',
            link: function (scope, element) {
                scope.$watch('feedbackStatisticDetail', function (newValue, oldValue) {
                    if (!newValue) {
                        return;
                    }

                    renderFeedbackStatisticDetail(scope, element);
                });
            }
        };
    });
})();