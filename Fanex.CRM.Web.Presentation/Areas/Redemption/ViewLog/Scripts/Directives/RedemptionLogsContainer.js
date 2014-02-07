"use strict";
/* global redemptionViewLogApplication,
viewResources */
redemptionViewLogApplication.directive('tableRedemptionLogsContainer', function () {
    function renderRedemptionLogs(scope, element, attrs) {
        /* jshint unused: vars */
        var tableRedemptionLogs = element.find("#table-redemption-logs");
        var logList = scope.logs;
        var bodyData = [];
        var footerData = [];
        tableRedemptionLogs.empty();
        if (logList && logList.length > 0) {
        }


        /* jshint camelcase: false */
        var gridOptions = {
            columns: [{ HeaderText: viewResources.Subject, Width: 600, Name: "ColSubject", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Claimed_Time, Width: 100, Name: "ColClaimedTime", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Status, Width: 140, Name: "ColStatus", HeaderAlign: "center", CellAlign: "center"}],
            showTitle: true,
            titleInfo: {
                text: viewResources.Customer_Redemption_Logs
            },
            bodyRows: bodyData,
            footerRows: footerData,
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 150,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 29,
            bodyRowHeight: 29
        };


        tableRedemptionLogs.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
    }

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (oldval, newval) {
                /* jshint unused: vars */
                renderRedemptionLogs(scope, element, attrs);
            });
        }
    };
});