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

    function getGridHeight() {
        return cachedData.scope.isShowFilter ?
            200 + crmConstants.MarginBottomPage :
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
                        { Name: "ColFirstName", Value: customer.FirstName },
                        { Name: "ColLastName", Value: customer.LastName },
                        {
                            Name: "ColVIP",
                            TdClass: 'icon-' + (customer.IsVIP ? 'vip' : ''),
                            Value: customer.IsVIP ? "<span class='glyphicon glyphicon-ok'></span>" : ''
                        },
                        {
                            Name: "ColContactNumber",
                            Value: contactInfo.join("<div class='contactpadding'>&nbsp;</div>")
                        },
                        {
                            Name: "ColMobile",
                            Value: customer.MobilePhone.CountryCode !== '' &&
                                customer.MobilePhone.AreaCode !== '' &&
                                customer.MobilePhone.PhoneNumber !== '' ?
                                    sharedScriptManager.stringFormat("{0}-{1}-{2}",
                                    [customer.MobilePhone.CountryCode, customer.MobilePhone.AreaCode, customer.MobilePhone.PhoneNumber]) :
                                    ""
                        },
                        {
                            Name: "ColFax",
                            Value: customer.FaxInfo.CountryCode !== '' &&
                                customer.FaxInfo.AreaCode !== '' &&
                                customer.FaxInfo.FaxNumber !== '' ?
                                    sharedScriptManager.stringFormat("{0}-{1}-{2}",
                                    [customer.FaxInfo.CountryCode, customer.FaxInfo.AreaCode, customer.FaxInfo.FaxNumber]) :
                                    ""
                        },
                        { Name: "ColSkype", Value: customer.Skype },
                        { Name: "ColEmail", Value: customer.Email },

                        { Name: "ColBirthday", Value: customer.BirthDayString },
                        { Name: "ColSite", Value: customer.SiteSetting.Name },
                        { Name: "ColCountry", Value: customer.Country.Name },
                        { Name: "ColLanguage", Value: customerLanguages },

                        { Name: "ColLine1", Value: customer.AddressInfo.LineList[0] },
                        { Name: "ColLine2", Value: customer.AddressInfo.LineList[1] },
                        { Name: "ColLine3", Value: customer.AddressInfo.LineList[2] },
                        { Name: "ColCity", Value: customer.AddressInfo.City },
                        { Name: "ColState", Value: customer.AddressInfo.State },
                        { Name: "ColPostCode", Value: customer.AddressInfo.PostCode}]
                });
            }
        }

        var sortColumnIndex = getSortColumnIndex();
        var headerColumns = [{ HeaderText: viewResources.Title, Width: 50, Name: "ColTitle", HeaderAlign: "center", CellAlign: "center", Sort: true },
            { HeaderText: viewResources.First_Name, Width: 120, Name: "ColFirstName", HeaderAlign: "center", CellAlign: "left", Sort: true },
            { HeaderText: viewResources.Last_Name, Width: 120, Name: "ColLastName", HeaderAlign: "center", CellAlign: "left", Sort: true },
            { HeaderText: viewResources.VIP, Width: 40, Name: "ColVIP", HeaderAlign: "center", CellAlign: "left", Sort: true },

            { HeaderText: viewResources.Contact_Number, Width: 180, Name: "ColContactNumber", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: viewResources.Mobile, Width: 180, Name: "ColMobile", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: viewResources.Fax, Width: 180, Name: "ColFax", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: viewResources.Skype, Width: 140, Name: "ColSkype", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: viewResources.Email, Width: 140, Name: "ColEmail", HeaderAlign: "center", CellAlign: "left" },

            { HeaderText: viewResources.Birthday, Width: 80, Name: "ColBirthday", HeaderAlign: "center", CellAlign: "center", Sort: true },
            { HeaderText: viewResources.Site, Width: 100, Name: "ColSite", HeaderAlign: "center", CellAlign: "left", Sort: true },
            { HeaderText: viewResources.Country, Width: 140, Name: "ColCountry", HeaderAlign: "center", CellAlign: "left", Sort: true },
            { HeaderText: viewResources.Language, Width: 275, Name: "ColLanguage", HeaderAlign: "center", CellAlign: "left", Sort: true },

            { HeaderText: viewResources.Line_1, Width: 140, Name: "ColLine1", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: viewResources.Line_2, Width: 140, Name: "ColLine2", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: viewResources.Line_3, Width: 140, Name: "ColLine3", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: viewResources.City, Width: 100, Name: "ColCity", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: viewResources.State, Width: 100, Name: "ColState", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: viewResources.Post_Code, Width: 100, Name: "ColPostCode", HeaderAlign: "center", CellAlign: "left"}];
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
            allowTruncate: true,
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
            case "ColFirstName":
                return "FirstName";
            case "ColLastName":
                return "LastName";
            case "ColCountry":
                return "CountryName";
            case "ColLanguage":
                return "Languages";
            case "ColSite":
                return "SiteSettingName";
            case "ColBirthday":
                return "BirthDay";
            default:
                return "";
        }
    }

    function getSortColumnIndex() {
        var sortField = cachedData.scope.lastSelectedParams.SortField;
        switch (sortField) {
            case "TitleName":
                return 0;
            case "FirstName":
                return 1;
            case "LastName":
                return 2;
            case "IsVIP":
                return 3;
            case "CountryName":
                return 11;
            case "Languages":
                return 12;
            case "SiteSettingName":
                return 10;
            case "BirthDay":
                return 9;
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