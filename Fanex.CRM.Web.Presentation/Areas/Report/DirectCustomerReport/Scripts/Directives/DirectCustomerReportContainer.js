(function () {
    "use strict";
    /* global crmConstants,
    viewResources,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('report-directcustomerreport-application').directive('tableDirectCustomerReportContainer', function () {
        var cachedData = {
            scope: null,
            reportTable: null,
            customerReport: null,
            gridOptions: null
        };

        function getGridHeight() {
            return crmConstants.MarginBottomPage + (cachedData.scope.isShowFilter ? 200 : 100);
        }

        function getGripOtions(customerReport) {
            var bodyData = [];
            var footerData = [];
            var reportLength = customerReport ? customerReport.CustomerList.length : 0;
            if (reportLength > 0) {
                for (var i = 0; i < reportLength; i++) {
                    var customer = customerReport.CustomerList[i];
                    bodyData.push({
                        Columns: [
                            { Name: "GroupHeadline", Value: customer.GroupHeadline },
                            { Name: "Username", Value: customer.Username },
                            { Name: "Status", Value: customer.Status },
                            { Name: "Site", Value: customer.Site },
                            { Name: "Currency", Value: customer.Currency },
                            { Name: "Position", Value: customer.Position },
                            { Name: "IntroduceBy", Value: customer.IntroduceBy },
                            { Name: "Languages", Value: customer.Languages },
                            { Name: "Country", Value: customer.Country },
                            { Name: "Checker", Value: customer.Checker },
                            { Name: "VIP", Value: customer.VIP },
                            { Name: "FirstName", Value: customer.FirstName },
                            { Name: "LastName", Value: customer.LastName },
                            { Name: "Phone1", Value: customer.Phone1 },
                            { Name: "Phone2", Value: customer.Phone2 },
                            { Name: "Phone3", Value: customer.Phone3 },
                            { Name: "PIC1", Value: customer.PIC1 },
                            { Name: "PIC2", Value: customer.PIC2 },
                            { Name: "PIC3", Value: customer.PIC3 },
                            { Name: "PIC4", Value: customer.PIC4 },
                            { Name: "Mobile", Value: customer.Mobile },
                            { Name: "Email", Value: customer.Email },
                            { Name: "Skype", Value: customer.Skype },
                            { Name: "MSN", Value: customer.MSN },
                            { Name: "WhatsApp", Value: customer.WhatsApp },
                            { Name: "Line", Value: customer.Line },
                            { Name: "WeChat", Value: customer.WeChat },
                            { Name: "Yahoo", Value: customer.Yahoo },
                            { Name: "BBM", Value: customer.BBM },
                            { Name: "QQ", Value: customer.QQ }]
                    });
                }
            }

            var headerColumns = [{ HeaderText: viewResources.GroupHeadline, Width: 120, Name: "GroupHeadline", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Username, Width: 120, Name: "Username", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Status, Width: 120, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Site, Width: 120, Name: "Site", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Currency, Width: 120, Name: "Currency", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Position, Width: 120, Name: "Position", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.IntroduceBy, Width: 120, Name: "IntroduceBy", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Languages, Width: 120, Name: "Languages", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Country, Width: 120, Name: "Country", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Checker, Width: 120, Name: "Checker", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.VIP, Width: 120, Name: "VIP", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.FirstName, Width: 120, Name: "FirstName", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.LastName, Width: 120, Name: "LastName", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Phone1, Width: 120, Name: "Phone1", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Phone2, Width: 120, Name: "Phone2", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Phone3, Width: 120, Name: "Phone3", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.PIC1, Width: 120, Name: "PIC1", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.PIC2, Width: 120, Name: "PIC2", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.PIC3, Width: 120, Name: "PIC3", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.PIC4, Width: 120, Name: "PIC4", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Mobile, Width: 120, Name: "Mobile", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Email, Width: 120, Name: "Email", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Skype, Width: 120, Name: "Skype", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.MSN, Width: 120, Name: "MSN", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.WhatsApp, Width: 120, Name: "WhatsApp", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Line, Width: 120, Name: "Line", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.WeChat, Width: 120, Name: "WeChat", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Yahoo, Width: 120, Name: "Yahoo", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.BBM, Width: 120, Name: "BBM", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.QQ, Width: 120, Name: "QQ", HeaderAlign: "center", CellAlign: "center" }];

            var gridOptions = {
                columns: headerColumns,
                showTitle: true,
                titleInfo: {
                    text: viewResources.CustomerReport_Title,
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
                    totalItem: customerReport.TotalCustomer,
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
            if (scope.customerReportOptions.DataList === null) {
                return;
            }

            cachedData.scope = scope;
            var tableCustomerReport = element.find("#table-customer-report");
            cachedData.reportTable = tableCustomerReport;
            var customerReport = scope.customerReportOptions.DataList;
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
                    if (!scope.customerReportOptions.IsUpdateGrid) {
                        return;
                    }

                    if (scope.customerReportOptions.IsResize && cachedData.reportTable) {
                        cachedData.reportTable.fanexGrid("resizeHeight", getGridHeight());
                        scope.customerReportOptions.IsResize = false;
                    } else {
                        renderCustomerReport(scope, element);
                    }

                    scope.customerReportOptions.IsUpdateGrid = false;
                });
            }
        };
    });
})();