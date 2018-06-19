(function () {
    "use strict";
    /* global crmConstants,
    viewResources,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('report-weeklystatistics-application').directive('tableDetailReportContainer', function () {
        var selfScope;
        var cachedTable;
        var cachedReport;
        var cachedGridOptions;

        function renderReport(scope, element) {
            if (scope.detailReportOptions.DataList === null) {
                return;
            }

            selfScope = scope;
            var tableReport = element.find("#table-detail-report");
            cachedTable = tableReport;
            var report = scope.detailReportOptions.DataList;
            cachedReport = report;
            var gridOptions = getGridOptions(report, scope);
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

        function getGridOptions(report, scope) {
            var bodyData = [];
            var footerData = [];
            var childReportLength = report ? report.ChildReports.length : 0;
            if (childReportLength > 0) {
                for (var i = 0; i < childReportLength; i++) {
                    var info = report.ChildReports[i];

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
                    { Name: "ColCustomer", Value: info.Username, CssClass: 'word-wrap-customer' },
                    { Name: "ColSite", Value: info.SiteInfo.Name },
                    { Name: "ColTypeOfContact", Value: renderCommunicationColumn(info, scope.CommunicationToolList) },
                    { Name: "ColExtension", Value: renderExtensionColumn(info, scope.ExtensionList) },
                    { Name: "ColRole", Value: info.Role },
                    { Name: "ColContact", Value: info.Contact, CssClass: 'word-wrap-contactinfo' },
                    { Name: "ColProduct", Value: renderProductColumn(info, scope.ProductList, scope.SubProductList) },
                    { Name: "ColCountry", Value: info.Country.Name },
                    { Name: "ColCategory", Value: renderCategoryColumn(info, scope.mainCategory) },
                    { Name: "ColRemark", Value: info.Remark, CssClass: 'word-wrap-remark' },
                    { Name: "ColSolution", Value: info.Solution, CssClass: 'word-wrap-remark' },
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
                { HeaderText: viewResources.Role, Width: 60, Name: "ColRole", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Contact, Width: 220, Name: "ColContact", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Product, Width: 100, Name: "ColProduct", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Country, Width: 100, Name: "ColCountry", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Category/Sub-Category", Width: 200, Name: "ColCategory", HeaderAlign: "center", CellAlign: "center" },
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
                    totalItem: report ? report.TotalChildReports : 0,
                    pagerSelectCallBack: rerenderAfterPaged
                }
            };

            return gridOptions;
        }

        function getMainCategoryName(mainCategoryId, mainCategories) {
            for (var i = 0; i < mainCategories.length; i++) {
                if (mainCategories[i].Id == mainCategoryId) {
                    return mainCategories[i].Name;
                }
            }
        }

        function getSubCategoryName(mainCategoryId, subCategoryId, mainCategories) {
            var mainCategory = null;
            for (var i = 0; i < mainCategories.length; i++) {
                if (mainCategories[i].Id == mainCategoryId) {
                    mainCategory = mainCategories[i];
                    break;
                }
            }

            if (mainCategory) {
                for (var j = 0; j < mainCategory.ChildSettings.length; j++) {
                    if (mainCategory.ChildSettings[j].Id == subCategoryId) {
                        return mainCategory.ChildSettings[j].Name;
                    }
                }
            }

            return '';
        }

        function renderCategoryColumn(feedback, mainCategories) {
            var settings = feedback.Settings;

            var mainCategoryIds = [];
            for (var i = 0; i < settings.length; i++) {
                if (settings[i].SettingType == "Category" && mainCategoryIds.indexOf(settings[i].SettingIdLv1) < 0) {
                    mainCategoryIds.push(settings[i].SettingIdLv1);
                }
            }

            var categories = [];
            for (var i = 0; i < mainCategoryIds.length; i++) {
                var subCategoryIds = [];
                for (var j = 0; j < settings.length; j++) {
                    if (settings[j].SettingType == "Category" && settings[j].SettingIdLv1 == mainCategoryIds[i]) {
                        subCategoryIds.push(settings[j].SettingIdLv2);
                    }
                }

                categories.push({
                    mainCategoryId: mainCategoryIds[i],
                    subCategoryIds: subCategoryIds
                })
            }

            var result = '';
            for (var i = 0; i < categories.length; i++) {
                var categoryHtml = '<div class="main-category">' + getMainCategoryName(categories[i].mainCategoryId, mainCategories);
                var subCategoryIds = categories[i].subCategoryIds;
                for (var j = 0; j < subCategoryIds.length; j++) {
                    categoryHtml += '<div><span class="sub-category">' + getSubCategoryName(categories[i].mainCategoryId, subCategoryIds[j], mainCategories) + '</span></div>';
                }
                categoryHtml += '</div>';
                result += categoryHtml;
            }

            return result;
        }

        function getProductName(productId, productList) {
            for (var i = 0; i < productList.length; i++) {
                if (productList[i].Id == productId) {
                    return productList[i].Name;
                }
            }

            return "N/A";
        }

        function renderProductColumn(feedback, productList, subProductList) {
            var settings = feedback.Settings;

            var productIds = [];
            for (var i = 0; i < settings.length; i++) {
                if (settings[i].SettingType == "Product" && productIds.indexOf(settings[i].SettingIdLv1) < 0) {
                    productIds.push(settings[i].SettingIdLv1);
                }
            }

            var products = [];
            for (var i = 0; i < productIds.length; i++) {
                var subProductIds = [];
                for (var j = 0; j < settings.length; j++) {
                    if (settings[j].SettingType == "Product"
                            && settings[j].SettingIdLv1 == productIds[i]
                            && subProductIds.indexOf(settings[j].SettingIdLv2) < 0) {
                        subProductIds.push(settings[j].SettingIdLv2);
                    }
                }

                products.push({
                    productId: productIds[i],
                    subProductIds: subProductIds
                })
            }

            var result = '';
            for (var i = 0; i < products.length; i++) {
                var productHtml = '<div class="main-category">' + getProductName(products[i].productId, productList);
                var subProductIds = products[i].subProductIds;
                for (var j = 0; j < subProductIds.length; j++) {
                    productHtml += '<div><span class="sub-category">' + getProductName(subProductIds[j], subProductList) + '</span></div>';
                }
                productHtml += '</div>';
                result += productHtml;
            }

            return result;
        }

        function getCommunicationName(id, communicationList) {
            for (var i = 0; i < communicationList.length; i++) {
                if (communicationList[i].Id == id) {
                    return communicationList[i].Name;
                }
            }

            return "N/A";
        }

        function renderCommunicationColumn(feedback, communicationList) {
            var settings = feedback.Settings;

            var result = '';
            for (var i = 0; i < settings.length; i++) {
                if (settings[i].SettingType == "Communication") {
                    result += '<div class="">' + getCommunicationName(settings[i].SettingIdLv1, communicationList) + '</div>';
                }
            }

            return result;
        }


        function getExtensionName(id, extensionList) {
            for (var i = 0; i < extensionList.length; i++) {
                if (extensionList[i].Id == id) {
                    return extensionList[i].Value;
                }
            }

            return "N/A";
        }

        function renderExtensionColumn(feedback, extensionList) {
            var settings = feedback.Settings;

            var result = '';
            for (var i = 0; i < settings.length; i++) {
                if (settings[i].SettingType == "Station") {
                    result += '<div class="">' + getExtensionName(settings[i].SettingIdLv1, extensionList) + '</div>';
                }
            }

            return result;
        }

        function backToMainReport(element) {
            element.addClass(crmConstants.Enums.CssClass.DisplayNone);
            $(".table-ibc-report-container").removeClass(crmConstants.Enums.CssClass.DisplayNone);
            $(".table-licensee-report-container").removeClass(crmConstants.Enums.CssClass.DisplayNone);
        }

        function rerenderAfterPaged(page_index, itemsPerPage) {
            selfScope.pagingDetailReport(page_index, itemsPerPage);
            return false;
        }

        return {
            restrict: 'C',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (oldval, newval) {
                    /* jshint unused: false */
                    if (!scope.detailReportOptions ||
                    !scope.detailReportOptions.IsUpdateGrid) {
                        return;
                    }

                    renderReport(scope, element);
                    scope.detailReportOptions.IsUpdateGrid = false;
                });
            }
        };
    });
})();