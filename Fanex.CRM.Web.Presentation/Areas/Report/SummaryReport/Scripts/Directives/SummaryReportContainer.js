'use strict';
/* global summaryReportApplication,
summaryReportResources,
crmConstants */
/* jshint camelcase: false */
/* jshint unused: vars */
summaryReportApplication.directive('tableSummaryReportContainer', function () {
    var cachedTable;

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

    function getGridHeight(scope) {
        var marginBottom = crmConstants.MarginBottomPage;
        return scope.isShowFilter ? (400 + marginBottom) : (50 + marginBottom);
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
                }
            },
            bodyRows: bodyData,
            showFooter: true,
            showTree: true,
            footerRows: footerData,
            numberOfFrozenColumn: 1,
            fullWidth: true,
            fullHeight: true,
            gridExpandHeight: getGridHeight(scope),
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 29
        };
    }

    function getGridBody(categorySummaries) {
        var bodyData = [];
        if (categorySummaries) {
            var category, categoryTotal, subCategory, summary;
            var categoryLen, categoryTotalLen, subLen, currentRow, currentSubRow;

            categoryLen = categorySummaries.length;
            for (var i = 0; i < categoryLen; i++) {
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
        var months = scope.summaryReport.Months;

        var bodyData = getGridBody(categorySummaries);
        var footerData = getGridFooter(grandTotal);

        var gridOptions = getGridOptions(scope, reportTitle, bodyData, footerData, months);
        cachedTable = element
            .find('#table-summary-report')
            .empty()
            .removeClass(crmConstants.Enums.CssClass.DisplayNone)
            .fanexGrid(gridOptions);
    }

    return {
        restrict: 'A',
        scope: {
            summaryReport: '=',
            isShowFilter: '=',
            isRenderGrid: '=',
            isFilterBoxClicked: '=',
            exportSummaryReportExcel: '&'
        },
        link: function (scope, element, attrs) {
            scope.$watch('isRenderGrid', function (newValue, oldValue) {
                if (!newValue) {
                    return;
                }

                scope.isRenderGrid = false;
                if (scope.isFilterBoxClicked && cachedTable) {
                    cachedTable.fanexGrid('resizeHeight', getGridHeight(scope));
                    scope.isFilterBoxClicked = false;
                    return;
                }

                renderSummaryReport(scope, element, attrs);
            });
        }
    };
});