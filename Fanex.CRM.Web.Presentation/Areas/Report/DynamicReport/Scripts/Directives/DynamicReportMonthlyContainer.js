"use strict";
/* global dynamicReportApplication,
crmConstants,
sharedScriptManager,
viewResources */
/* jshint unused: vars */
dynamicReportApplication.directive('tableDynamicReportMonthlyContainer', function () {
    function getGripOtions(scope, dynamicReport) {
        var bodyData = [];
        var footerData = [];
        if (dynamicReport && dynamicReport.ChildReports.length > 0) {
            var childReportLength = dynamicReport.ChildReports.length;
            for (var i = 0; i < childReportLength; i++) {
                var periodReport = dynamicReport.ChildReports[i];
                var currentRow =
                {
                    Columns: [{
                        Name: "ColNo",
                        Value: periodReport.Name +
                            ' <span class="link viewsubreport-link" data-subreportindex="' + i + '">' + periodReport.TotalLogs + '</span>',
                        MergeColNumber: 9
                    }],
                    SubRows: []
                };

                var childsLength = periodReport.ChildReports.length;
                for (var j = 0; j < childsLength; j++) {
                    var info = periodReport.ChildReports[j];
                    var currentSubRow =
                    {
                        Columns: [{ Name: "ColNo", Value: j + 1 },
                            {
                                Name: "ColRunningNumber",
                                Value: '<span class="link viewcommunicationlog-link" data-subreportindex="' + i + '" data-logindex="' + j + '">' + info.RunningNumber + '</span>',
                                TdClass: 'viewcommunicationlog-link-td'
                            },
                            { Name: "ColFeedbackDate", Value: info.PeriodRange.FromDateString },
                            {
                                Name: "ColCategory",
                                Value: ('<div class="maincategory">' + info.MainCategory.Name + '</div>') +
                                    (info.SubCategory.Name !== "" ? '<div class="subcategory">' + info.SubCategory.Name + '</div>' : "")
                            },
                            { Name: "ColSubject", Value: info.RunningNumber },
                            { Name: "ColStatus", Value: info.RunningNumber },
                            {
                                Name: "ColDuration",
                                Value: sharedScriptManager.stringFormat("{0}:{1}:{2}",
                                [info.Duration.Hours, info.Duration.Minutes, info.Duration.Seconds])
                            },
                            { Name: "ColHandler", Value: info.RunningNumber },
                            { Name: "ColReviewer", Value: info.RunningNumber}],
                        CssClass: 'viewcommunicationlog-link-row row-' + info.StatusName.toLowerCase()
                    };

                    currentRow.SubRows.push(currentSubRow);
                }

                bodyData.push(currentRow);
            }
        }

        var breadcrumb = (scope.lastSelectedParams.ReportType === crmConstants.Enums.PeriodType.Yearly ?
            '<span>' + scope.reportByYearly.Id + '<span> > ' :
            '') +
            sharedScriptManager.stringFormat("{0} [{1}]",
                [dynamicReport.Name, dynamicReport.TotalLogs]);
        var gridOptions = {
            columns: [{ HeaderText: viewResources.Symbol_No, Width: 60, Name: "ColNo", HeaderAlign: "center", CellAlign: "left", ShowCollapseExpand: true },
                { HeaderText: viewResources.Running_No, Width: 80, Name: "ColRunningNumber", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Feedback_Date, Width: 90, Name: "ColFeedbackDate", HeaderAlign: "center", CellAlign: "center" },
                {
                    HeaderText: sharedScriptManager.stringFormat("{0}/{1}", [viewResources.Main_Category, viewResources.Sub_Category]),
                    Width: 200,
                    Name: "ColCategory",
                    HeaderAlign: "center",
                    CellAlign: "left"
                },
                { HeaderText: viewResources.Subject, Width: 130, Name: "ColSubject", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Status, Width: 70, Name: "ColStatus", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Duration, Width: 80, Name: "ColDuration", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Handled_By, Width: 100, Name: "ColHandler", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Reviewed_By, Width: 100, Name: "ColReviewer", HeaderAlign: "center", CellAlign: "left"}],
            showTitle: true,
            titleInfo: {
                text: breadcrumb,
                showExcelButton: true,
                exportExcelFunction: function () {
                    exportReport();
                },
                showCollapseExpandButton: true
            },
            bodyRows: bodyData,
            footerRows: footerData,
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 250,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 29,
            bodyRowHeight: 51,
            showPager: true,
            showTree: true,
            pagerOption: {
                itemsPerPage: sharedScriptManager.cachedData.defaultItemsPerPage,
                currentPage: 0,
                itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                totalItem: bodyData.length
            }
        };

        return gridOptions;
    }

    function renderDynamicReport(scope, element, attrs) {
        if (scope.reportByMonthly === null) {
            return;
        }

        var tableDynamicReport = element.find("#table-dynamic-report-monthly");
        var dynamicReport = scope.reportByMonthly;
        var gridOptions = getGripOtions(scope, dynamicReport);
        tableDynamicReport.empty().removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
        var subReportLinks = tableDynamicReport.find(".viewsubreport-link");
        subReportLinks.click(function () {
            scope.getWeekyReport(parseInt($(this).attr("data-subreportindex")));
        });
    }
    return {
        restrict: 'C',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (oldval, newval) {
                /* jshint unused: vars */
                renderDynamicReport(scope, element, attrs);
            });
        }
    };
});