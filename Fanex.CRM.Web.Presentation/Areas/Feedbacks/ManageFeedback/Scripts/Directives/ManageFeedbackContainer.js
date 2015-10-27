(function () {
    "use strict";
    /* global manageFeedbackResources,
    crmConstants,
    sharedScriptManager */
    /* jshint camelcase: false */
    /* jshint unused: vars */
    angular.module('feedback-manage-application').directive('tableManageFeedbackContainer', function () {
        var gridOptions, tableManageFeedback;
        function getGridOptions(scope, bodyData, totalItems) {
            function ReRenderAfterPaged(page_index, itemsPerPage, jq) {
                gridOptions.pagerOption.currentPage = page_index;
                gridOptions.pagerOption.itemsPerPage = itemsPerPage;
                tableManageFeedback.fanexGrid(gridOptions);

                scope.searchCriteria.PageIndex = page_index;
                scope.searchCriteria.ItemsPerPage = itemsPerPage;
                scope.searchManageFeedback();
                return false;
            }

            return {
                columns: [
                    { HeaderText: manageFeedbackResources.Feedback, Width: 380, Name: 'ColFeedback', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: manageFeedbackResources.Customer, Width: 130, Name: 'ColCustomer', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: manageFeedbackResources.Category, Width: 270, Name: 'ColCategory', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: manageFeedbackResources.Status, Width: 50, Name: 'ColStatus', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: manageFeedbackResources.RelatedCustomer, Width: 130, Name: 'ColRelatedCustomer', HeaderAlign: 'center', CellAlign: 'left'}],
                showTitle: false,
                bodyRows: bodyData,
                showFooter: false,
                numberOfFrozenColumn: 1,
                autoFixFrozenRowHeight: true,
                fullWidth: true,
                fullHeight: true,
                gridExpandHeight: 190,
                showDistinctRow: false,
                showHighlightRow: false,
                headerColumnHeight: 29,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: scope.searchCriteria.ItemsPerPage,
                    currentPage: scope.searchCriteria.PageIndex,
                    itemsPerPageArray: sharedScriptManager.cachedData.largePagingOptions,
                    totalItem: totalItems,
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };
        }

        function getGridBody(feedbacks) {
            var fbLength = feedbacks.length;
            if (fbLength === 0) { return []; }

            var bodyData = [];
            var fb, currentRow, status, cssClass;
            for (var i = 0; i < fbLength; i++) {
                fb = feedbacks[i];

                currentRow = [
                { Name: 'ColFeedback', Value: renderFeedbackColumn(fb) },
                { Name: 'ColCustomer', Value: fb.Username, CssClass: 'word-wrap' },
                { Name: 'ColCategory', Value: renderCategoryColumn(fb.MainCategory, fb.SubCategory) },
                { Name: 'ColStatus', Value: fb.Status ? fb.Status : 'N/A' },
                { Name: 'ColRelatedCustomer', Value: fb.IsRelated ? fb.Username : fb.RelatedCustomer, CssClass: 'word-wrap' }
            ];

                status = fb.Status ? fb.Status.toLowerCase() : '';
                cssClass = (status !== 'pending' && status !== 'closed') ? '' : status;
                bodyData.push({ Columns: currentRow, CssClass: cssClass });
            }

            return bodyData;
        }

        function renderManageFeedback(report, scope, element) {
            if (!report) { return; }

            var totalItems = report.TotalItems;
            var feedbacks = totalItems > 0 ? report.Feedbacks : [];
            var bodyData = getGridBody(feedbacks);
            gridOptions = getGridOptions(scope, bodyData, totalItems);

            tableManageFeedback = element.find('#table-manage-feedback');
            tableManageFeedback
            .empty()
            .removeClass(crmConstants.Enums.CssClass.DisplayNone)
            .fanexGrid(gridOptions);

            if (totalItems > 0) {
                $('.desTooltip')
                .unbind('click')
                .bind('click', function () {
                    scope.openEditFeedbackPopup($(this).data('id'));
                });
            }
        }

        function renderCategoryColumn(main, sub) {
            return '<div class="main-category">' + main + '</div><span class="sub-category">' + sub + '</span></div>';
        }

        function renderFeedbackColumn(feedback) {
            var html = '<div style="margin:2px 0 0 2px;"> \
            <div class="lnksubject">\
                <a class="desTooltip" href="javascript:;" data-id="' + feedback.FeedbackId + '">\
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

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newValues, oldValues) {
                    renderManageFeedback(newValues, scope, element);
                });
            }
        };
    });
})();