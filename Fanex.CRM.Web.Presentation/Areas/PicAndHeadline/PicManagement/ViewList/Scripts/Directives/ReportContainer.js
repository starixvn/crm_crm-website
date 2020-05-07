(function () {
    "use strict";
    /* global crmConstants,
    viewResources,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('picmanagement-application').directive('tableReportContainer', function () {
        var cachedData = {
            scope: null,
            reportTable: null,
            customerReport: null,
            gridOptions: null
        };

        function getGridHeight() {
            return crmConstants.MarginBottomPage + (cachedData.scope.isShowFilter ? 230 : 50);
        }

        function getGripOtions(report) {
            var bodyData = [];
            var footerData = [];
            var reportLength = report ? report.DataList.length : 0;
            if (reportLength > 0) {
                for (var i = 0; i < reportLength; i++) {
                    var data = report.DataList[i];
                    var no = i + 1 + cachedData.scope.lastSelectedParams.PageSize * (cachedData.scope.lastSelectedParams.PageIndex - 1);
                    var columns = [
                        { Name: "ColNo", Value: no },
                        { Name: "ColUsername", Value: data.Username },
                        { Name: "ColRole", Value: data.Role },
                        { Name: "ColGroupHeadline", Value: data.GroupHeadline },
                        { Name: "ColContact", Value: data.Contact },
                        { Name: "ColRemark", Value: data.Remark },
                        { Name: "ColEdit", Value: data.Edit },
                        { Name: "ColActive", Value: data.Active }
                    ];

                    bodyData.push({
                        Columns: columns
                    });
                }
            }

            var headerColumns = [
                { HeaderText: viewResources.No, Width: 120, Name: "ColNo", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Username, Width: 120, Name: "ColUsername", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Role, Width: 120, Name: "ColRole", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.GroupHeadline, Width: 120, Name: "ColGroupHeadline", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Contact, Width: 120, Name: "ColContact", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Remark, Width: 120, Name: "ColRemark", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Edit, Width: 120, Name: "ColEdit", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Active, Width: 120, Name: "ColActive", HeaderAlign: "center", CellAlign: "left" }
                ];

            var gridOptions = {
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
                footerRows: footerData,
                numberOfFrozenColumn: 0,
                fullWidth: true,
                fullHeight: true,
                gridExpandHeight: getGridHeight(),
                showDistinctRow: true,
                showHighlightRow: true,
                headerColumnHeight: 29,
                bodyRowHeight: 29,
                showPager: true,
                showTree: false,
                customPager: true,
                //allowTruncate: true,
                pagerOption: {
                    itemsPerPage: cachedData.scope.lastSelectedParams.PageSize,
                    currentPage: cachedData.scope.lastSelectedParams.PageIndex - 1,
                    itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
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

        function renderCustomerReport(scope, element) {
            if (scope.reportOptions.DataList === null) {
                return;
            }

            cachedData.scope = scope;
            console.log("xx");
            debugger
            console.log(scope.communicationTools);
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
                    /* jshint unused: false */
                    if (!scope.reportOptions.IsUpdateGrid) {
                        return;
                    }

                    if (scope.reportOptions.IsResize && cachedData.reportTable) {
                        cachedData.reportTable.fanexGrid("resizeHeight", getGridHeight());
                        scope.customerReportOptions.IsResize = false;
                    } else {
                        renderCustomerReport(scope, element);
                    }

                    scope.reportOptions.IsUpdateGrid = false;
                });
            }
        };
    });
})();