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
                    bodyData.push({
                        Columns: [
                            {
                                Name: "ColRunningNumber",
                                Value: '<span class="link editlog-link" data-logid="' + info.RunningNumber + '">' + info.RunningNumber + '</span>',
                                TdClass: 'editlog-link-td'
                            },
                            { Name: 'ColUpdate', Value: '<span class="updateIcon"  data-id="' + info.RunningNumber + '"></span>' },
                            {
                                Name: "ColFeedbackDate",
                                Value: moment(new Date(info.FeedbackDate)).format(crmConstants.Enums.FormatConstant.MM_DD_YYYY_HH_MM_TT_2Line_String_Moment)
                            },
                            {
                                Name: "ColCategory",
                                Value: nameMapping.renderCategoryColumn(info, scope.mainCategory)
                            },
                            { Name: "ColSubject", Value: sharedScriptManager.encodeHtml(info.Subject) },
                            { Name: "ColStatus", Value: info.StatusName },
                            {
                                Name: "ColDuration",
                                Value: sharedScriptManager.stringFormat("{0}:{1}:{2}",
                                    [info.Duration.Hours, info.Duration.Minutes, info.Duration.Seconds])
                            },
                            { Name: "ColHandler", Value: info.Handler },
                            { Name: "ColReviewer", Value: info.Reviewer },
                            { Name: "ColDuplicate", Value: '<button class="btn duplicate-btn" data-logid="' + info.RunningNumber + '">' + viewResources.Duplicate + '</button>' }],
                        CssClass: 'editlog-link-row row-' + info.StatusName.toLowerCase()
                    });
                }
            }

            var gridOptions = {
                columns: [{ HeaderText: viewResources.Running_No, Width: 80, Name: "ColRunningNumber", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Update, Width: 50, Name: 'ColUpdate', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: viewResources.Feedback_Date, Width: 90, Name: "ColFeedbackDate", HeaderAlign: "center", CellAlign: "center" },
                {
                    HeaderText: sharedScriptManager.stringFormat("{0}/{1}", [viewResources.Main_Category, viewResources.Sub_Category]),
                    Width: 280,
                    Name: "ColCategory",
                    HeaderAlign: "center",
                    CellAlign: "left"
                },
                { HeaderText: viewResources.Subject, Width: 140, Name: "ColSubject", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Status, Width: 80, Name: "ColStatus", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Duration, Width: 60, Name: "ColDuration", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Handler, Width: 100, Name: "ColHandler", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Reviewer, Width: 100, Name: "ColReviewer", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Duplicate, Width: 100, Name: "ColDuplicate", HeaderAlign: "center", CellAlign: "center" }],
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
                    $(".specialRemark").height() - 120,
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

            var duplicateBtns = tableCommunicationLogs.find(".duplicate-btn");
            duplicateBtns.unbind("click");
            duplicateBtns.click(function () {
                scope.duplicateCommunicationLog($(this).attr("data-logid"));
            });
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