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

    function getGridOptions(scope, reportTitle, bodyData, footerData) {
        return {
            columns: [
                    {
                        HeaderText: summaryReportResources.Categories,
                        Width: 300,
                        Name: 'ColCategory',
                        HeaderAlign: 'center',
                        CellAlign: 'left',
                        ShowCollapseExpand: true 
                    },
                    getMonthColumnTitle(summaryReportResources.Jan, 'ColJAN'),
                    getMonthColumnTitle(summaryReportResources.Feb, 'ColFEB'),
                    getMonthColumnTitle(summaryReportResources.Mar, 'ColMAR'),
                    getMonthColumnTitle(summaryReportResources.Apr, 'ColAPR'),
                    getMonthColumnTitle(summaryReportResources.May, 'ColMAY'),
                    getMonthColumnTitle(summaryReportResources.Jun, 'ColJUN'),
                    getMonthColumnTitle(summaryReportResources.Jul, 'ColJUL'),
                    getMonthColumnTitle(summaryReportResources.Aug, 'ColAUG'),
                    getMonthColumnTitle(summaryReportResources.Sep, 'ColSEP'),
                    getMonthColumnTitle(summaryReportResources.Oct, 'ColOCT'),
                    getMonthColumnTitle(summaryReportResources.Nov, 'ColNOV'),
                    getMonthColumnTitle(summaryReportResources.Dec, 'ColDEC')],
            showTitle: true,
            titleInfo: {
                text: reportTitle,
                showCollapseExpandButton: true,
                showExcelButton: true,
                exportExcelFunction: function () {
                    scope.exportSummaryReportExcel();
                }
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
            showHighlightRow: false,
            headerColumnHeight: 29,
            bodyRowHeight: 29
        };
    }

    function getGridBody(categorySummaries) {
        var bodyData = [];
        if (categorySummaries) {
            var category, categoryTotal, subCategory, summary;
            var categoryLen, categoryTotalLen, subLen, currentRow, currentSubRow;
            var evenClass = '';

            categoryLen = categorySummaries.length;
            for (var i = 0; i < categoryLen; i++) {
                evenClass = (i % 2 === 0) ? '' : 'freegrid-even';
                category = categorySummaries[i];
                currentRow = {
                    Columns: [{ Name: 'ColCategory', Value: category.CategoryName}],
                    SubRows: []
                };

                categoryTotalLen = category.Total ? category.Total.length : 0;
                for (var l = 0; l < categoryTotalLen; l++) {
                    categoryTotal = category.Total[l];
                    currentRow.Columns.push({ Name: 'Col' + categoryTotal.Month, Value: categoryTotal.Amount === 0 ? '' : categoryTotal.Amount });
                }

                subLen = category.SubCategorySummaries ? category.SubCategorySummaries.length : 0;
                for (var j = 0; j < subLen; j++) {
                    subCategory = category.SubCategorySummaries[j];
                    currentSubRow = {
                        Columns: [
                            { Name: 'ColCategory', Value: subCategory.SubCategoryName}]
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

    function getGridFooter(grandTotal) {
        var footerData = [];

        var monthLen = grandTotal.Summaries ? grandTotal.Summaries.length : 0;
        var cell = [{ Name: 'ColCategory', Value: grandTotal.SubCategoryName}];
        var summary;
        for (var i = 0; i < monthLen; i++) {
            summary = grandTotal.Summaries[i];
            cell.push({ Name: 'Col' + summary.Month, Value: summary.Amount === 0 ? '' : summary.Amount });
        }

        footerData.push({ Columns: cell });
        return footerData;
    }

    function renderSummaryReport(scope, element, attrs) {
        if (!scope.summaryReport) {
            return;
        }

        var reportTitle = scope.summaryReport.ReportTitle;
        var categorySummaries = scope.summaryReport.CategorySummaries;
        var grandTotal = scope.summaryReport.GrandTotal;

        var tableSummaryReport = element.find('#table-summary-report').empty();

        var bodyData = getGridBody(categorySummaries);
        var footerData = getGridFooter(grandTotal);

        var gridOptions = getGridOptions(scope, reportTitle, bodyData, footerData);
        tableSummaryReport
            .removeClass(crmConstants.Enums.CssClass.DisplayNone)
            .fanexGrid(gridOptions);
    }

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (newValues, oldValues) {
                renderSummaryReport(scope, element, attrs);
            });
        }
    };
});