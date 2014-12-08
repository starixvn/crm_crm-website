"use strict";
/* global redemptionStatisticsApplication */
/* jshint unused: vars */
/* jshint camelcase: false */
redemptionStatisticsApplication.directive('tableRedemptionStatisticsDetailContainer', function () {
    var selfScope;

    function modifyGridOptions(data) {
        var options = {
            titleInfo: {
                text: data.TitleInfoText + ' - ' + selfScope.title,
                showExcelButton: true,
                exportExcelFunction: function () {
                    selfScope.exportDetailReport();
                },
                customButtons: '<input id="button-back" class="btnGoBack" type="button" title="' + 'Go Back' + '"/>',
                showButtonsWhenNoData: true
            },
            fullWidth: true,
            fullHeight: true,
            gridExpandHeight: 200,
            headerColumnHeight: 29,
            bodyRowHeight: 29,
            autoFixFrozenRowHeight: true
        };

        return $.extend({}, data, options);
    }

    function renderRedemptionDetail(scope, element, attrs) {
        selfScope = scope;

        var report = scope.redemptionStatisticsOptions.DetailDataList;
        report = modifyGridOptions(report);

        var grid = element.find('#table-redemption-statistics-detail-report');
        grid.empty().removeClass('hide').fanexGrid(report);

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