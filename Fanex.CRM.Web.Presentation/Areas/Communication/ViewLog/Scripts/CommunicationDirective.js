communicationApplication.directive('tableCommunicationLogsContainer', function () {
    function renderCommunicationLogs(scope, element, attrs) {
        var tableCommunicationLogs = element.find("#table-communication-logs");
        var logList = scope.logs;
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
                            Value: '<div class="link editlog-link" data-logindex="' + i + '">' + info.RunningNumber + '</div>',
                            TdClass: 'editlog-link-td'
                        },
                        {
                            Name: "ColFeedbackDate",
                            Value: moment(new Date(info.FeedbackDate)).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment)
                        },
                        {
                            Name: "ColCategory",
                            Value: ('<div class="maincategory">' + info.MainCategory.Name + '</div>') +
                                (info.SubCategory.Name != "" ? '<div class="subcategory">' + info.SubCategory.Name + '</div>' : "")
                        },
                        { Name: "ColSubject", Value: info.Subject },
                        { Name: "ColStatus", Value: info.StatusName },
                        { Name: "ColDuration", Value: sharedScriptManager.stringFormat("{0}:{1}:{2}", [info.Duration.Hours, info.Duration.Minutes, info.Duration.Seconds]) },
                        { Name: "ColHandler", Value: info.Handler },
                        { Name: "ColReviewer", Value: info.Reviewer}],
                    CssClass: 'editlog-link-row row-' + info.StatusName.toLowerCase()
                });
            }
        }

        var gridOptions = {
            columns: [{ HeaderText: viewResources.Running_Number, Width: 110, Name: "ColRunningNumber", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Feedback_Date, Width: 100, Name: "ColFeedbackDate", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: sharedScriptManager.stringFormat("{0}/{1}", [viewResources.Main_Category, viewResources.Sub_Category]), Width: 220, Name: "ColCategory", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Subject, Width: 140, Name: "ColSubject", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Status, Width: 60, Name: "ColStatus", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Duration, Width: 80, Name: "ColDuration", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Handled_By, Width: 100, Name: "ColHandler", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Reviewed_By, Width: 100, Name: "ColReviewer", HeaderAlign: "center", CellAlign: "left"}],
            showTitle: true,
            titleInfo: {
                text: viewResources.Customer_Communication_Logs
            },
            bodyRows: bodyData,
            footerRows: footerData,
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 150,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 29,
            bodyRowHeight: 29,
            showPager: true,
            pagerOption: {
                itemsPerPage: sharedScriptManager.cachedData.defaultItemsPerPage,
                currentPage: 0,
                itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                totalItem: bodyData.length
            }
        };

        tableCommunicationLogs.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
        var editLogLinks = tableCommunicationLogs.find(".editlog-link-td");
        editLogLinks.unbind("click");
        editLogLinks.click(function () {
            scope.editCommunicationLog($(this).find('.editlog-link').attr("data-logindex"));
        });

        var editLogRows = tableCommunicationLogs.find(".editlog-link-row");
        editLogRows.dblclick(function () {
            scope.editCommunicationLog($(this).find('.editlog-link').attr("data-logindex"));
        });
    };

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (oldval, newval) {
                renderCommunicationLogs(scope, element, attrs);
            });
        }
    };
});