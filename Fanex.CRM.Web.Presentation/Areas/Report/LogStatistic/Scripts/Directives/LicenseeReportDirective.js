(function () {
    "use strict";
    /* global crmConstants,
    reportDirectiveHelper */
    /* jshint unused: vars,
    camelcase: false */
    angular.module('log-statistic-application').directive('tableLicenseeReportContainer', function () {
        var selfScope;
        var cachedTable;
        var cachedReport;
        var cachedGridOptions;

        function renderReport(scope, element, attrs) {
            if (scope.licenseeReportOptions.DataList === null) {
                return;
            }

            selfScope = scope;
            var tableReport = element.find("#table-licensee-report");
            cachedTable = tableReport;
            var report = scope.licenseeReportOptions.DataList;
            cachedReport = report;
            var gridOptions = reportDirectiveHelper.getGridOptions(report, scope.licenseeReportOptions.IsExpand);
            gridOptions.gridHeight = $(window).height() - 200 - $("#table-ibc-report").height();
            gridOptions.titleInfo.exportExcelFunction = function () {
                selfScope.exportLicenseeReport();
            };

            cachedGridOptions = gridOptions;
            element.removeClass(crmConstants.Enums.CssClass.DisplayNone);
            $(".table-detail-report-container").addClass(crmConstants.Enums.CssClass.DisplayNone);
            tableReport.empty().fanexGrid(gridOptions);

            bindEvents(scope, element);
        }

        function bindEvents(scope, element) {
            element.find(".btnCharting").click(function () {
                reportDirectiveHelper.viewChart(scope, cachedReport);
            });

            element.find(".btnViewDetail").click(function () {
                scope.viewDetail(crmConstants.Enums.Site.Licensee);
            });
            element.find(".btnExpansion").click(function () {
                scope.toggleExpansionForLicenseeReport();
            });
        }

        return {
            restrict: 'C',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (oldval, newval) {
                    if (!scope.licenseeReportOptions ||
                    !scope.licenseeReportOptions.IsUpdateGrid) {
                        return;
                    }

                    renderReport(scope, element, attrs);
                    scope.licenseeReportOptions.IsUpdateGrid = false;
                });
            }
        };
    });
})();