(function () {
    "use strict";
    /* global crmConstants,
    sharedScriptManager,
    viewResources,
    moment */
    /* jshint camelcase: false */
    angular.module('report-dynamicreport-application').directive('tableDynamicReportContainer', function () {
        var cachedDynamicReport;
        var cachedGridOptions;
        var selfScope;
        var cachedTable;

        function getGridHeight() {
            return selfScope.isShowFilter ?
                245 + crmConstants.MarginBottomPage :
                65 + crmConstants.MarginBottomPage;
        }

        function getGripOtions(dynamicReport, mainCategory) {
            var bodyData = [];
            var childReportLength = dynamicReport ? dynamicReport.ChildReports.length : 0;
            if (childReportLength > 0) {
                for (var i = 0; i < childReportLength; i++) {
                    var periodReport = dynamicReport.ChildReports[i];
                    var currentRow =
                    {
                        Columns: [{
                            Name: "ColNo",
                            Value: sharedScriptManager.stringFormat("{0} [{1}]",
                                [periodReport.Name, periodReport.TotalChildReports]),
                            MergeColNumber: 10
                        }],
                        SubRows: []
                    };

                    var childsLength = periodReport.ChildReports.length;
                    var startSplittingIndex = (i !== 0 || childsLength === periodReport.TotalChildReports) ?
                        0 :
                        periodReport.StartChildIndex;

                    for (var j = 0; j < childsLength; j++) {
                        var info = periodReport.ChildReports[j];
                        var currentSubRow =
                        {
                            Columns: [
                                {
                                    Name: "ColNo",
                                    Value: startSplittingIndex + j + 1,
                                    CellAlign: "center"
                                },
                                {
                                    Name: "ColRunningNumber",
                                    Value: '<span class="link viewcommunicationlog-link" data-subreportindex="' +
                                        i + '" data-logindex="' + j + '">' + info.RunningNumber + '</span>',
                                    TdClass: 'viewcommunicationlog-link-td'
                                },
                                {
                                    Name: "ColFeedbackDate",
                                    Value: moment(new Date(info.PeriodRange.FromDateString))
                                        .format(crmConstants.Enums.FormatConstant.MM_DD_YYYY_HH_MM_TT_2Line_String_Moment)
                                },
                                {
                                    Name: "ColCategory",
                                    Value: nameMapping.renderCategoryColumn(info, mainCategory, true)
                                },
                                { Name: "ColSubject", Value: info.Subject },
                                { Name: "ColStatus", Value: info.StatusName },
                                {
                                    Name: "ColDuration",
                                    Value: sharedScriptManager.stringFormat("{0}:{1}:{2}",
                                        [info.Duration.Hours, info.Duration.Minutes, info.Duration.Seconds])
                                },
                                { Name: "ColUsername", Value: info.Username },
                                { Name: "ColHandler", Value: info.Handler },
                                { Name: "ColReviewer", Value: info.Reviewer }],
                            CssClass: 'viewcommunicationlog-link-row row-' + info.StatusName.toLowerCase()
                        };

                        currentRow.SubRows.push(currentSubRow);
                    }

                    bodyData.push(currentRow);
                }
            }

            var viewChartIcon = '<input id="button-viewchart" class="btnCharting" type="button" value="">';
            var gridOptions = {
                columns: [{ HeaderText: viewResources.Symbol_No, Width: 50, Name: "ColNo", HeaderAlign: "center", CellAlign: "left", ShowCollapseExpand: true },
                { HeaderText: viewResources.Running_No, Width: 80, Name: "ColRunningNumber", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Feedback_Date, Width: 90, Name: "ColFeedbackDate", HeaderAlign: "center", CellAlign: "center" },
                {
                    HeaderText: sharedScriptManager.stringFormat("{0}/{1}", [viewResources.Main_Category, viewResources.Sub_Category]),
                    Width: 240,
                    Name: "ColCategory",
                    HeaderAlign: "center",
                    CellAlign: "left"
                },
                { HeaderText: viewResources.Subject, Width: 130, Name: "ColSubject", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Status, Width: 80, Name: "ColStatus", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Duration, Width: 60, Name: "ColDuration", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Username, Width: 100, Name: "ColUsername", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Handler, Width: 100, Name: "ColHandler", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Reviewer, Width: 100, Name: "ColReviewer", HeaderAlign: "center", CellAlign: "left" }],
                showTitle: true,
                titleInfo: {
                    text: dynamicReport ?
                        sharedScriptManager.stringFormat("{0} [{1}]",
                            [dynamicReport.Name, dynamicReport.TotalChildReports]) :
                        "",
                    showExcelButton: true,
                    exportExcelFunction: function () {
                        selfScope.exportReport();
                    },
                    showCollapseExpandButton: true,
                    customButtons: viewChartIcon
                },
                bodyRows: bodyData,
                numberOfFrozenColumn: 0,
                fullWidth: true,
                fullHeight: true,
                gridExpandHeight: getGridHeight(),
                showDistinctRow: false,
                showHighlightRow: true,
                showPager: true,
                showTree: true,
                treeParentPaddingLeft: 5,
                customPager: true,
                showNumberColumn: true,
                pagerOption: {
                    itemsPerPage: selfScope.lastSelectedParams.PageSize,
                    currentPage: selfScope.lastSelectedParams.PageIndex,
                    itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                    totalItem: dynamicReport ? dynamicReport.TotalChildReports : 0,
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };

            return gridOptions;
        }
        function bindLogLink(tableDynamicReport, scope) {
            var viewLogLinks = tableDynamicReport.find(".viewcommunicationlog-link-td .viewcommunicationlog-link");
            viewLogLinks.unbind("click");
            viewLogLinks.click(function () {
                var subReportIndex = parseInt($(this).attr("data-subreportindex"));
                var logIndex = parseInt($(this).attr("data-logindex"));
                scope.viewCommunicationLog(cachedDynamicReport.ChildReports[subReportIndex].ChildReports[logIndex]);
            });

            var viewLogRows = tableDynamicReport.find(".viewcommunicationlog-link-row");
            viewLogRows.dblclick(function () {
                var subReportIndex = parseInt($(this).find('.viewcommunicationlog-link').attr("data-subreportindex"));
                var logIndex = parseInt($(this).find('.viewcommunicationlog-link').attr("data-logindex"));
                scope.viewCommunicationLog(cachedDynamicReport.ChildReports[subReportIndex].ChildReports[logIndex]);
            });
        }

        function renderDynamicReport(scope, element) {
            if (scope.dynamicReportOptions.DataList === null) {
                return;
            }

            selfScope = scope;
            var tableDynamicReport = element.find("#table-dynamic-report");
            cachedTable = tableDynamicReport;
            var dynamicReport = scope.dynamicReportOptions.DataList;
            var mainCategory = scope.dynamicReportOptions.mainCategory;
            cachedDynamicReport = dynamicReport;
            var gridOptions = getGripOtions(dynamicReport, mainCategory);
            cachedGridOptions = gridOptions;
            tableDynamicReport.empty().removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
            bindLogLink(tableDynamicReport, scope);
            element.find("#button-viewchart").click(function () {
                viewChart(scope, element);
            });
        }

        function ReRenderAfterPaged(page_index, itemsPerPage) {
            selfScope.pagingDynamicReport(page_index, itemsPerPage);
            return false;
        }

        function viewChart(scope, element) {
            /* jshint unused: false */
            var crmChart = parent.crmChart;
            var chartInfo = cachedDynamicReport ? cachedDynamicReport.ChartInfo : null;
            crmChart.title = chartInfo.Name;

            var chartData = {
                labels: getChartLabels(chartInfo),
                datasets: getChartDatasets(chartInfo, crmChart)
            };

            var chartOption = angular.copy(crmChart.newopts);
            chartOption = $.extend(chartOption, {
                chartType: crmChart.chartType.bar,
                xAxisLabel: "",
                yAxisLabel: "",
                yAxisUnit: viewResources.DynamicReport_ChartSuffix
            });

            crmChart.viewChart(chartData, chartOption);
            parent.$(".ui-dialog-titlebar-close").click(function () {
                scope.setFocus();
            });
        }

        function getChartLabels(chartInfo) {
            var xAxisCategories = [];
            xAxisCategories.push(chartInfo ? chartInfo.ShortName : "");
            return xAxisCategories;
        }

        function getChartDatasets(chartInfo, crmChart) {
            var chartSeries = [];
            var chartSerie = null;
            var childReportLength = chartInfo ? chartInfo.ChildReports.length : 0;
            for (var xIndex = 0; xIndex < childReportLength; xIndex++) {
                var periodReport = chartInfo.ChildReports[xIndex];
                var backgroundColor = crmChart.getBackGroundColor(xIndex);
                chartSerie = {
                    fillColor: backgroundColor,
                    strokeColor: backgroundColor,
                    data: [periodReport.TotalChildReports],
                    title: periodReport.ShortName
                };
                chartSeries.push(chartSerie);
            }

            return chartSeries;
        }

        return {
            restrict: 'C',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (oldval, newval) {
                    /* jshint unused: false */
                    if (!scope.dynamicReportOptions.IsUpdateGrid) {
                        return;
                    }

                    if (scope.dynamicReportOptions.IsResize && cachedTable) {
                        cachedTable.fanexGrid("resizeHeight", getGridHeight());
                        scope.dynamicReportOptions.IsResize = false;
                    } else {
                        renderDynamicReport(scope, element);
                    }

                    scope.dynamicReportOptions.IsUpdateGrid = false;
                });
            }
        };
    });
})();