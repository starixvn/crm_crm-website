(function () {
    "use strict";
    angular.module('changes-log-application').directive('headlineDaTableReportContainer', function () {
        var cachedData = {
            scope: null,
            reportTable: null,
            headlineDAReport: null,
            gridOptions: null
        };

        function getGridHeight() {
            return crmConstants.MarginBottomPage + 160;
        }

        function getGripOtions(report) {
            var bodyData = [];
            var reportLength = report ? report.DataList.length : 0; 
            if (reportLength > 0) {
                for (var i = 0; i < reportLength; i++) {
                    var data = report.DataList[i];
                    var no = i + 1 + cachedData.scope.lastHeadlineDASelectedParams.PageSize * (cachedData.scope.lastHeadlineDASelectedParams.PageIndex - 1);

                    var columns = [
                        { Name: "ColNo", Value: no, CellAlign: "center" },
                        { Name: "ColLogId", Value: data.LogId, CellAlign: "center" },
                        { Name: "ColLogTime", Value: data.LogTime, CellAlign: "center" },
                        { Name: "ColEditBy", Value: data.EditByName, CellAlign: "center" },
                        { Name: "ColHeadlineDA", Value: data.Name, CellAlign: "center" },
                        { Name: "ColAction", Value: data.Action },
                        { Name: "ColDetail", Value: '<div class="detail-child-col width-20-percent">Group Headline</div><div class="detail-child-col width-30-percent background-change"><div class="new-value">S01</div><div class="old-value">HL01</div></div><div class="detail-child-col width-20-percent">PIC</div><div class="detail-child-col width-30-percent border-right-none"><div>Boss1 (Boss), PIC1 (PIC)</div><div>Boss1 (Boss), PIC1 (PIC)</div></div>' },
                    ];

                    bodyData.push({
                        Columns: columns
                    });
                }
            }

            var headerColumns = [
                { HeaderText: viewResources.No, Width: 50, Name: "ColNo", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.LogId, Width: 80, Name: "ColLogId", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.LogDate, Width: 120, Name: "ColLogTime", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.EditBy, Width: 80, Name: "ColEditBy", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.HeadlineDA, Width: 120, Name: "ColHeadlineDA", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Action, Width: 250, Name: "ColAction", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Detail, Width: 600, Name: "ColDetail", HeaderAlign: "center", CellAlign: "center" },
            ];

            var gridOptions = {
                columns: headerColumns,
                headerColumnHeight: 50,
                showTitle: true,
                titleInfo: {
                    text: viewResources.ReportTitle
                },
                bodyRows: bodyData,
                gridExpandHeight: getGridHeight(),
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: cachedData.scope.lastHeadlineDASelectedParams.PageSize,
                    currentPage: cachedData.scope.lastHeadlineDASelectedParams.PageIndex - 1,
                    itemsPerPageArray: [100, 200, 300, 400, 500, 1000],
                    totalItem: report.Totals,
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };

            return gridOptions;
        }

        function ReRenderAfterPaged(page_index, itemsPerPage) {
            cachedData.scope.pagingHeadlineDAReport(page_index + 1, itemsPerPage);
            return false;
        }

        function renderHeadlineDAReport(scope, element) {
            if (scope.headlineDAReportOptions.DataList !== null) {
                cachedData.scope = scope;
                var tableHeadlineDAReport = element.find("#headline-da-table-report");
                cachedData.reportTable = tableHeadlineDAReport;
                var headlineDAReport = scope.headlineDAReportOptions.DataList;
                cachedData.headlineDAReport = headlineDAReport;
                var gridOptions = getGripOtions(headlineDAReport);
                cachedData.gridOptions = gridOptions;
                tableHeadlineDAReport.empty().removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
            }
        }

        return {
            restrict: 'C',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (oldval, newval) {
                    if (scope.headlineDAReportOptions.IsUpdateGrid) {
                        renderHeadlineDAReport(scope, element);
                        scope.headlineDAReportOptions.IsUpdateGrid = false;
                    }
                });
            }
        };
    });
})();