(function (angular) {
    'use strict';
    /* global manageFeedbackResources, crmConstants, sharedScriptManager */

    angular.module('manageFeedbackModule').directive('manageFeedbackGrid', function () {
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

            var columns = [];
            columns.push({
                HeaderText: '<input type="checkbox" class="checkActionAll"/>',
                Name: "Edit",
                Width: 50,
                HeaderAlign: 'center', CellAlign: 'center'
            });

            if (scope.createUpdateFeedbackPermission) {
                columns.push({ HeaderText: manageFeedbackResources.Update, Width: 50, Name: 'ColUpdate', HeaderAlign: 'center', CellAlign: 'center' });
            }

            columns.push({ HeaderText: manageFeedbackResources.RunningNo, Width: 100, Name: 'ColRunningNo', HeaderAlign: 'center', CellAlign: 'center' });
            columns.push({ HeaderText: manageFeedbackResources.Feedback, Width: 380, Name: 'ColFeedback', HeaderAlign: 'center', CellAlign: 'left' });
            columns.push({ HeaderText: manageFeedbackResources.Customer, Width: 130, Name: 'ColCustomer', HeaderAlign: 'center', CellAlign: 'left' });
            columns.push({ HeaderText: manageFeedbackResources.Category, Width: 270, Name: 'ColCategory', HeaderAlign: 'center', CellAlign: 'left' });
            columns.push({ HeaderText: manageFeedbackResources.Status, Width: 80, Name: 'ColStatus', HeaderAlign: 'center', CellAlign: 'center' });
            columns.push({ HeaderText: manageFeedbackResources.RelatedCustomer, Width: 130, Name: 'ColRelatedCustomer', HeaderAlign: 'center', CellAlign: 'left' });

            return {
                columns: columns,
                showTitle: true,
                titleInfo: {
                    text: renderTitle(scope),
                    showExcelButton: true,
                    exportExcelFunction: function () {
                        scope.exportFeedbackExcel();
                    }
                },
                bodyRows: bodyData,
                gridExpandHeight: 210,
                showDistinctRow: false,
                showHighlightRow: false,
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

        function getGridBody(feedbacks, mainCategories, scope) {
            var fbLength = feedbacks.length;
            if (fbLength === 0) { return []; }

            var bodyData = [];
            var fb, currentRow, status, cssClass;
            for (var i = 0; i < fbLength; i++) {
                fb = feedbacks[i];
                currentRow = [];
                currentRow.push({ Name: 'Edit', Value: '<input type="checkbox" class="checkActionItem" data-id="' + fb.FeedbackId + '" />' });

                if (scope.createUpdateFeedbackPermission) {
                    currentRow.push({ Name: 'ColUpdate', Value: '<span class="updateIcon"  data-id="' + fb.FeedbackId + '"></span>' });
                }

                currentRow.push({ Name: 'ColRunningNo', Value: fb.FeedbackId });
                currentRow.push({ Name: 'ColFeedback', Value: renderFeedbackColumn(fb, i) });
                currentRow.push({ Name: 'ColCustomer', Value: fb.Username, CssClass: 'word-wrap' });
                currentRow.push({ Name: 'ColCategory', Value: nameMapping.renderCategoryColumn(fb, mainCategories) });
                currentRow.push({ Name: 'ColStatus', Value: fb.Status ? fb.Status : 'N/A' });
                currentRow.push({ Name: 'ColRelatedCustomer', Value: renderRelatedCustomerColumn(fb), CssClass: 'word-wrap' });

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
            var bodyData = getGridBody(feedbacks, report.MainCategories, scope);
            gridOptions = getGridOptions(scope, bodyData, totalItems);

            tableManageFeedback = element.find('#table-manage-feedback');
            tableManageFeedback
                .empty()
                .removeClass(crmConstants.Enums.CssClass.DisplayNone)
                .fanexGrid(gridOptions);

            if (totalItems > 0) {
                var tooltips = element.find('.desTooltip');
                tooltips.unbind('click')
                    .bind('click', function () {
                        var $this = $(this);
                        $(".tipsy").remove();
                        scope.openEditFeedbackPopup($this.data('id'));
                    }).tipsy({
                        fade: true,
                        gravity: 'nw',
                        html: true,
                        title: function () {
                            var feedbacksTooltip = feedbacks[$(this).data('index')];
                            return renderTooltipManage(feedbacksTooltip);
                        }
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

                $('.updateIcon')
                    .unbind('click')
                    .bind('click', function () {
                        scope.openQuickUpdateFeedbackPopup($(this).data('id'));
                    });
            }
        }

        function renderTitle(scope) {
            const updateButtons = !scope.createUpdateFeedbackPermission ? '' :
                '<div class="lnksubject multiple-statuses-edit"><a class="multipleStatusesTitle" href="javascript:;">' + manageFeedbackResources.EditMultipleLogStatusesTitle + '</a></div>' +
                '<div class="split-bar"> | </div>' +
                '<div class="lnksubject multiple-edit"><a class="desTitle" href="javascript:;">' + manageFeedbackResources.MultipleEdit + '</a></div>' +
                '<div class="split-bar"> | </div>';

            return '<div class="feed-back">' + manageFeedbackResources.Feedback + '</div>' + updateButtons;
        }

        function renderFeedbackColumn(feedback, number) {
            var html = '<div style="margin:2px 0 0 2px;"> \
            <div class="lnksubject">\
                <a class="desTooltip" href="javascript:;" data-id="' + feedback.FeedbackId + '" data-index="' + number + '">\
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

        function renderTooltipManage(feedbacksManage) {
            var desctiptionText = !feedbacksManage.Description ? '' : "Description: <br>" + feedbacksManage.Description + "<br>";
            var solutionText = !feedbacksManage.Solution ? '' : "Solution: <br>" + feedbacksManage.Solution + "<br>";
            var tooltipDesctiption = desctiptionText.length > 650 ? desctiptionText.substring(0, 650) + " ... <br><br>" : desctiptionText;
            var tooltipSolution = solutionText.length > 650 ? solutionText.substring(0, 650) + " ... <br><br>" : solutionText;
            var displayTooltips = tooltipDesctiption + tooltipSolution;
            return displayTooltips
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
})(angular);