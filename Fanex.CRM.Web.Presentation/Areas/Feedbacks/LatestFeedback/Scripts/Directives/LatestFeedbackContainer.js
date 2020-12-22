(function () {
    "use strict";
    /* global latestFeedbackResources,
    sharedScriptManager */
    angular.module('feedback-latest-application').directive('tableLatestFeedbackContainer', function () {
        var gridOptions, tableLatestFeedback;
        function getGridOptions(scope) {
            var columns = [];
            columns.push({ HeaderText: latestFeedbackResources.RunningNo, Width: 100, Name: 'ColRunningNo', HeaderAlign: 'center', CellAlign: 'center' });

            if (scope.createUpdateFeedbackPermission) {
                columns.push({ HeaderText: latestFeedbackResources.Update, Width: 50, Name: 'ColUpdate', HeaderAlign: 'center', CellAlign: 'center' });
            }

            columns.push({ HeaderText: latestFeedbackResources.Feedback, Width: 400, Name: 'ColFeedback', HeaderAlign: 'center', CellAlign: 'left' });
            columns.push({ HeaderText: latestFeedbackResources.Category, Width: 270, Name: 'ColCategory', HeaderAlign: 'center', CellAlign: 'left' });
            columns.push({ HeaderText: latestFeedbackResources.Status, Width: 80, Name: 'ColStatus', HeaderAlign: 'center', CellAlign: 'center' });
            columns.push({ HeaderText: latestFeedbackResources.RelatedCustomer, Width: 160, Name: 'ColRelatedCustomer', HeaderAlign: 'center', CellAlign: 'left' });

            return {
                columns: columns,
                showTitle: true,
                titleInfo: {
                    showExcelButton: true
                },
                showFooter: false,
                fullWidth: true,
                fullHeight: true,
                gridExpandHeight: 90,
                showDistinctRow: false,
                showHighlightRow: false,
                headerColumnHeight: 29,
                noDataText: latestFeedbackResources.There_is_no_data
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
                currentRow.push({ Name: 'ColRunningNo', Value: fb.FeedbackId });

                if (scope.createUpdateFeedbackPermission) {
                    currentRow.push({ Name: 'ColUpdate', Value: '<span class="updateIcon"  data-id="' + fb.FeedbackId + '"></span>' });
                }                

                currentRow.push({ Name: 'ColFeedback', Value: renderFeedbackColumn(fb, i) });
                currentRow.push({ Name: 'ColCategory', Value: nameMapping.renderCategoryColumn(fb, mainCategories) });
                currentRow.push({ Name: 'ColStatus', Value: fb.Status ? fb.Status : 'N/A' });
                currentRow.push({ Name: 'ColRelatedCustomer', Value: renderRelatedCustomerColumn(fb), CssClass: 'word-wrap' });

                status = fb.Status ? fb.Status.toLowerCase() : '';
                cssClass = (status !== 'pending' && status !== 'closed') ? '' : status;
                bodyData.push({ Columns: currentRow, CssClass: cssClass });
            }

            return bodyData;
        }

        function renderLatestFeedback(report, scope, element) {
            if (!report) { return; }

            var feedbacks = report.Feedbacks;
            var bodyData = getGridBody(feedbacks, report.MainCategories, scope);

            gridOptions = gridOptions || getGridOptions(scope);
            gridOptions.bodyRows = bodyData;
            gridOptions.titleInfo.text = report.Title;
            gridOptions.titleInfo.exportExcelFunction = function () {
                scope.exportFeedbackExcel();
            };

            tableLatestFeedback = element.find('#table-latest-feedback');
            tableLatestFeedback
                .empty()
                .removeClass('hide')
                .fanexGrid(gridOptions);

            if (bodyData.length > 0) {
                var tooltips = $('.desTooltip');
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
                            return renderTooltipLatest(feedbacksTooltip);
                        }
                    });

                $('.updateIcon')
                    .unbind('click')
                    .bind('click', function () {
                        scope.openQuickUpdateFeedbackPopup($(this).data('id'));
                    });

                $('.viewRelatedCustomer').unbind('click')
                    .bind('click', function () {
                        scope.showRelatedCustomer($(this).data('id'));
                    });
            }
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

        function renderTooltipLatest(feedbacksLatest) {
            var desctiptionText = !feedbacksLatest.Description ? '' : "Description: <br>" + feedbacksLatest.Description + "<br>";
            var solutionText = !feedbacksLatest.Solution ? '' : "Solution: <br>" + feedbacksLatest.Solution + "<br>";

            var lastCharacterSlit = desctiptionText.substring(578, 580);
            var isEscapseWord = "<br />".includes(lastCharacterSlit);
            var subLength = isEscapseWord ? 592 : 580;

            var lastCharacterSlitSolution = solutionText.substring(578, 580);
            var isEscapseWordSolution = "<br />".includes(lastCharacterSlitSolution);
            var subLengthSolution = isEscapseWordSolution ? 592 : 580;

            var tooltipDescription = desctiptionText.length > subLength ? desctiptionText.substring(0, subLength) + " ... <br><br>" : desctiptionText;
            var tooltipSolution = solutionText.length > subLengthSolution ? solutionText.substring(0, subLengthSolution) + " ... <br><br>" : solutionText;
            return tooltipDescription + tooltipSolution;
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newValues, oldValues) {
                    /* jshint unused: false */
                    renderLatestFeedback(newValues, scope, element);
                });
            }
        };
    });
})();