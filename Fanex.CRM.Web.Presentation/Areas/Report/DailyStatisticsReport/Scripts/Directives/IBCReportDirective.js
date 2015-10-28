(function () {
    "use strict";
    /* global crmConstants,
    reportDirectiveHelper */
    /* jshint unused: vars,
    camelcase: false */
    angular.module('report-dailystatistics-application').directive('tableIbcReportContainer', function () {
        var selfScope;
        var cachedTable;
        var cachedReport;
        var cachedGridOptions;

        function renderIBCReport(scope, element, attrs) {
            if (scope.ibcReportOptions.DataList === null) {
                return;
            }

            selfScope = scope;
            var tableIBCReport = element.find("#table-ibc-report");
            cachedTable = tableIBCReport;
            var report = scope.ibcReportOptions.DataList;
            cachedReport = report;
            var gridOptions = reportDirectiveHelper.getGridOptions(report);
            gridOptions.gridHeight = ($(window).height() - 200) / 2;
            gridOptions.titleInfo.exportExcelFunction = function () {
                selfScope.exportIBCReport();
            };

            cachedGridOptions = gridOptions;
            element.removeClass(crmConstants.Enums.CssClass.DisplayNone);
            $(".table-detail-report-container").addClass(crmConstants.Enums.CssClass.DisplayNone);
            tableIBCReport.empty().fanexGrid(gridOptions);
            bindEvents(scope, element);
        }

        function bindEvents(scope, element) {
            element.find(".btnCharting").click(function () {
                reportDirectiveHelper.viewChart(scope, cachedReport);
            });

            element.find(".btnViewDetail").click(function () {
                scope.viewDetail(crmConstants.Enums.Site.IBC);
            });
        }

        return {
            restrict: 'C',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (oldval, newval) {
                    if (!scope.ibcReportOptions ||
                    !scope.ibcReportOptions.IsUpdateGrid) {
                        return;
                    }

                    renderIBCReport(scope, element, attrs);
                    scope.ibcReportOptions.IsUpdateGrid = false;
                });
            }
        };
    });
})();