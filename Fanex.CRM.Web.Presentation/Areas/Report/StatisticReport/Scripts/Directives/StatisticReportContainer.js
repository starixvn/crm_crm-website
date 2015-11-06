(function () {
    "use strict";
    /* global crmConstants,
    sharedScriptManager,
    viewResources */
    /* jshint unused: vars */
    /* jshint camelcase: false */
    /* jshint maxstatements: 1000 */
    angular.module('report-statisticreport-application').directive('tableStatisticReportContainer', function () {
        var cachedData = {
            scope: null,
            statisticReport: null,
            table: null,
            gridOptions: null
        };

        function getGridHeight() {
            return cachedData.scope.isShowFilter ?
             cachedData.scope.isShowResultMessage ?
                170 + crmConstants.MarginBottomPage :
                150 + crmConstants.MarginBottomPage :
            50 + crmConstants.MarginBottomPage;
        }

        function getGripOtions(statisticReport) {
            var bodyData = [];
            var footerData = [];
            var dateReportLength = statisticReport ? statisticReport.ChildReports.length : 0;
            var countryColumns = [];
            if (dateReportLength > 0) {
                var countryList = getCountryList(statisticReport);
                var countryLength;
                var i;
                var k;
                var columns;
                var reportByCountryInfo;
                var total;
                var countryInfo;
                for (i = 0; i < dateReportLength; i++) {
                    var dateReport = statisticReport.ChildReports[i];
                    var timeReportLength = dateReport.ChildReports.length;
                    for (var j = 0; j < timeReportLength; j++) {
                        var timeReport = dateReport.ChildReports[j];
                        countryLength = timeReport.ChildReports.length;
                        columns = [];
                        if (j === 0) {
                            columns.push({
                                Name: "ColDate",
                                Value: dateReport.ShortName,
                                MergeRowNumber: dateReport.ChildReports.length,
                                CellAlign: 'center'
                            });
                        }

                        columns.push({
                            Name: "ColTime",
                            Value: timeReport.ShortName
                        });

                        for (k = 0; k < countryLength; k++) {
                            countryInfo = countryList[k];
                            reportByCountryInfo = timeReport.ChildReports[k];
                            columns.push({
                                Name: "ColCountry_" + countryInfo.Id,
                                Value: reportByCountryInfo.HasData ? reportByCountryInfo.NumberOfIssues : ""
                            });
                        }

                        columns.push({
                            Name: "ColTotal",
                            Value: timeReport.NumberOfIssues,
                            CssClass: "font-weight-bold"
                        });

                        bodyData.push({
                            Columns: columns
                        });
                    }

                    total = dateReport.Total;
                    countryLength = total.ChildReports.length;
                    columns = [];
                    columns.push({ Name: "ColDate", Value: total.ShortName });
                    columns.push({ Name: "ColTime", Value: '' });
                    for (k = 0; k < countryLength; k++) {
                        countryInfo = countryList[k];
                        reportByCountryInfo = total.ChildReports[k];
                        columns.push({
                            Name: "ColCountry_" + countryInfo.Id,
                            Value: reportByCountryInfo.NumberOfIssues
                        });
                    }

                    columns.push({ Name: "ColTotal", Value: total.NumberOfIssues });
                    bodyData.push({
                        Columns: columns,
                        CssClass: 'subtotalrow font-weight-bold'
                    });
                }

                total = statisticReport.Total;
                countryLength = total.ChildReports.length;
                columns = [];
                columns.push({ Name: "ColDate", Value: total.ShortName });
                columns.push({ Name: "ColTime", Value: '' });
                for (k = 0; k < countryLength; k++) {
                    reportByCountryInfo = total.ChildReports[k];
                    columns.push({
                        Name: "ColCountry_" + reportByCountryInfo.CountryInfo.Id,
                        Value: reportByCountryInfo.NumberOfIssues
                    });
                }

                columns.push({ Name: "ColTotal", Value: total.NumberOfIssues });
                footerData.push({
                    Columns: columns
                });

                countryLength = countryList.length;
                for (i = 0; i < countryLength; i++) {
                    countryInfo = countryList[i];
                    countryColumns.push({
                        HeaderText: countryInfo.Name,
                        Width: 80,
                        Name: "ColCountry_" + countryInfo.Id,
                        HeaderAlign: "center",
                        CellAlign: "right",
                        FormatNumber: crmConstants.Enums.FormatConstant.IntNumber
                    });
                }
            }

            var itemsPerPageArray = getItemsPerPageArray();
            var viewChartIcon = '<input id="button-viewchart" class="btnCharting" type="button" value="">';
            var gridOptions = {
                columns: [].concat(
                 [{ HeaderText: viewResources.Date, Width: 180, Name: "ColDate", HeaderAlign: "center", CellAlign: "left" },
                  { HeaderText: viewResources.Time, Width: dateReportLength ? 120 : 120, Name: "ColTime", HeaderAlign: "center", CellAlign: "center"}],
                  countryColumns,
                  [{
                      HeaderText: viewResources.Total,
                      Width: dateReportLength ? 80 : 120,
                      Name: "ColTotal",
                      HeaderAlign: "center",
                      CellAlign: "right",
                      FormatNumber: crmConstants.Enums.FormatConstant.IntNumber
                  }]),
                showTitle: true,
                showFooter: true,
                titleInfo: {
                    text: statisticReport ? statisticReport.Name : "",
                    showExcelButton: true,
                    exportExcelFunction: function () {
                        cachedData.scope.exportReport();
                    },
                    customButtons: viewChartIcon
                },
                bodyRows: bodyData,
                footerRows: footerData,
                numberOfFrozenColumn: 2,
                fullWidth: true,
                fullHeight: true,
                gridExpandHeight: getGridHeight(),
                showDistinctRow: false,
                showHighlightRow: false,
                headerColumnHeight: 29,
                autoFixFrozenRowHeight: true,
                bodyRowHeight: 29,
                showPager: parseInt(cachedData.scope.searchCriteria.ReportType.Id) === crmConstants.Enums.PeriodType.Daily,
                showTree: true,
                treeParentPaddingLeft: 5,
                customPager: true,
                pagerOption: {
                    itemsPerPage: cachedData.scope.lastSelectedParams.PageSize,
                    currentPage: cachedData.scope.lastSelectedParams.PageIndex,
                    itemsPerPageArray: itemsPerPageArray,
                    totalItem: statisticReport ? statisticReport.TotalChildReports : 0,
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };

            return gridOptions;
        }

        function getItemsPerPageArray() {
            var reportTypeId = parseInt(cachedData.scope.searchCriteria.ReportType.Id);
            switch (reportTypeId) {
                case crmConstants.Enums.PeriodType.Daily:
                    return [1, 5, 7, 10];
                case crmConstants.Enums.PeriodType.Monthly:
                    return [3, 4, 6, 12];
                default:
                    return [1, 5, 7, 10];
            }
        }

        function getCountryList(statisticReport) {
            var countryList = [];
            var countryLength = statisticReport.Total.ChildReports.length;
            for (var k = 0; k < countryLength; k++) {
                var reportByCountryInfo = statisticReport.Total.ChildReports[k];
                var country = reportByCountryInfo.CountryInfo;
                countryList.push(country);
            }

            return countryList;
        }

        function renderStatisticReport(scope, element) {
            if (scope.statisticReportOptions.DataList === null) {
                return;
            }

            cachedData.scope = scope;
            var tableStatisticReport = element.find("#table-statistic-report");
            cachedData.table = tableStatisticReport;
            var statisticReport = scope.statisticReportOptions.DataList;
            cachedData.statisticReport = statisticReport;
            var gridOptions = getGripOtions(statisticReport);
            cachedData.gridOptions = gridOptions;
            tableStatisticReport.empty().removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
            element.find("#button-viewchart").click(function () {
                viewChart(scope, element);
            });
        }

        function ReRenderAfterPaged(page_index, itemsPerPage) {
            cachedData.scope.pagingStatisticReport(page_index, itemsPerPage);
            return false;
        }

        function viewChart(scope, element) {
            /* jshint unused: false */
            var crmChart = parent.crmChart;
            var chartInfo = cachedData.statisticReport ? cachedData.statisticReport.ChartInfo : null;
            crmChart.title = chartInfo.Name;

            var chartData = {
                labels: getChartLabels(chartInfo),
                datasets: getChartDatasets(chartInfo, crmChart, cachedData.statisticReport)
            };

            var chartOption = angular.copy(crmChart.newopts);
            chartOption = $.extend(chartOption, {
                chartType: crmChart.chartType.bar,
                scaleFontSize: 9,
                yAxisUnit: viewResources.StatisticReport_ChartSuffix
            });

            crmChart.viewChart(chartData, chartOption);
            parent.$(".ui-dialog-titlebar-close").click(function () {
                scope.setFocus();
            });
        }

        function getChartLabels(chartInfo) {
            var xAxisCategories = [];
            var dateReportLength = chartInfo ? chartInfo.ChildReports.length : 0;

            var rowIndex = 0;
            for (var xIndex = 0; xIndex < dateReportLength; xIndex++) {
                var dateReport = chartInfo.ChildReports[xIndex];
                var timeLength = dateReport.ChildReports.length;

                for (var timeIndex = 0; timeIndex < timeLength; timeIndex++, rowIndex++) {
                    var timeReport = dateReport.ChildReports[timeIndex];
                    xAxisCategories.push(sharedScriptManager.stringFormat("{0} - {1}",
                    [dateReport.ShortName, timeReport.ShortName]));

                }
            }

            return xAxisCategories;
        }
        function getChartDatasets(chartInfo, crmChart, statisticReport) {
            var countryList = getCountryList(statisticReport);
            var chartSeries = [];
            var dateReportLength = chartInfo ? chartInfo.ChildReports.length : 0;

            var rowIndex = 0;
            for (var xIndex = 0; xIndex < dateReportLength; xIndex++) {
                var dateReport = chartInfo.ChildReports[xIndex];

                var timeLength = dateReport.ChildReports.length;
                for (var timeIndex = 0; timeIndex < timeLength; timeIndex++, rowIndex++) {
                    var timeReport = dateReport.ChildReports[timeIndex];
                    var countryLength = timeReport.ChildReports.length;
                    for (var countryIndex = 0; countryIndex < countryLength; countryIndex++) {
                        var countryReport = timeReport.ChildReports[countryIndex];
                        if (chartSeries[countryIndex] === undefined ||
                        chartSeries[countryIndex] === null) {
                            var backgroundColor = crmChart.getBackGroundColor(countryIndex);
                            chartSeries[countryIndex] = {
                                fillColor: backgroundColor,
                                strokeColor: backgroundColor,
                                data: [],
                                title: countryList[countryIndex].Name
                            };
                        }

                        chartSeries[countryIndex].data[rowIndex] = countryReport.NumberOfIssues;
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
                    /* jshint unused: false */
                    if (!scope.statisticReportOptions.IsUpdateGrid) {
                        return;
                    }

                    if (scope.statisticReportOptions.IsResize && cachedData.table) {
                        cachedData.table.fanexGrid("resizeHeight", getGridHeight());
                        scope.statisticReportOptions.IsResize = false;
                    } else {
                        renderStatisticReport(scope, element);
                    }

                    scope.statisticReportOptions.IsUpdateGrid = false;
                });
            }
        };
    });
})();