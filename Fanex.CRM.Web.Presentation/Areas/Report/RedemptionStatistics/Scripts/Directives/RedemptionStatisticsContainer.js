"use strict";
/* global dynamicReportApplication,
crmConstants,
sharedScriptManager,
viewResources,
moment */
/* jshint unused: vars */
/* jshint camelcase: false */
redemptionStatisticsApplication.directive('tableRedemptionStatisticsContainer', function () {
    var cachedRedemptionStatisticsReport;
    var cachedGridOptions;
    var selfScope;
    var cachedTable;

    function getGridOptions(data) {
        var bodyData = [];
        var footerData = [];
        var row;

        var columns = [
            { HeaderText: 'Date', Width: 200, Name: 'ColDate', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: 'Currency', Width: 150, Name: 'ColCurrency', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: 'Amount', Width: 150, Name: 'ColAmount', HeaderAlign: 'center', CellAlign: 'left' }
        ];

        var clickDateFormat = '<span class="detail-date link" data-subject="{0}">{1}</span>';
        var clickCurrencyFormat = '<span class="detail-currency link" data-currency-id="{0}">{1}</span>';
        var dateIdFormat = '{0} {1}';

        var clickDateCol = '';
        var clickCurrencyCol = '';
        var dateId = '';

        var reportData = data.ReportData;
        if (reportData && reportData.length > 0) {
            $.each(reportData, function (index, item) {

                dateId = dateIdFormat.format(crmConstants.MonthNames[item.Month], item.Year);
                clickDateCol = clickDateFormat.format(item.Subject, dateId);
                clickCurrencyCol = clickCurrencyFormat.format(item.Currency, item.Currency);

                row = {
                    Columns: [
                        { Name: 'ColDate', Value: clickDateCol },
                        { Name: 'ColCurrency', Value: clickCurrencyCol },
                        { Name: 'ColAmount', Value: item.Amount}]
                };
                bodyData.push(row);
            });
        }

        return {
            columns: columns,
            bodyRows: bodyData,

            showDistinctRow: true,
            showNumberColumn: false,
            showPager: true,
            showTitle: true,
            titleInfo: {
                text: data.Title,
                showExcelButton: true,
                exportExcelFunction: function () {
                    selfScope.exportSummaryReport();
                }
            },
            pagerOption: {
                currentPage: 0,
                itemsPerPage: selfScope.lastSelectedParams.PageSize,
                itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions
            }
        };
    }

    function renderRedemptionSummary(scope, element, attrs) {
        selfScope = scope;

        var report = scope.redemptionStatisticsOptions.SummaryDataList;
        var gridOptions = getGridOptions(report);

        var grid = element.find('#table-redemption-statistics-report');
        grid.empty().removeClass('hide').fanexGrid(gridOptions);
        bindDateLink(grid, scope);
    }

    function bindDateLink(table, scope) {
        var viewLinks = table.find(".detail-date");
        viewLinks.unbind("click");
        viewLinks.click(function () {
            var subject = parseInt($(this).attr("data-subject"));
            scope.getRedemptionStatisticsDetailReport(subject);
        });
    }

    return {
        restrict: 'C',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (oldval, newval) {
                if (scope.isSubmitClick) {
                    renderRedemptionSummary(scope, element, attrs);
                    scope.redemptionStatisticsOptions.IsUpdateGrid = false;
                }
            });
        }
    };
});