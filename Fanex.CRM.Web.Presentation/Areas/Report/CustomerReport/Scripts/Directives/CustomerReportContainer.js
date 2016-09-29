(function () {
    "use strict";
    /* global crmConstants,
    viewResources,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('report-customerreport-application').directive('tableCustomerReportContainer', function () {
        var cachedData = {
            scope: null,
            reportTable: null,
            customerReport: null,
            gridOptions: null
        };

        function getGridHeight() {
            return crmConstants.MarginBottomPage + (cachedData.scope.isShowFilter ? 230 : 50);
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
                            { Name: "ColHeadline", Value: customer.Headline },
                            { Name: "ColChecker", Value: customer.Checker },
                            { Name: "ColPosition", Value: customer.Position },
                            { Name: "ColStatus", Value: customer.Status },
                            { Name: "ColUsername", Value: customer.Username },
                            { Name: "ColFirstname", Value: customer.FirstName },
                            { Name: "ColLastname", Value: customer.LastName },
                            { Name: "ColPhone1", Value: customer.Phone1 },
                            { Name: "ColPhone2", Value: customer.Phone2 },
                            { Name: "ColPhone3", Value: customer.Phone3 },
                            { Name: "ColMobile", Value: customer.Mobile },
                            { Name: "ColFax", Value: customer.Fax },
                            { Name: "ColEmail", Value: customer.Email },
                            { Name: "ColSkype", Value: customer.Skype },
                            { Name: "ColMSN", Value: customer.MSN },
                            { Name: "ColWhatsApp", Value: customer.WhatsApp },
                            { Name: "ColLine", Value: customer.Line },
                            { Name: "ColWeChat", Value: customer.WeChat },
                            { Name: "ColYahoo", Value: customer.Yahoo },
                            { Name: "ColBBM", Value: customer.BBM },
                            { Name: "ColQQ", Value: customer.QQ },
                            { Name: "ColLanguage", Value: customer.Language },
                            {
                                Name: "ColVIP",
                                TdClass: 'icon-' + (customer.IsVIP ? 'vip' : ''),
                                Value: customer.IsVIP ? "<span class='glyphicon glyphicon-ok'></span>" : ''
                            },
                            { Name: "ColPIC1", Value: customer.Pic1 },
                            { Name: "ColPIC2", Value: customer.Pic2 },
                            { Name: "ColPIC3", Value: customer.Pic3 },
                            { Name: "ColPIC4", Value: customer.Pic4 },
                            { Name: "ColBirthday", Value: customer.BirthDay },
                            { Name: "ColPassport", Value: customer.Passport },
                            { Name: "ColSiteName", Value: customer.SiteName },
                            { Name: "ColCountry", Value: customer.CountryName }]
                    });
                }
            }

            var headerColumns = [{ HeaderText: viewResources.Headline, Width: 120, Name: "ColHeadline", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Checker, Width: 120, Name: "ColChecker", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Position, Width: 120, Name: "ColPosition", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Status, Width: 120, Name: "ColStatus", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Username, Width: 120, Name: "ColUsername", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.FirstName, Width: 120, Name: "ColFirstname", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.LastName, Width: 120, Name: "ColLastname", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Phone1, Width: 120, Name: "ColPhone1", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Phone2, Width: 120, Name: "ColPhone2", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Phone3, Width: 120, Name: "ColPhone3", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Mobile, Width: 120, Name: "ColMobile", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Fax, Width: 120, Name: "ColFax", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Email, Width: 180, Name: "ColEmail", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Skype, Width: 80, Name: "ColSkype", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.MSN, Width: 80, Name: "ColMSN", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.WhatsApp, Width: 80, Name: "ColWhatsApp", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Line, Width: 80, Name: "ColLine", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.WeChat, Width: 80, Name: "ColWeChat", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Yahoo, Width: 80, Name: "ColYahoo", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.BBM, Width: 80, Name: "ColBBM", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.QQ, Width: 80, Name: "ColQQ", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Languages, Width: 200, Name: "ColLanguage", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.VIP, Width: 40, Name: "ColVIP", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.PIC1, Width: 180, Name: "ColPIC1", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.PIC2, Width: 180, Name: "ColPIC2", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.PIC3, Width: 180, Name: "ColPIC3", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.PIC4, Width: 180, Name: "ColPIC4", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Birthday, Width: 80, Name: "ColBirthday", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MMM DD, YYYY" },
                { HeaderText: viewResources.Passport, Width: 140, Name: "ColPassport", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.SiteName, Width: 100, Name: "ColSiteName", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Country, Width: 100, Name: "ColCountry", HeaderAlign: "center", CellAlign: "left" }];

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