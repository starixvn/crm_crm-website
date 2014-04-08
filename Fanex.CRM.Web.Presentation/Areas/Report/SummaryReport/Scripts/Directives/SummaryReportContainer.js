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
            Width: 50,
            Name: name,
            HeaderAlign: 'center',
            CellAlign: 'right',
            FormatNumber: crmConstants.Enums.FormatConstant.IntNumber
        };
    }

    function getGridOptions(scope, reportTitle, bodyData, footerData) {
        return {
            columns: [{ HeaderText: summaryReportResources.Categories, Width: 400, Name: 'ColCategory', HeaderAlign: 'left', CellAlign: 'left' },
                    getMonthColumnTitle(summaryReportResources.Jan, 'ColJan'),
                    getMonthColumnTitle(summaryReportResources.Feb, 'ColFeb'),
                    getMonthColumnTitle(summaryReportResources.Mar, 'ColMar'),
                    getMonthColumnTitle(summaryReportResources.Apr, 'ColApr'),
                    getMonthColumnTitle(summaryReportResources.May, 'ColMay'),
                    getMonthColumnTitle(summaryReportResources.Jun, 'ColJun'),
                    getMonthColumnTitle(summaryReportResources.Jul, 'ColJul'),
                    getMonthColumnTitle(summaryReportResources.Aug, 'ColAug'),
                    getMonthColumnTitle(summaryReportResources.Sep, 'ColSep'),
                    getMonthColumnTitle(summaryReportResources.Oct, 'ColOct'),
                    getMonthColumnTitle(summaryReportResources.Nov, 'ColNov'),
                    getMonthColumnTitle(summaryReportResources.Dec, 'ColDec')],
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
            footerRows: footerData,
            numberOfFrozenColumn: 0,
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
            var category, subCategory, subLen, cell;
            var evenClass = '';
            for (var i = 0, max = categorySummaries.length; i < max; i++) {
                evenClass = (i % 2 === 0) ? '' : 'freegrid-even';
                category = categorySummaries[i];
                cell = {
                    Columns: [{ Name: 'ColCategory', Value: info.CategoryName, MergeColNumber: 12 }],
                    SubRows: []
                }

                subLen = info.SubCategorySummaries ? info.SubCategorySummaries.length : 0;
                for (var j = 0; j < subLen; j++) {
                    cell.SubRows.push();
                }

                bodyData.push({ Columns: cell, CssClass: evenClass });
            }
        }

        return bodyData;
    }

    function getGridFooter(grandTotal) {
        var footerData = [];

        var monthLen = grandTotal.Months.length;
        var cell = [{ Name: 'ColCategory', Value: grandTotal.CategoryName }];
        for (var i = 0; i < monthLen; i++) {
            cell.push({ Name: 'Col' + grandTotal.Months[i].Value, Value: grandTotal.Months[i].Amount });
        }

        footerData.push({ Columns: cell });
        return footerData;
    }

    function renderSummaryReport(scope, element, attrs) {
        if (!scope.summaryReport) {
            return;
        }

        var reportTitle = scope.summaryReport.ReportTitle;
        var categorySummaries = scope.summaryReport.categorySummaries;
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