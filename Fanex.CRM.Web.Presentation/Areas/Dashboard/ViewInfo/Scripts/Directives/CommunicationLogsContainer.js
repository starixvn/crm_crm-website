(function () {
    "use strict";
    /* global moment,
    crmConstants,
    sharedScriptManager,
    viewResources */
    /* jshint camelcase: false */
    angular.module('dashboard-application').directive('tableCommunicationLogsContainer', function () {
        function renderCommunicationLogs(scope, element) {
            if (scope.communicationLogs == null) {
                return;
            }

            var tableCommunicationLogs = element.find("#table-communication-logs");
            var logList = scope.communicationLogs;
            var bodyData = [];
            var footerData = [];

            tableCommunicationLogs.empty();
            if (logList && logList.length > 0) {
                var logLength = logList.length;
                for (var i = 0; i < logLength; i++) {
                    var info = logList[i];
                    var dataColumns = [];
                    dataColumns.push({
                        Name: "ColRunningNumber",
                        Value: '<span class="link editlog-link" data-logid="' + info.RunningNumber + '">' + info.RunningNumber + '</span>',
                        TdClass: 'editlog-link-td'
                    });

                    if (scope.createUpdateFeedbackPermission) {
                        dataColumns.push({ Name: 'ColUpdate', Value: '<span class="updateIcon"  data-id="' + info.RunningNumber + '"></span>' });
                    }

                    dataColumns.push({
                        Name: "ColFeedbackDate",
                        Value: moment(new Date(info.FeedbackDate)).format(crmConstants.Enums.FormatConstant.MM_DD_YYYY_HH_MM_TT_2Line_String_Moment)
                    });
                    dataColumns.push({
                        Name: "ColCategory",
                        Value: nameMapping.renderCategoryColumn(info, scope.mainCategory)
                    });
                    dataColumns.push({ Name: "ColSubject", Value: sharedScriptManager.encodeHtml(info.Subject) });
                    dataColumns.push({ Name: "ColStatus", Value: info.StatusName });
                    dataColumns.push({
                        Name: "ColDuration",
                        Value: sharedScriptManager.stringFormat("{0}:{1}:{2}",
                        [info.Duration.Hours, info.Duration.Minutes, info.Duration.Seconds])
                    });
                    dataColumns.push({ Name: "ColHandler", Value: info.Handler });
                    dataColumns.push({ Name: "ColReviewer", Value: info.Reviewer });
                    if (!scope.isInactiveSite) {
                        dataColumns.push({ Name: "ColDuplicate", Value: '<button class="btn duplicate-btn" data-logid="' + info.RunningNumber + '">' + viewResources.Duplicate + '</button>' });
                    }
                    else {
                        dataColumns.push({ Name: "ColDuplicate", Value: '<button disabled class="btn duplicate-btn" data-logid="' + info.RunningNumber + '">' + viewResources.Duplicate + '</button>' });
                    }

                    bodyData.push({
                        Columns: dataColumns,
                        CssClass: 'editlog-link-row row-' + info.StatusName.toLowerCase()
                    });
                }
            }

            var columns = [];
            columns.push({ HeaderText: viewResources.Running_No, Width: 80, Name: "ColRunningNumber", HeaderAlign: "center", CellAlign: "left" });

            if (scope.createUpdateFeedbackPermission) {
                columns.push({ HeaderText: viewResources.Update, Width: 50, Name: 'ColUpdate', HeaderAlign: 'center', CellAlign: 'center' });
            }

            columns.push({ HeaderText: viewResources.Feedback_Date, Width: 90, Name: "ColFeedbackDate", HeaderAlign: "center", CellAlign: "center" });
            columns.push({
                        HeaderText: sharedScriptManager.stringFormat("{0}/{1}", [viewResources.Main_Category, viewResources.Sub_Category]),
                            Width: 280,
                                Name: "ColCategory",
                                    HeaderAlign: "center",
                                        CellAlign: "left"
            });
            columns.push({ HeaderText: viewResources.Subject, Width: 140, Name: "ColSubject", HeaderAlign: "center", CellAlign: "left" });
            columns.push({ HeaderText: viewResources.Status, Width: 80, Name: "ColStatus", HeaderAlign: "center", CellAlign: "center" });
            columns.push({ HeaderText: viewResources.Duration, Width: 60, Name: "ColDuration", HeaderAlign: "center", CellAlign: "center" });
            columns.push({ HeaderText: viewResources.Handler, Width: 100, Name: "ColHandler", HeaderAlign: "center", CellAlign: "left" });
            columns.push({ HeaderText: viewResources.Reviewer, Width: 100, Name: "ColReviewer", HeaderAlign: "center", CellAlign: "left" });
            columns.push({ HeaderText: viewResources.Duplicate, Width: 100, Name: "ColDuplicate", HeaderAlign: "center", CellAlign: "center" });

            var gridOptions = {
                columns: columns,
                showTitle: true,
                titleInfo: {
                    text: viewResources.Last_10_Communication_Log
                },
                bodyRows: bodyData,
                footerRows: footerData,
                numberOfFrozenColumn: 0,
                fullWidth: true,
                fullHeight: false,
                gridHeight: $(window).height() - $("#table-redemption-logs").height() -
                    $(".height-remark").height() - 200,
                showDistinctRow: false,
                showHighlightRow: true,
                headerColumnHeight: 29,
                bodyRowHeight: 25,
                showPager: false,
                noDataText: viewResources.There_is_no_data
            };

            tableCommunicationLogs.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
            var editLogLinks = tableCommunicationLogs.find(".editlog-link-td .editlog-link");
            editLogLinks.unbind("click");
            editLogLinks.click(function () {
                scope.editCommunicationLog($(this).attr("data-logid"));
            });

            var editLogRows = tableCommunicationLogs.find(".editlog-link-row");
            editLogRows.dblclick(function () {
                scope.editCommunicationLog($(this).find('.editlog-link').attr("data-logid"));
            });

            tableCommunicationLogs.find('.updateIcon')
                .unbind('click')
                .bind('click', function () {
                    scope.openQuickUpdateFeedbackPopup($(this).data('id'));
                });
            if(!scope.isInactiveSite) {
                var duplicateBtns = tableCommunicationLogs.find(".duplicate-btn");
                duplicateBtns.unbind("click");
                duplicateBtns.click(function () {
                    scope.duplicateCommunicationLog($(this).attr("data-logid"));
                });
            }
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (oldval, newval) {
                    /* jshint unused: false */
                    renderCommunicationLogs(scope, element);
                });
            }
        };
    });
})();