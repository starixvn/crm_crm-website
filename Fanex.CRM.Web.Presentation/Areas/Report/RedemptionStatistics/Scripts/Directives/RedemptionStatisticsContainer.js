"use strict";
/* global redemptionStatisticsApplication,
crmConstants*/
/* jshint unused: vars */
/* jshint camelcase: false */
redemptionStatisticsApplication.directive('tableRedemptionStatisticsContainer', function () {
    var selfScope;

    function modifyGridOptions(data) {
        var options = {
            titleInfo: {
                text: data.TitleInfoText,
                showExcelButton: true,
                exportExcelFunction: function () {
                    selfScope.exportSummaryReport();
                }
            },
            headerColumnHeight: 29,
            bodyRowHeight: 29,
            fullWidth: true,
            fullHeight: false,
            gridHeight: 665
        };

        return $.extend({}, data, options);
    }

    function renderRedemptionSummary(scope, element, attrs) {
        selfScope = scope;

        var report = scope.redemptionStatisticsOptions.SummaryDataList;
        report = modifyGridOptions(report);

        var grid = element.find('#table-redemption-statistics-report');
        grid.empty().removeClass('hide').fanexGrid(report);
        bindDateLink(grid, scope);
    }

    function bindDateLink(table, scope) {
        var viewDateLinks = table.find(".detail-date, .detail-currency");
        viewDateLinks.unbind("click");
        viewDateLinks.click(function () {
            var $this = $(this);
            var currencyId = parseInt($this.attr("data-currency-id"));
            var month = parseInt($this.attr("data-month"));
            var year = parseInt($this.attr("data-year"));

            var title = currencyId ? $this.attr("data-currency") :
                month === 0 ? year : crmConstants.MonthNames[month - 1] + ' ' + year;
            scope.getRedemptionStatisticsDetailReport(title, currencyId, year, month);
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