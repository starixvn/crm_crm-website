(function () {
    "use strict";
    /* global manageFeedbackResources,
    crmConstants,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('feedback-manage-application').directive('tableManageFeedbackContainer', function () {
        var gridOptions, tableManageFeedback;
        function getGridOptions(scope, bodyData, totalItems) {
            function ReRenderAfterPaged(page_index, itemsPerPage) {
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
                    {
                        HeaderText: '<input type="checkbox" class="checkActionAll"/>',
                        Name: "Edit",
                        Width: 50,
                        HeaderAlign: 'center', CellAlign: 'center'
                    },
                    { HeaderText: manageFeedbackResources.RunningNo, Width: 100, Name: 'ColRunningNo', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: manageFeedbackResources.Feedback, Width: 380, Name: 'ColFeedback', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: manageFeedbackResources.Customer, Width: 130, Name: 'ColCustomer', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: manageFeedbackResources.Category, Width: 270, Name: 'ColCategory', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: manageFeedbackResources.Status, Width: 50, Name: 'ColStatus', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: manageFeedbackResources.RelatedCustomer, Width: 130, Name: 'ColRelatedCustomer', HeaderAlign: 'center', CellAlign: 'left' }],
                showTitle: true,
                titleInfo: {
                    text: renderTitle(scope),
                    showExcelButton: true,
                    exportExcelFunction: function () {
                        scope.exportFeedbackExcel();
                    }
                },
                bodyRows: bodyData,
                showFooter: false,
                numberOfFrozenColumn: 1,
                autoFixFrozenRowHeight: true,
                fullWidth: true,
                fullHeight: true,
                gridExpandHeight: 210,
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
                },
                noDataText: manageFeedbackResources.There_is_no_data
            };
        }

        function getGridBody(feedbacks, mainCategories) {
            var fbLength = feedbacks.length;
            if (fbLength === 0) { return []; }

            var bodyData = [];
            var fb, currentRow, status, cssClass;
            for (var i = 0; i < fbLength; i++) {
                fb = feedbacks[i];
                var checkedClass = fb.IsChecked ? 'checked="checked"' : '';
                currentRow = [
                    { Name: 'Edit', Value: '<input type="checkbox" class="checkActionItem" data-id="' + fb.FeedbackId + '" />' },
                    { Name: 'ColRunningNo', Value: fb.FeedbackId },
                    { Name: 'ColFeedback', Value: renderFeedbackColumn(fb) },
                    { Name: 'ColCustomer', Value: fb.Username, CssClass: 'word-wrap' },
                    { Name: 'ColCategory', Value: nameMapping.renderCategoryColumn(fb, mainCategories) },
                    { Name: 'ColStatus', Value: fb.Status ? fb.Status : 'N/A' },
                    { Name: 'ColRelatedCustomer', Value: renderRelatedCustomerColumn(fb), CssClass: 'word-wrap' }
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
            var bodyData = getGridBody(feedbacks, report.MainCategories);
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

                $('.desTitle')
                    .unbind('click')
                    .bind('click', function () {
                        scope.openEditMultipleLogs();
                    });

                $('.multipleStatusesTitle')
                    .unbind('click')
                    .bind('click', function () {
                        scope.openEditMultipleLogStatuses();
                    });

                $('.updateSiteLicensee')
                    .unbind('click')
                    .bind('click', function () {
                        scope.updateSiteLicenseeType();
                    });

                $('.checkActionItem')
                    .unbind('click')
                    .bind('click', function () {
                        scope.checkActionItem($(this).data('id'));
                        $(".checkActionAll").prop('checked', scope.getCheckAllStatus());
                    });

                $('.checkActionAll')
                    .unbind('click')
                    .bind('click', function () {
                        scope.checkActionAll();
                        $(".checkActionItem").prop('checked', scope.getCheckAllStatus());
                    });

                $('.viewRelatedCustomer').unbind('click')
                    .bind('click', function () {
                        scope.showRelatedCustomer($(this).data('id'));
                    });
            }
        }

        function renderTitle(scope) {
            var updateSiteLicenseeType = scope.isCN88 ? '' : '<div class="update-site-licensee"><a class="updateSiteLicensee" href="javascript:;">' + manageFeedbackResources.UpdateSiteLicenseeType + '</a></div>' +
                '<div class="split-bar"> | </div>';
            var updateButtons = !scope.createUpdateFeedbackPermission ? '' :
                updateSiteLicenseeType +
                '<div class="lnksubject multiple-statuses-edit"><a class="multipleStatusesTitle" href="javascript:;">' + manageFeedbackResources.EditMultipleLogStatusesTitle + '</a></div>' +
                '<div class="split-bar"> | </div>' +
                '<div class="lnksubject multiple-edit"><a class="desTitle" href="javascript:;">' + manageFeedbackResources.MultipleEdit + '</a></div>' +
                '<div class="split-bar"> | </div>';

            return '<div class="feed-back">' + manageFeedbackResources.Feedback + '</div>' + updateButtons;
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

        function renderRelatedCustomerColumn(feedback) {
            var html = feedback.Username;

            if (!feedback.IsRelated) {
                var customers = feedback.RelatedCustomer.split(',');
                if (customers.length > 20) {
                    html = '<a class="viewRelatedCustomer" href="javascript:;" data-id="' + feedback.FeedbackId + '">'
                        + customers.length + ' customers</a>';
                } else {
                    html = customers.join(', ');
                }
            }

            return html;
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newValues, oldValues) {
                    /* jshint unused: false */
                    renderManageFeedback(newValues, scope, element);
                });
            }
        };
    });
})();