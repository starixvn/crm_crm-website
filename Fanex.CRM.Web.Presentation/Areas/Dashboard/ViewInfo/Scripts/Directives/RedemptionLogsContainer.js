"use strict";
/* global dashboardApplication,
viewResources,
crmConstants,
sharedScriptManager */
dashboardApplication.directive('tableRedemptionLogsContainer', function () {
    function renderRedemptionLogs(scope, element, attrs) {
        if (scope.redemptionLogs == null) {
            return;
        }

        /* jshint unused: vars */
        var tableRedemptionLogs = element.find("#table-redemption-logs");
        var logList = scope.redemptionLogs;
        var bodyData = [];
        var footerData = [];
        tableRedemptionLogs.empty();
        if (logList && logList.length > 0) {
            var logLength = logList.length;
            for (var i = 0; i < logLength; i++) {
                var info = logList[i];
                bodyData.push({
                    Columns: [
                        {
                            Name: "ColSubject",
                            Value: '<span class="link editlog-link" data-logid="' + info.Id + '">' + sharedScriptManager.encodeHtml(info.Subject) + '</span>',
                            TdClass: 'editlog-link-td'
                        },
                        { Name: "ColClaimedTime", Value: info.ClaimedTime },
                        { Name: "ColStatus", Value: info.Status.Value}],
                    CssClass: 'editlog-link-row'
                });
            }
        }

        /* jshint camelcase: false */
        var gridOptions = {
            columns: [{ HeaderText: viewResources.Subject, Width: 673, Name: "ColSubject", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Claimed_Time, Width: 120, Name: "ColClaimedTime", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Status, Width: 140, Name: "ColStatus", HeaderAlign: "center", CellAlign: "center"}],
            showTitle: true,
            titleInfo: {
                text: viewResources.Last_5_Redemption_Log
            },
            bodyRows: bodyData,
            footerRows: footerData,
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 150,
            showDistinctRow: true,
            showHighlightRow: true,
            headerColumnHeight: 29,
            bodyRowHeight: 29
        };

        tableRedemptionLogs.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
        var editLogLinks = tableRedemptionLogs.find(".editlog-link-td .editlog-link");
        editLogLinks.unbind("click");
        editLogLinks.click(function () {
            scope.editRedemptionLog($(this).attr("data-logid"));
        });

        var editLogRows = tableRedemptionLogs.find(".editlog-link-row");
        editLogRows.dblclick(function () {
            scope.editRedemptionLog($(this).find('.editlog-link').attr("data-logid"));
        });
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