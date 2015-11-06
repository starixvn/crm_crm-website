(function () {
    "use strict";
    /* global viewResources,
    crmConstants,
    sharedScriptManager */
    angular.module('dashboard-application').directive('tableRedemptionLogsContainer', function () {
        function renderRedemptionLogs(scope, element) {
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
                        { Name: "ColCurrency", Value: info.Currency.Name },
                        { Name: "ColAmount", Value: info.Amount },
                        { Name: "ColDoneBy", Value: sharedScriptManager.encodeHtml(info.DoneBy) },
                        { Name: "ColStatus", Value: info.Status.Value}],
                        CssClass: 'editlog-link-row'
                    });
                }
            }

            /* jshint camelcase: false */
            var gridOptions = {
                columns: [{ HeaderText: viewResources.Subject, Width: 430, Name: "ColSubject", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Claimed_Time, Width: 100, Name: "ColClaimedTime", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Currency, Width: 90, Name: "ColCurrency", HeaderAlign: "center", CellAlign: "center" },
                {
                    HeaderText: viewResources.Amount,
                    Width: 100,
                    Name: "ColAmount",
                    HeaderAlign: "center",
                    CellAlign: "right",
                    FormatNumber: crmConstants.Enums.FormatConstant.TwoDecimalNumber
                },
                { HeaderText: viewResources.DoneBy, Width: 120, Name: "ColDoneBy", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Status, Width: 80, Name: "ColStatus", HeaderAlign: "center", CellAlign: "center"}],
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
                    //jshint unused:false
                    renderRedemptionLogs(scope, element);
                });
            }
        };
    });
})();