"use strict";
/* global statisticReportApplication,
crmConstants,
sharedScriptManager,
viewResources */
/* jshint unused: vars */
/* jshint camelcase: false */
/* jshint maxstatements: 1000 */
statisticReportApplication.directive('tableStatisticReportContainer', function () {
    var cachedData = {
        scope: null,
        statisticReport: null,
        table: null,
        gridOptions: null
    };

    function getGridHeight() {
        return cachedData.scope.isShowFilter ?
            230 + crmConstants.MarginBottomPage :
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
                        reportByCountryInfo = timeReport.ChildReports[k];
                        columns.push({
                            Name: "ColCountry_" + reportByCountryInfo.CountryInfo.Id,
                            Value: reportByCountryInfo.HasData ? reportByCountryInfo.NumberOfIssues : ""
                        });
                    }

                    columns.push({
                        Name: "ColTotal",
                        Value: timeReport.NumberOfIssues
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
                    reportByCountryInfo = total.ChildReports[k];
                    columns.push({
                        Name: "ColCountry_" + reportByCountryInfo.CountryInfo.Id,
                        Value: reportByCountryInfo.NumberOfIssues
                    });
                }

                columns.push({ Name: "ColTotal", Value: total.NumberOfIssues });
                bodyData.push({
                    Columns: columns,
                    CssClass: 'subtotalrow'
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
                var countryInfo = countryList[i];
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

        var viewChartIcon = '<input id="button-viewchart" class="btnCharting" type="button" value="">';
        var gridOptions = {
            columns: [].concat(
                 [{ HeaderText: viewResources.Date, Width: 140, Name: "ColDate", HeaderAlign: "center", CellAlign: "left" },
                  { HeaderText: viewResources.Time, Width: dateReportLength ? 80 : 120, Name: "ColTime", HeaderAlign: "center", CellAlign: "center"}],
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
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: true,
            gridExpandHeight: getGridHeight(),
            showDistinctRow: false,
            showHighlightRow: false,
            headerColumnHeight: 29,
            bodyRowHeight: 29,
            showPager: true,
            showTree: true,
            treeParentPaddingLeft: 5,
            customPager: true,
            pagerOption: {
                itemsPerPage: cachedData.scope.lastSelectedParams.PageSize,
                currentPage: cachedData.scope.lastSelectedParams.PageIndex,
                itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                totalItem: statisticReport ? statisticReport.TotalChildReports : 0,
                pagerSelectCallBack: ReRenderAfterPaged
            }
        };

        return gridOptions;
    }

    function getCountryList(statisticReport) {
        var countryList = [];
        var firstDateReport = statisticReport.ChildReports[0];
        var firstTimeReport = firstDateReport.ChildReports[0];
        var countryLength = firstTimeReport.ChildReports.length;
        for (var k = 0; k < countryLength; k++) {
            var reportByCountryInfo = firstTimeReport.ChildReports[k];
            var country = reportByCountryInfo.CountryInfo;
            countryList.push(country);
        }

        return countryList;
    }

    function renderStatisticReport(scope, element, attrs) {
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
            viewChart(scope, element, attrs);
        });
    }

    function ReRenderAfterPaged(page_index, itemsPerPage, jq) {
        cachedData.scope.pagingStatisticReport(page_index, itemsPerPage);
        return false;
    }

    function viewChart(scope, element, attrs) {
        var chartInfo = cachedData.statisticReport ? cachedData.statisticReport.ChartInfo : null;
        parent.crmChart.title = chartInfo.Name;
        var xAxisCategories = [];
        var chartSeries = [];
        var dateReportLength = chartInfo ? chartInfo.ChildReports.length : 0;

        var rowIndex = 0;
        for (var xIndex = 0; xIndex < dateReportLength; xIndex++) {
            var dateReport = chartInfo.ChildReports[xIndex];

            var timeLength = dateReport.ChildReports.length;
            for (var timeIndex = 0; timeIndex < timeLength; timeIndex++, rowIndex++) {
                var timeReport = dateReport.ChildReports[timeIndex];
                xAxisCategories.push(sharedScriptManager.stringFormat("{0} - {1}",
                    [dateReport.ShortName, timeReport.ShortName]));

                var countryLength = timeReport.ChildReports.length;
                for (var countryIndex = 0; countryIndex < countryLength; countryIndex++) {
                    var countryReport = timeReport.ChildReports[countryIndex];
                    if (chartSeries[countryIndex] === undefined ||
                        chartSeries[countryIndex] === null) {
                        chartSeries[countryIndex] = {
                            name: countryReport.CountryInfo.Name,
                            data: []
                        };
                    }

                    chartSeries[countryIndex].data[rowIndex] = countryReport.NumberOfIssues;
                }
            }
        }

        var chartOptions = {
            xAxis: {
                categories: xAxisCategories
            },
            series: chartSeries,
            tooltip: {
                valueSuffix: viewResources.StatisticReport_ChartSuffix
            }
        };

        parent.crmChart.viewChart(chartOptions);
    }

    return {
        restrict: 'C',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (oldval, newval) {
                if (!scope.statisticReportOptions.IsUpdateGrid) {
                    return;
                }

                if (scope.statisticReportOptions.IsResize && cachedData.table) {
                    cachedData.table.fanexGrid("resizeHeight", getGridHeight());
                    scope.statisticReportOptions.IsResize = false;
                } else {
                    renderStatisticReport(scope, element, attrs);
                }

                scope.statisticReportOptions.IsUpdateGrid = false;
            });
        }
    };
});