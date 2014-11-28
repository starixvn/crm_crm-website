'use strict';
/* global wagerReportApplication,
wagerReportResources,
crmConstants */
/* jshint camelcase: false */
/* jshint unused: vars */
wagerReportApplication.directive('tableWagerReportContainer', function () {
    function getGridOptions(scope, reportTitle, bodyData, footerData) {
        return {
            columns: [{ HeaderText: wagerReportResources.Month, Width: 200, Name: 'ColMonth', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: wagerReportResources.Wagers, Width: 200, Name: 'ColWagers', HeaderAlign: 'center', CellAlign: 'left' },
                    {
                        HeaderText: wagerReportResources.Amount,
                        Width: 200, Name: 'ColAmount',
                        HeaderAlign: 'center',
                        CellAlign: 'right',
                        FormatNumber: crmConstants.Enums.FormatConstant.IntNumber 
                    },
                    {
                        HeaderText: wagerReportResources.Total,
                        Width: 200,
                        Name: 'ColTotal',
                        HeaderAlign: 'center',
                        CellAlign: 'right',
                        FormatNumber: crmConstants.Enums.FormatConstant.IntNumber 
                    }],
            showTitle: true,
            titleInfo: {
                text: reportTitle,
                showExcelButton: true,
                exportExcelFunction: function () {
                    scope.exportWagerReportExcel();
                }
            },
            bodyRows: bodyData,
            showFooter: true,
            footerRows: footerData,
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: true,
            gridExpandHeight: 120,
            showDistinctRow: false,
            showHighlightRow: false,
            headerColumnHeight: 29
        };
    }

    function getGridBody(wagerByMonths) {
        var bodyData = [];
        if (wagerByMonths) {
            var reportLength = wagerByMonths.length;
            var info, cell;
            var mergeRowNumber = 0;
            for (var i = 0; i < reportLength; i++) {
                info = wagerByMonths[i];
                mergeRowNumber = info.Wagers ? info.Wagers.length : 0;
                if (mergeRowNumber === 0) {
                    continue;
                }

                for (var j = 0; j < mergeRowNumber; j++) {
                    cell = [];
                    if (j === 0) {
                        cell.push({ Name: 'ColMonth', Value: info.Month, MergeRowNumber: mergeRowNumber, CssClass: 'month' });
                    }

                    cell.push({ Name: 'ColWagers', Value: info.Wagers[j].Name });
                    cell.push({ Name: 'ColAmount', Value: info.Wagers[j].Amount });

                    if (j === 0) {
                        cell.push({ Name: 'ColTotal', Value: info.Total, MergeRowNumber: mergeRowNumber, CssClass: 'month' });
                    }

                    bodyData.push({ Columns: cell });
                }
            }
        }

        return bodyData;
    }

    function getGridFooter(grandTotal) {
        var mergeRowNumber = grandTotal.Wagers ? grandTotal.Wagers.length : 0;
        var i, cell;
        var footerData = [];
        for (i = 0; i < mergeRowNumber; i++) {
            cell = [];
            if (i === 0) {
                cell.push({ Name: 'ColMonth', Value: grandTotal.Month, MergeRowNumber: mergeRowNumber, CssClass: 'month' });
            }

            cell.push({ Name: 'ColWagers', Value: grandTotal.Wagers[i].Name });
            cell.push({ Name: 'ColAmount', Value: grandTotal.Wagers[i].Amount });

            if (i === 0) {
                cell.push({ Name: 'ColTotal', Value: grandTotal.Total, MergeRowNumber: mergeRowNumber, CssClass: 'month' });
            }

            footerData.push({ Columns: cell });
        }

        return footerData;
    }

    function renderWagerReport(scope, element, attrs) {
        if (!scope.wagerReport) {
            return;
        }

        var reportTitle = scope.wagerReport.ReportTitle;
        var wagerByMonths = scope.wagerReport.WagerByMonths;
        var grandTotal = scope.wagerReport.GrandTotal;

        var tableWagerReport = element.find('#table-wager-report').empty();

        var bodyData = getGridBody(wagerByMonths);
        var footerData = getGridFooter(grandTotal);

        var gridOptions = getGridOptions(scope, reportTitle, bodyData, footerData);
        tableWagerReport
            .removeClass(crmConstants.Enums.CssClass.DisplayNone)
            .fanexGrid(gridOptions);
    }

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (newValues, oldValues) {
                renderWagerReport(scope, element, attrs);
            });
        }
    };
});