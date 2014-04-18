"use strict";
/* global settlementReportApplication,
crmConstants,
sharedScriptManager,
viewResources */
/* jshint unused: vars */
/* jshint camelcase: false */
/* jshint maxstatements: 1000 */
settlementReportApplication.directive('tableSettlementReportContainer', function () {
    var cachedData = {
        scope: null,
        settlementReport: null,
        table: null,
        gridOptions: null
    };

    function getGripOtions(settlementReport) {
        var bodyData = [];
        var footerData = [];
        var monthReportLength = settlementReport ? settlementReport.ChildReports.length : 0;
        if (monthReportLength > 0) {
            var i;
            var j;
            var columns;
            var countryReportLength;
            var countryReport;
            for (i = 0; i < monthReportLength; i++) {
                var isSetEvenRowColor = false;
                var monthReport = settlementReport.ChildReports[i];
                countryReportLength = monthReport.ChildReports.length;
                for (j = 0; j < countryReportLength; j++) {
                    countryReport = monthReport.ChildReports[j];
                    columns = [];
                    if (j === 0) {
                        columns.push({
                            Name: "ColMonth",
                            Value: monthReport.Name.toUpperCase(),
                            MergeRowNumber: monthReport.ChildReports.length,
                            CellAlign: 'center'
                        });
                    }

                    columns.push({
                        Name: "ColCountry",
                        Value: countryReport.Name
                    });

                    columns.push({
                        Name: "ColAmount",
                        Value: countryReport.NumberOfIssues
                    });

                    if (j === 0) {
                        columns.push({
                            Name: "ColTotal",
                            Value: monthReport.NumberOfIssues,
                            MergeRowNumber: monthReport.ChildReports.length,
                            CellAlign: 'center'
                        });
                    }

                    bodyData.push({
                        Columns: columns,
                        CssClass: isSetEvenRowColor ? "freegrid-even" : ""
                    });
                }
            }

            var total = settlementReport.Total;
            countryReportLength = total.ChildReports.length;
            for (j = 0; j < countryReportLength; j++) {
                countryReport = total.ChildReports[j];
                columns = [];
                if (j === 0) {
                    columns.push({
                        Name: "ColMonth",
                        Value: total.Name,
                        MergeRowNumber: total.ChildReports.length,
                        CellAlign: 'center'
                    });
                }

                columns.push({
                    Name: "ColCountry",
                    Value: countryReport.Name
                });

                columns.push({
                    Name: "ColAmount",
                    Value: countryReport.NumberOfIssues
                });

                if (j === 0) {
                    columns.push({
                        Name: "ColTotal",
                        Value: total.NumberOfIssues,
                        MergeRowNumber: total.ChildReports.length,
                        CellAlign: 'center'
                    });
                }

                footerData.push({
                    Columns: columns
                });
            }
        }

        var viewChartIcon = sharedScriptManager.cachedData.viewChartIcon;
        var gridOptions = {
            columns: [{ HeaderText: viewResources.Month, Width: 80, Name: "ColMonth", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Country, Width: 160, Name: "ColCountry", HeaderAlign: "center", CellAlign: "left" },
                {
                    HeaderText: viewResources.Amount,
                    Width: 160,
                    Name: "ColAmount",
                    HeaderAlign: "center",
                    CellAlign: "right",
                    FormatNumber: crmConstants.Enums.FormatConstant.IntNumber
                },
                {
                    HeaderText: viewResources.Total,
                    Width: 160,
                    Name: "ColTotal",
                    HeaderAlign: "center",
                    CellAlign: "right",
                    FormatNumber: crmConstants.Enums.FormatConstant.IntNumber
                }],
            showTitle: true,
            showFooter: true,
            titleInfo: {
                text: settlementReport ? settlementReport.Name : "",
                showExcelButton: true,
                exportExcelFunction: function () {
                    cachedData.scope.exportReport();
                },
                customButtons: viewChartIcon
            },
            bodyRows: bodyData,
            footerRows: footerData,
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: true,
            gridExpandHeight: 100,
            showDistinctRow: false,
            showHighlightRow: false,
            headerColumnHeight: 29,
            bodyRowHeight: 29,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: cachedData.scope.lastSelectedParams.PageSize,
                currentPage: cachedData.scope.lastSelectedParams.PageIndex,
                itemsPerPageArray: [1, 5, 7, 10],
                totalItem: settlementReport ? settlementReport.TotalChildReports : 0,
                pagerSelectCallBack: ReRenderAfterPaged
            }
        };

        return gridOptions;
    }

    function ReRenderAfterPaged(page_index, itemsPerPage, jq) {
        cachedData.scope.pagingSettlementReport(page_index, itemsPerPage);
        return false;
    }

    function renderSettlementReport(scope, element, attrs) {
        if (scope.settlementReportOptions.DataList === null) {
            return;
        }

        cachedData.scope = scope;
        var tableSettlementReport = element.find("#table-settlement-report");
        cachedData.table = tableSettlementReport;
        var settlementReport = scope.settlementReportOptions.DataList;
        cachedData.settlementReport = settlementReport;
        var gridOptions = getGripOtions(settlementReport);
        cachedData.gridOptions = gridOptions;
        tableSettlementReport.empty().removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
        element.find("#button-viewchart").click(function () {
            viewChart(scope, element, attrs);
        });
    }

    function viewChart(scope, element, attrs) {
        var crmChart = parent.crmChart;
        var chartInfo = cachedData.settlementReport ? cachedData.settlementReport.ChartInfo : null;
        crmChart.title = chartInfo.Name;

        var chartData = {
            labels: getChartLabels(chartInfo, crmChart),
            datasets: getChartDatasets(chartInfo, crmChart)
        };

        var chartOption = angular.copy(crmChart.newopts);
        chartOption = $.extend(chartOption, {
            chartType: crmChart.chartType.bar,
            yAxisUnit: viewResources.SettlementReport_ChartSuffix
        });

        crmChart.viewChart(chartData, chartOption);
        parent.$(".ui-dialog-titlebar-close").click(function () {
            scope.setFocus();
        });
    }

    function getChartLabels(chartInfo, crmChart) {
        var xAxisCategories = [];
        var monthReportLength = chartInfo ? chartInfo.ChildReports.length : 0;
        var xIndex;
        for (xIndex = 0; xIndex < monthReportLength; xIndex++) {
            var monthReport = chartInfo.ChildReports[xIndex];
            xAxisCategories.push(monthReport.Name);
        }

        return xAxisCategories;
    }

    function getChartDatasets(chartInfo, crmChart) {
        var chartSeries = [];
        var monthReportLength = chartInfo ? chartInfo.ChildReports.length : 0;
        var countryDictionary = [];
        var currentCountryIndex = 0;
        var countryIndex;
        var xIndex;
        for (xIndex = 0; xIndex < monthReportLength; xIndex++) {
            var monthReport = chartInfo.ChildReports[xIndex];
            var countryLength = monthReport.ChildReports.length;
            for (countryIndex = 0; countryIndex < countryLength; countryIndex++) {
                var countryReport = monthReport.ChildReports[countryIndex];
                if (countryDictionary[countryReport.Name] === undefined ||
                    countryDictionary[countryReport.Name] === null) {
                    countryDictionary[countryReport.Name] = currentCountryIndex;
                    currentCountryIndex++;
                }

                var chartSerieIndex = countryDictionary[countryReport.Name];
                if (chartSeries[chartSerieIndex] === undefined ||
                    chartSeries[chartSerieIndex] === null) {
                    var backgroundColor = crmChart.getBackGroundColor(chartSerieIndex);
                    chartSeries[chartSerieIndex] = {
                        fillColor: backgroundColor,
                        strokeColor: backgroundColor,
                        data: [],
                        title: countryReport.Name
                    };
                }

                chartSeries[chartSerieIndex].data[xIndex] = countryReport.NumberOfIssues;
            }
        }

        var chartSeriesLength = chartSeries.length;
        for (countryIndex = 0; countryIndex < chartSeriesLength; countryIndex++) {
            var chartSerie = chartSeries[countryIndex];
            for (xIndex = 0; xIndex < monthReportLength; xIndex++) {
                if (chartSerie.data[xIndex] === undefined) {
                    chartSeries[countryIndex].data[xIndex] = 0;
                }
            }
        }

        return chartSeries;
    }

    return {
        restrict: 'C',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (oldval, newval) {
                if (!scope.settlementReportOptions.IsUpdateGrid) {
                    return;
                }

                renderSettlementReport(scope, element, attrs);
                scope.settlementReportOptions.IsUpdateGrid = false;
            });
        }
    };
});