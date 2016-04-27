(function () {
    "use strict";
    /* global crmConstants,
    reportDirectiveHelper */
    /* jshint unused: false,
    camelcase: false */
    angular.module('log-statistics-application').directive('tableIbcReportContainer', function () {
        var selfScope;
        var cachedTable;
        var cachedReport;
        var cachedGridOptions;

        function renderIBCReport(scope, element) {
            var report = scope.ibcReportOptions.DataList;
            if (!report || report.ProductColumns.length === 0 || report.CommunicationColumns.length === 0) {
                return;
            }

            selfScope = scope;
            var tableIBCReport = element.find("#table-ibc-report");
            cachedTable = tableIBCReport;
            cachedReport = report;
            var gridOptions = reportDirectiveHelper.getGridOptions(report, scope.ibcReportOptions.IsExpand);
            gridOptions.gridHeight = ($(window).height() - 200) / 2;
            gridOptions.titleInfo.exportExcelFunction = function () {
                selfScope.exportReport(selfScope.ibcReportOptions.Type);
            };

            cachedGridOptions = gridOptions;
            element.removeClass(crmConstants.Enums.CssClass.DisplayNone);
            $(".table-detail-report-container").addClass(crmConstants.Enums.CssClass.DisplayNone);
            tableIBCReport.fanexGrid(gridOptions);
            
            bindEvents(scope, element);
        }

        function bindEvents(scope, element) {
            element.find(".btnCharting").click(function () {
                reportDirectiveHelper.viewChart(scope, cachedReport);
            });

            element.find(".btnViewDetail").click(function () {
                scope.viewDetail(selfScope.ibcReportOptions.Type);
            });
            element.find(".btnHorizontal").click(function () {
                scope.toggleExpansionForIBCReport();
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

                    renderIBCReport(scope, element);
                    scope.ibcReportOptions.IsUpdateGrid = false;
                });
            }
        };
    });
})();