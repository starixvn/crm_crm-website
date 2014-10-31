"use strict";
/* global dailyStatisticsReportApplication,
crmConstants,
viewResources,
sharedScriptManager */
/* jshint unused: vars,
camelcase: false */
dailyStatisticsReportApplication.directive('tableDetailReportContainer', function () {
    var selfScope;
    var cachedTable;
    var cachedReport;
    var cachedGridOptions;

    function renderReport(scope, element, attrs) {
        if (scope.detailReportOptions.DataList === null) {
            return;
        }

        selfScope = scope;
        var tableReport = element.find("#table-detail-report");
        cachedTable = tableReport;
        var report = scope.detailReportOptions.DataList;
        cachedReport = report;
        var gridOptions = getGridOptions(report);
        cachedGridOptions = gridOptions;
        element.removeClass(crmConstants.Enums.CssClass.DisplayNone);
        tableReport.empty().fanexGrid(gridOptions);

        $(".table-ibc-report-container").addClass(crmConstants.Enums.CssClass.DisplayNone);
        $(".table-licensee-report-container").addClass(crmConstants.Enums.CssClass.DisplayNone);
        element.find(".btnGoBack").click(function () {
            backToMainReport(element);
        });
        element.find(".back-main-report").click(function () {
            backToMainReport(element);
        });
    }

    function getGridOptions(report) {
        var bodyData = [];
        var footerData = [];
        var childReportLength = report ? report.ChildReports.length : 0;
        if (childReportLength > 0) {
            for (var i = 0; i < childReportLength; i++) {
                var info = report.ChildReports[i];
                var contactInfo =
                   viewResources.MSN + ": " + sharedScriptManager.encodeHtml(info.MSN) +
                   "<br/>" + viewResources.Skype + ": " + sharedScriptManager.encodeHtml(info.Skype) +
                   "<br/>" + viewResources.Email + ": " + sharedScriptManager.encodeHtml(info.Email) +
                   "<br/>" + viewResources.Phone + ": " + sharedScriptManager.encodeHtml(info.Phone) +
                   "<br/>" + viewResources.Mobile + ": " + sharedScriptManager.encodeHtml(info.MobilePhone);

                bodyData.push({
                    Columns: [{ Name: "ColDate", Value: info.StartDateString },
                    { Name: "ColStartTime", Value: info.StartTime },
                    { Name: "ColEndTime", Value: info.ResolutionTimeString },
                    {
                        Name: "ColDuration",
                        Value: sharedScriptManager.stringFormat("{0}:{1}:{2}",
                            [sharedScriptManager.pad2(info.Duration.Days),
                            sharedScriptManager.pad2(info.Duration.Hours),
                            sharedScriptManager.pad2(info.Duration.Minutes)])
                    },
                    { Name: "ColCustomer", Value: info.Username },
                    { Name: "ColSite", Value: info.SiteInfo.Name },
                    { Name: "ColTypeOfContact", Value: info.CommunicationTool.Name },
                    { Name: "ColExtension", Value: info.Extension.Name },
                    { Name: "ColContact", Value: contactInfo },
                    { Name: "ColProduct", Value: info.Product.Name },
                    { Name: "ColCountry", Value: info.Country.Name },
                    { Name: "ColCategory", Value: info.MainCategory.Name },
                    { Name: "ColSubcategory", Value: info.SubCategory.Name },
                    { Name: "ColRemark", Value: info.Remark },
                    { Name: "ColSolution", Value: info.Solution },
                    { Name: "ColStatus", Value: info.StatusName },
                    { Name: "ColResponse", Value: info.Response.Name },
                    { Name: "ColHandler", Value: info.Handler },
                    { Name: "ColCreationTime", Value: info.CreationTimeString}]
                });
            }
        }

        var breadcrumb = report ? report.Name : "";
        var backIcon = '<input class="btnGoBack" type="button" title="' + viewResources.GoBack + '" />';
        var gridOptions = {
            columns: [{ HeaderText: viewResources.Date, Width: 80, Name: "ColDate", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Start_Time, Width: 60, Name: "ColStartTime", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.End_Time, Width: 60, Name: "ColEndTime", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Duration, Width: 80, Name: "ColDuration", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Customer, Width: 140, Name: "ColCustomer", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Site, Width: 100, Name: "ColSite", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Type_Of_Contact, Width: 100, Name: "ColTypeOfContact", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Symbol_Extension, Width: 60, Name: "ColExtension", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Contact, Width: 220, Name: "ColContact", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Product, Width: 100, Name: "ColProduct", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Country, Width: 100, Name: "ColCountry", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Category, Width: 200, Name: "ColCategory", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Subcategory, Width: 200, Name: "ColSubcategory", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Complaints_Enquiries, Width: 480, Name: "ColRemark", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Solution, Width: 480, Name: "ColSolution", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Status, Width: 60, Name: "ColStatus", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Response, Width: 60, Name: "ColResponse", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Handle_By, Width: 100, Name: "ColHandler", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Creation_Time, Width: 80, Name: "ColCreationTime", HeaderAlign: "center", CellAlign: "center"}],
            showTitle: true,
            titleInfo: {
                text: breadcrumb,
                showExcelButton: true,
                exportExcelFunction: function () {
                    selfScope.exportDetailReport();
                },
                customButtons: backIcon,
                showButtonsWhenNoData: true
            },
            bodyRows: bodyData,
            footerRows: footerData,
            numberOfFrozenColumn: 5,
            fullWidth: true,
            fullHeight: true,
            gridExpandHeight: 200,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 29,
            bodyRowHeight: 29,
            autoFixFrozenRowHeight: true,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: selfScope.getDetailReportCriteria.PageSize,
                currentPage: selfScope.getDetailReportCriteria.PageIndex - 1,
                itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                totalItem: report ? report.TotalChildInfo : 0,
                pagerSelectCallBack: rerenderAfterPaged
            }
        };

        return gridOptions;
    }

    function backToMainReport(element) {
        element.addClass(crmConstants.Enums.CssClass.DisplayNone);
        $(".table-ibc-report-container").removeClass(crmConstants.Enums.CssClass.DisplayNone);
        $(".table-licensee-report-container").removeClass(crmConstants.Enums.CssClass.DisplayNone);
    }

    function rerenderAfterPaged(page_index, itemsPerPage, jq) {
        selfScope.pagingDetailReport(page_index, itemsPerPage);
        return false;
    }

    return {
        restrict: 'C',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (oldval, newval) {
                if (!scope.detailReportOptions ||
                    !scope.detailReportOptions.IsUpdateGrid) {
                    return;
                }

                renderReport(scope, element, attrs);
                scope.detailReportOptions.IsUpdateGrid = false;
            });
        }
    };
});