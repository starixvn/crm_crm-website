"use strict";
/* global customerReportApplication,
crmConstants,
viewResources,
sharedScriptManager */
/* jshint unused: vars */
/* jshint camelcase: false */
customerReportApplication.directive('tableCustomerReportContainer', function () {
    var cachedData = {
        scope: null,
        reportTable: null,
        customerReport: null,
        gridOptions: null
    };

    function getCustomerLanguages(customer) {
        var languageLength = customer.Languages.length;
        var languages = [];
        for (var i = 0; i < languageLength; i++) {
            languages.push(customer.Languages[i].Name);
        }

        return languages.join(", ");
    }

    function getGripOtions(customerReport) {
        var bodyData = [];
        var footerData = [];
        var reportLength = customerReport ? customerReport.CustomerList.length : 0;
        if (reportLength > 0) {
            for (var i = 0; i < reportLength; i++) {
                var customer = customerReport.CustomerList[i];
                var customerLanguages = getCustomerLanguages(customer);


                bodyData.push({
                    Columns: [{ Name: "ColVIP", TdClass: 'icon-' + (customer.IsVIP ? 'vip' : '') },
                        { Name: "ColTitle", Value: customer.Title.Name },
                        { Name: "ColFirstName", Value: customer.FirstName },
                        { Name: "ColLastName", Value: customer.LastName },
                        { Name: "ColCountry", Value: customer.Country.Name },
                        { Name: "ColLanguage", Value: customerLanguages },
                        { Name: "ColSite", Value: customer.SiteSetting.Name },
                        { Name: "ColBirthday", Value: customer.BirthDayString}]
                });
            }
        }

        var gridOptions = {
            columns: [{ HeaderText: viewResources.VIP, Width: 40, Name: "ColVIP", HeaderAlign: "center", CellAlign: "left", Sort: true },
                { HeaderText: viewResources.Title, Width: 40, Name: "ColTitle", HeaderAlign: "center", CellAlign: "center", Sort: true },
                { HeaderText: viewResources.First_Name, Width: 120, Name: "ColFirstName", HeaderAlign: "center", CellAlign: "left", Sort: true },
                { HeaderText: viewResources.Last_Name, Width: 120, Name: "ColLastName", HeaderAlign: "center", CellAlign: "left", Sort: true },
                { HeaderText: viewResources.Country, Width: 140, Name: "ColCountry", HeaderAlign: "center", CellAlign: "left", Sort: true },
                { HeaderText: viewResources.Language, Width: 275, Name: "ColLanguage", HeaderAlign: "center", CellAlign: "left", Sort: true },
                { HeaderText: viewResources.Site, Width: 100, Name: "ColSite", HeaderAlign: "center", CellAlign: "left", Sort: true },
                { HeaderText: viewResources.Birthday, Width: 80, Name: "ColBirthday", HeaderAlign: "center", CellAlign: "center", Sort: true}],
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
            gridExpandHeight: 230 + crmConstants.MarginBottomPage,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 29,
            bodyRowHeight: 29,
            showPager: true,
            showTree: false,
            customPager: true,
            pagerOption: {
                itemsPerPage: cachedData.scope.lastSelectedParams.PageSize,
                currentPage: cachedData.scope.lastSelectedParams.PageIndex,
                itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                totalItem: customerReport.TotalCustomer,
                pagerSelectCallBack: ReRenderAfterPaged
            }
        };

        return gridOptions;
    }

    function ReRenderAfterPaged(page_index, itemsPerPage, jq) {
        cachedData.scope.pagingCustomerReport(page_index, itemsPerPage);
        return false;
    }

    function renderCustomerReport(scope, element, attrs) {
        if (scope.customerReport === null) {
            return;
        }

        cachedData.scope = scope;
        var tableCustomerReport = element.find("#table-customer-report");
        cachedData.reportTable = tableCustomerReport;
        var customerReport = scope.customerReport;
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
                renderCustomerReport(scope, element, attrs);
            });
        }
    };
});