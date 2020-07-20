(function () {
    "use strict";
    angular.module('change-log-application').directive('tableReportContainer', function () {
        var cachedData = {
            scope: null,
            reportTable: null,
            customerReport: null,
            gridOptions: null
        };

        function getGridHeight() {
            return crmConstants.MarginBottomPage + (cachedData.scope.isShowFilter ? 160 : 50);
        }

        function getGripOtions(report) {
            var bodyData = [];
            var reportLength = report ? report.DataList.length : 0; 
            if (reportLength > 0) {
                for (var i = 0; i < reportLength; i++) {
                    var data = report.DataList[i];
                    var no = i + 1 + cachedData.scope.lastSelectedParams.PageSize * (cachedData.scope.lastSelectedParams.PageIndex - 1);

                    var columns = [
                        { Name: "ColNo", Value: no },
                        { Name: "ColLogId", Value: data.LogId },
                        { Name: "ColLogTime", Value: data.LogTime },
                        { Name: "ColUserName", Value: data.UserName },
                        { Name: "ColOldValue", Value: data.OldValue },
                        { Name: "ColNewValue", Value: data.NewValue },
                        { Name: "ColEditBy", Value: data.EditByName }
                    ];

                    bodyData.push({
                        Columns: columns
                    });
                }
            }

            var headerColumns = [
                { HeaderText: viewResources.No, Width: 50, Name: "ColNo", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.LogId, Width: 120, Name: "ColLogId", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Username, Width: 60, Name: "ColUserName", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.OldValue, Width: 120, Name: "ColOldValue", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.NewValue, Width: 120, Name: "ColNewValue", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.EditBy, Width: 200, Name: "ColEditBy", HeaderAlign: "center", CellAlign: "center" }
            ];

            var gridOptions = {
                customizeHeader: true,
                columns: headerColumns,
                showTitle: true,
                titleInfo: {
                    text: viewResources.ReportTitle,
                    showExcelButton: true,
                    exportExcelFunction: function () {
                        cachedData.scope.exportReport();
                    }
                },
                bodyRows: bodyData,
                gridExpandHeight: getGridHeight(),
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: cachedData.scope.lastSelectedParams.PageSize,
                    currentPage: cachedData.scope.lastSelectedParams.PageIndex - 1,
                    itemsPerPageArray: [100, 200, 300, 400, 500, 1000],
                    totalItem: report.Total,
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };

            return gridOptions;
        }

        function ReRenderAfterPaged(page_index, itemsPerPage) {
            cachedData.scope.pagingCustomerReport(page_index + 1, itemsPerPage);
            return false;
        }

        function renderChangeLogReport(scope, element) {
            if (scope.reportOptions.DataList === null) {
                return;
            }

            cachedData.scope = scope;
            var tableCustomerReport = element.find("#table-report");
            cachedData.reportTable = tableCustomerReport;
            var customerReport = scope.reportOptions.DataList;
            cachedData.customerReport = customerReport;
            var gridOptions = getGripOtions(customerReport);
            cachedData.gridOptions = gridOptions;
            tableCustomerReport.empty().removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
        }

        return {
            restrict: 'C',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (oldval, newval) {
                    if (!scope.reportOptions.IsUpdateGrid) {
                        return;
                    }

                    if (scope.reportOptions.IsResize && cachedData.reportTable) {
                        cachedData.reportTable.fanexGrid("resizeHeight", getGridHeight());
                        scope.customerReportOptions.IsResize = false;
                    } else {
                        renderChangeLogReport(scope, element);
                    }

                    scope.reportOptions.IsUpdateGrid = false;
                });
            }
        };
    });
})();