(function () {
    "use strict";
    /* global crmConstants,
    viewResources,
    sharedScriptManager */
    /* jshint unused: vars */
    /* jshint camelcase: false */
    angular.module('report-customerreport-application').directive('tableCustomerReportContainer', function () {
        var cachedData = {
            scope: null,
            reportTable: null,
            customerReport: null,
            gridOptions: null
        };

        function getGridHeight() {
            return cachedData.scope.isShowFilter ?
220 + crmConstants.MarginBottomPage :
50 + crmConstants.MarginBottomPage;
        }

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

                    var contactInfo = [];
                    var contactLength = customer.PhoneInfo.length;
                    for (var j = 0; j < contactLength; j++) {
                        var contact = customer.PhoneInfo[j];
                        if (contact.CountryCode !== '' &&
contact.AreaCode !== '' &&
contact.PhoneNumber !== '') {
                            contactInfo.push(sharedScriptManager.stringFormat("{0}-{1}-{2}",
[contact.CountryCode, contact.AreaCode, contact.PhoneNumber]));
                        }
                    }

                    bodyData.push({
                        Columns: [{ Name: "ColTitle", Value: customer.Title.Name },
{ Name: "ColUsername", Value: customer.Username },
{
    Name: "ColVIP",
    TdClass: 'icon-' + (customer.IsVIP ? 'vip' : ''),
    Value: customer.IsVIP ? "<span class='glyphicon glyphicon-ok'></span>" : ''
},
{ Name: "ColBirthday", Value: customer.BirthDayString },
{ Name: "ColPIC1", Value: customer.PIC1 },
{ Name: "ColPIC2", Value: customer.PIC2 },
{ Name: "ColPIC3", Value: customer.PIC3 },
{ Name: "ColPIC4", Value: customer.PIC4 },
{ Name: "ColSkype", Value: customer.Skype },
{ Name: "ColEmail", Value: customer.Email },
{ Name: "ColLanguage", Value: customerLanguages },
{ Name: "ColSite", Value: customer.SiteSetting.Name}]
                    });
                }
            }

            var sortColumnIndex = getSortColumnIndex();
            var headerColumns = [{ HeaderText: viewResources.Title, Width: 50, Name: "ColTitle", HeaderAlign: "center", CellAlign: "center", Sort: true },
{ HeaderText: viewResources.Username, Width: 120, Name: "ColUsername", HeaderAlign: "center", CellAlign: "left", Sort: true },
{ HeaderText: viewResources.VIP, Width: 40, Name: "ColVIP", HeaderAlign: "center", CellAlign: "left", Sort: true },
{ HeaderText: viewResources.Birthday, Width: 80, Name: "ColBirthday", HeaderAlign: "center", CellAlign: "center", Sort: true },
{ HeaderText: viewResources.PIC1, Width: 180, Name: "ColPIC1", HeaderAlign: "center", CellAlign: "left" },
{ HeaderText: viewResources.PIC2, Width: 180, Name: "ColPIC2", HeaderAlign: "center", CellAlign: "left" },
{ HeaderText: viewResources.PIC3, Width: 180, Name: "ColPIC3", HeaderAlign: "center", CellAlign: "left" },
{ HeaderText: viewResources.PIC4, Width: 180, Name: "ColPIC4", HeaderAlign: "center", CellAlign: "left" },
{ HeaderText: viewResources.Skype, Width: 140, Name: "ColSkype", HeaderAlign: "center", CellAlign: "left" },
{ HeaderText: viewResources.Email, Width: 180, Name: "ColEmail", HeaderAlign: "center", CellAlign: "left" },
{ HeaderText: viewResources.Language, Width: 275, Name: "ColLanguage", HeaderAlign: "center", CellAlign: "left", Sort: true },
{ HeaderText: viewResources.Site, Width: 100, Name: "ColSite", HeaderAlign: "center", CellAlign: "left", Sort: true}];
            if (sortColumnIndex !== -1) {
                headerColumns[sortColumnIndex].SortDirection = cachedData.scope.lastSelectedParams.SortDirection;
            }

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
                    currentPage: cachedData.scope.lastSelectedParams.PageIndex,
                    itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                    totalItem: customerReport.TotalCustomer,
                    pagerSelectCallBack: ReRenderAfterPaged
                },
                sortOption: {
                    serverSortFunction: function (sortColumnName, sortDirection) {
                        cachedData.scope.lastSelectedParams.SortField = getSortField(sortColumnName);
                        cachedData.scope.lastSelectedParams.SortDirection = sortDirection;
                        cachedData.scope.sortCustomerReport();
                    }
                }
            };

            return gridOptions;
        }

        function getSortField(sortColumnName) {
            switch (sortColumnName) {
                case "ColVIP":
                    return "IsVIP";
                case "ColTitle":
                    return "TitleName";
                case "ColUsername":
                    return "Username";
                case "ColBirthday":
                    return "BirthDay";
                case "ColLanguage":
                    return "Languages";
                case "ColSite":
                    return "SiteSettingName";
                default:
                    return "";
            }
        }

        function getSortColumnIndex() {
            var sortField = cachedData.scope.lastSelectedParams.SortField;
            switch (sortField) {
                case "TitleName":
                    return 0;
                case "Username":
                    return 1;
                case "IsVIP":
                    return 2;
                case "BirthDay":
                    return 3;
                case "Languages":
                    return 10;
                case "SiteSettingName":
                    return 11;
                default:
                    return -1;
            }
        }

        function ReRenderAfterPaged(page_index, itemsPerPage, jq) {
            cachedData.scope.pagingCustomerReport(page_index, itemsPerPage);
            return false;
        }

        function renderCustomerReport(scope, element, attrs) {
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
                    if (!scope.customerReportOptions.IsUpdateGrid) {
                        return;
                    }

                    if (scope.customerReportOptions.IsResize && cachedData.reportTable) {
                        cachedData.reportTable.fanexGrid("resizeHeight", getGridHeight());
                        scope.customerReportOptions.IsResize = false;
                    } else {
                        renderCustomerReport(scope, element, attrs);
                    }

                    scope.customerReportOptions.IsUpdateGrid = false;
                });
            }
        };
    });
})();