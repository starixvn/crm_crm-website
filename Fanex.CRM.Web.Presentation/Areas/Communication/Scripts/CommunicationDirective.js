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
                            Value: sharedScriptManager.stringFormat(
                                '<div class="maincategory">{0}</div>\
                                 <div class="subcategory">{1}</div>', [info.MainCategory.Name, info.SubCategory.Name])
                        },
                        { Name: "ColSubject", Value: info.Subject },
                        { Name: "ColStatus", Value: info.StatusName },
                        { Name: "ColDuration", Value: info.Duration },
                        { Name: "ColHandler", Value: info.Handler },
                        { Name: "ColReviewer", Value: info.Reviewer}]
                });
            }
        }

        var gridOptions = {
            columns: [{ HeaderText: viewResources.Running_Number, Width: 120, Name: "ColRunningNumber", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Feedback_Date, Width: 140, Name: "ColFeedbackDate", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: sharedScriptManager.stringFormat("{0}/{1}", [viewResources.Main_Category, viewResources.Sub_Category]), Width: 400, Name: "ColCategory", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Subject, Width: 160, Name: "ColSubject", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Status, Width: 120, Name: "ColStatus", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Duration, Width: 120, Name: "ColDuration", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Handled_By, Width: 140, Name: "ColHandler", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Reviewed_By, Width: 140, Name: "ColReviewer", HeaderAlign: "center", CellAlign: "left"}],
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
            showHighlightRow: false,
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
        var editLogLink = tableCommunicationLogs.find(".editlog-link-td");
        editLogLink.unbind("click");
        editLogLink.click(function () {
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

            element.on('change', function () {
                scope.$apply(function () {
                    //                    scope.updateSelectedLanguage(element.find("#customerinfodetail-languageoptions").fanexMultiSelect("getItems", true).join(","));
                });
            });
        }
    };
});