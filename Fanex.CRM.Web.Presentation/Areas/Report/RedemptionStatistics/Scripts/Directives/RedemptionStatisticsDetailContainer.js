"use strict";
/* global dynamicReportApplication,
crmConstants,
sharedScriptManager,
viewResources,
moment */
/* jshint unused: vars */
/* jshint camelcase: false */
redemptionStatisticsApplication.directive('tableRedemptionStatisticsDetailContainer', function () {
    var selfScope;
    var cachedTable;

    function getGridOptions(data) {
        var bodyData = [];
        var footerData = [];
        var row;

        var columns = [
            { HeaderText: 'ClaimedDate', Width: 150, Name: 'ColClaimedDate', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: 'Customer', Width: 150, Name: 'ColCustomer', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: 'Subject', Width: 250, Name: 'ColSubject', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: 'Currency', Width: 80, Name: 'ColCurrency', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: 'Amount', Width: 80, Name: 'ColAmount', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: 'Status', Width: 80, Name: 'ColStatus', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: 'DoneBy', Width: 80, Name: 'ColDoneBy', HeaderAlign: 'center', CellAlign: 'left' }
        ];

        var reportData = data.ReportData;
        if (reportData && reportData.length > 0) {
            $.each(reportData, function (index, item) {
                row = {
                    Columns: [
                        { Name: 'ColClaimedDate', Value: item.CreatedDate },
                        { Name: 'ColCustomer', Value: item.UserName },
                        { Name: 'ColSubject', Value: item.Subject },
                        { Name: 'ColCurrency', Value: item.Currency },
                        { Name: 'ColAmount', Value: item.Amount },
                        { Name: 'ColStatus', Value: item.Status },
                        { Name: 'ColDoneBy', Value: item.DoneBy}]
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
                },
                customButtons: '<input id="button-back" class="btnGoBack" type="button" title="' + 'Go Back' + '"/>',
                showButtonsWhenNoData: true
            },
            pagerOption: {
                currentPage: 0,
                itemsPerPage: selfScope.lastSelectedParams.PageSize,
                itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions
            }
        };
    }

    function renderRedemptionDetail(scope, element, attrs) {
        selfScope = scope;

        var report = scope.redemptionStatisticsOptions.SummaryDataList;
        var gridOptions = getGridOptions(report);

        var grid = element.find('#table-redemption-statistics-detail-report');
        grid.empty().removeClass('hide').fanexGrid(gridOptions);

        element.find('#button-back').click(function () {
            scope.viewBack();
        });
    }

    return {
        restrict: 'C',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (oldval, newval) {
                if (scope.isSubmitClick) {
                    renderRedemptionDetail(scope, element, attrs);
                }
            });
        }
    };
});