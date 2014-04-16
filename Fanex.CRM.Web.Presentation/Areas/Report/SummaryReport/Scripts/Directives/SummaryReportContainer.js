'use strict';
/* global summaryReportApplication,
summaryReportResources,
crmConstants */
/* jshint camelcase: false */
/* jshint unused: vars */
summaryReportApplication.directive('tableSummaryReportContainer', function () {
    function getMonthColumnTitle(headerText, name) {
        return {
            HeaderText: headerText,
            Width: 72,
            Name: name,
            HeaderAlign: 'center',
            CellAlign: 'right',
            FormatNumber: crmConstants.Enums.FormatConstant.IntNumber
        };
    }

    function getGridOptions(scope, reportTitle, bodyData, footerData, months) {
        var columns = [{
            HeaderText: summaryReportResources.Categories,
            Width: 300,
            Name: 'ColCategory',
            HeaderAlign: 'center',
            CellAlign: 'left',
            ShowCollapseExpand: true
        }];

        for (var i = 0, leng = months.length; i < leng; i++) {
            columns.push(getMonthColumnTitle(months[i], 'Col' + months[i]));
        }

        return {
            columns: columns,
            showTitle: true,
            titleInfo: {
                text: reportTitle,
                showCollapseExpandButton: true,
                showExcelButton: true,
                exportExcelFunction: function () {
                    scope.exportSummaryReportExcel();
                },
                customButtons: '<input id="button-viewchart" class="btnCharting" type="button" />'
            },
            bodyRows: bodyData,
            showFooter: true,
            showTree: true,
            footerRows: footerData,
            numberOfFrozenColumn: 1,
            fullWidth: true,
            fullHeight: true,
            gridExpandHeight: 100,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 29
        };
    }

    function getGridBody(scope, categorySummaries) {
        var bodyData = [];
        if (categorySummaries) {
            var category, categoryTotal, subCategory, summary;
            var categoryLen, categoryTotalLen, subLen, currentRow, currentSubRow;

            categoryLen = categorySummaries.length;
            for (var i = 0; i < categoryLen; i++) {
                category = categorySummaries[i];
                currentRow = {
                    Columns: [{ Name: 'ColCategory', Value: category.Name}],
                    SubRows: []
                };

                // chart
                categoryTotalLen = category.Summaries ? category.Summaries.length : 0;
                for (var l = 0; l < categoryTotalLen; l++) {
                    categoryTotal = category.Summaries[l];
                    currentRow.Columns.push({ Name: 'Col' + categoryTotal.Month, Value: categoryTotal.Amount === 0 ? '' : categoryTotal.Amount });
                }

                subLen = category.SubCategories ? category.SubCategories.length : 0;
                for (var j = 0; j < subLen; j++) {
                    subCategory = category.SubCategories[j];
                    currentSubRow = {
                        Columns: [
                            { Name: 'ColCategory', Value: subCategory.Name}]
                    };

                    var summaryLen = subCategory.Summaries ? subCategory.Summaries.length : 0;
                    for (var k = 0; k < summaryLen; k++) {
                        summary = subCategory.Summaries[k];
                        currentSubRow.Columns.push({ Name: 'Col' + summary.Month, Value: summary.Amount === 0 ? '' : summary.Amount });
                    }

                    currentRow.SubRows.push(currentSubRow);
                }

                bodyData.push(currentRow);
            }
        }

        return bodyData;
    }

    function getGridFooter(grandTotals) {
        var footerData = [];

        var grandTotalLen = grandTotals ? grandTotals.length : 0;
        var cell = [{ Name: 'ColCategory', Value: summaryReportResources.GrandTotal}];
        var summary;
        for (var i = 0; i < grandTotalLen; i++) {
            summary = grandTotals[i];
            cell.push({ Name: 'Col' + summary.Month, Value: summary.Amount === 0 ? '' : summary.Amount });
        }

        footerData.push({ Columns: cell });
        return footerData;
    }

    function renderSummaryReport(scope, element, attrs) {
        var report = scope.summaryReport;
        if (!report) {
            return;
        }

        var reportTitle = report.ReportTitle;
        var categorySummaries = report.CategorySummaries;
        var grandTotals = report.GrandTotals;
        var months = report.Months;

        var bodyData = getGridBody(scope, categorySummaries);
        var footerData = getGridFooter(grandTotals);

        var gridOptions = getGridOptions(scope, reportTitle, bodyData, footerData, months);
        element.find('#table-summary-report')
            .empty()
            .removeClass(crmConstants.Enums.CssClass.DisplayNone)
            .fanexGrid(gridOptions);
        if (parseInt(scope.chartType) === 0) {
            element.find('#btnCollapseExpandtable-summary-report').trigger('click');
        }
        // view chart event.
        element.find('#button-viewchart').click(function () {
            viewChart(scope);
        });
    }

    function getChartDatasets(categorySummaries, typeOfChart) {
        var chartType = parseInt(typeOfChart);
        var datasets = [];

        var bgColor, bgColorIndex = 0;
        var bgColorFunction = parent.crmChart.getBackGroundColor;
        var category, categoryTotal, subCategory, summary, categoryCharts, subCategoryCharts;
        var categoryTotalLen, subLen, summaryLen;
        var categoryLen = categorySummaries.length;
        for (var i = 0; i < categoryLen; i++) {
            category = categorySummaries[i];
            categoryCharts = [];
            categoryTotalLen = category.Summaries ? category.Summaries.length : 0;
            for (var l = 0; l < categoryTotalLen; l++) {
                categoryTotal = category.Summaries[l];
                categoryCharts.push(categoryTotal.Amount);
            }

            if (chartType === 0) { // Summary Chart
                bgColor = bgColorFunction(bgColorIndex++);
                datasets.push({ fillColor: bgColor, strokeColor: bgColor, title: category.Name, data: categoryCharts });
            }

            subLen = category.SubCategories ? category.SubCategories.length : 0;
            for (var j = 0; j < subLen; j++) {
                subCategory = category.SubCategories[j];
                subCategoryCharts = [];
                summaryLen = subCategory.Summaries ? subCategory.Summaries.length : 0;
                for (var k = 0; k < summaryLen; k++) {
                    summary = subCategory.Summaries[k];
                    subCategoryCharts.push(summary.Amount);
                }

                if (chartType === 1) { // Detailed Chart
                    bgColor = bgColorFunction(bgColorIndex++);
                    datasets.push({ fillColor: bgColor, strokeColor: bgColor, title: subCategory.Name, data: subCategoryCharts });
                }
            }
        }

        return datasets;
    }

    function viewChart(scope) {
        var report = scope.summaryReport;
        if (!report) {
            return;
        }

        var chartData = {
            labels: report.Months,
            datasets: getChartDatasets(report.CategorySummaries, scope.chartType)
        };
        var crmChart = parent.crmChart;
        crmChart.title = report.ReportTitle;
        var chartOption = crmChart.newopts;
        chartOption = $.extend(chartOption, {
            chartType: crmChart.chartType.bar,
            xAxisLabel: '',
            yAxisLabel: '',
            yAxisUnit: summaryReportResources.Feedbacks,
            yAxisFontSize: 8,
            scaleFontSize: 12,
            inGraphDataFontSize: 9,
            inGraphDataRotate: 90
        });

        crmChart.viewChart(chartData, chartOption);
    }

    return {
        restrict: 'A',
        scope: {
            summaryReport: '=',
            exportSummaryReportExcel: '&',
            chartType: '='
        },
        link: function (scope, element, attrs) {
            scope.$watch('summaryReport', function (newValue, oldValue) {
                if (!newValue) {
                    return;
                }

                renderSummaryReport(scope, element, attrs);
            });
        }
    };
});