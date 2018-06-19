(function () {
    "use strict";
    /* global casinoDailyResources,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('report-casinodaily-application').directive('tableDailyDetailContainer', function () {
        var tableDailyDetail, gridOptions;
        function getGridOptions(scope, report, bodyData) {
            var columns = [
            { HeaderText: casinoDailyResources.Date, Width: 80, Name: 'ColDate', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: casinoDailyResources.StartTime, Width: 60, Name: 'ColStartTime', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: casinoDailyResources.EndTime, Width: 60, Name: 'ColEndTime', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: casinoDailyResources.Duration, Width: 60, Name: 'ColDuration', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: casinoDailyResources.Customer, Width: 140, Name: 'ColUserName', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: casinoDailyResources.Site, Width: 100, Name: 'ColSite', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: casinoDailyResources.TypeOfContact, Width: 100, Name: 'ColCommunication', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: casinoDailyResources.ProductName, Width: 100, Name: 'ColProduct', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: casinoDailyResources.Ext, Width: 60, Name: 'ColStation', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: casinoDailyResources.Role, Width: 60, Name: 'ColRole', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: casinoDailyResources.Contact, Width: 220, Name: 'ColContact', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: casinoDailyResources.Country, Width: 110, Name: 'ColCountry', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: "Category/Sub-Category", Width: 200, Name: 'ColMainCategory', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: casinoDailyResources.Description, Width: 480, Name: 'ColDescription', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: casinoDailyResources.Solution, Width: 480, Name: 'ColSolution', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: casinoDailyResources.Status, Width: 60, Name: 'ColStatus', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: casinoDailyResources.Response, Width: 60, Name: 'ColResponse', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: casinoDailyResources.HandleBy, Width: 100, Name: 'ColCreator', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: casinoDailyResources.CreationTime, Width: 80, Name: 'ColCreationTime', HeaderAlign: 'center', CellAlign: 'center' }
        ];

            return {
                columns: columns,
                showTitle: true,
                titleInfo: {
                    text: report.Title,
                    showExcelButton: true,
                    exportExcelFunction: function () {
                        scope.exportCasinoDailyDetail();
                    },
                    customButtons: '<input id="button-back" class="btnGoBack" type="button" title="' + casinoDailyResources.GoBack + '"/>',
                    showButtonsWhenNoData: true
                },
                bodyRows: bodyData,
                showFooter: false,
                numberOfFrozenColumn: 5,
                autoFixFrozenRowHeight: true,
                fullWidth: true,
                fullHeight: true,
                gridExpandHeight: 140,
                showDistinctRow: false,
                showHighlightRow: false,
                headerColumnHeight: 29,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: report.ItemsPerPage,
                    currentPage: report.CurrentPage,
                    itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                    totalItem: report.TotalItems,
                    pagerSelectCallBack: function (page_index, itemsPerPage) {
                        gridOptions.pagerOption.currentPage = page_index;
                        gridOptions.pagerOption.itemsPerPage = itemsPerPage;
                        tableDailyDetail.fanexGrid(gridOptions);

                        scope.getCasinoDailyDetail(page_index, itemsPerPage);
                        return false;
                    }
                }
            };
        }

        function getGridBody(casinoDetails, scope) {
            var bodyData = [];

            var detail, currentColumn;
            var reportLen = casinoDetails.length;
            for (var i = 0; i < reportLen; i++) {
                detail = casinoDetails[i];
                currentColumn = [
                { Name: 'ColDate', Value: detail.FeedbackDateString },
                { Name: 'ColStartTime', Value: detail.FeedbackTimeString },
                { Name: 'ColEndTime', Value: detail.ResolutionTimeString },
                { Name: 'ColDuration', Value: detail.Duration },
                { Name: 'ColUserName', Value: detail.UserName, CssClass: 'word-wrap-customer' },
                { Name: 'ColSite', Value: detail.SiteName },
                { Name: 'ColCommunication', Value: renderCommunicationColumn(detail, scope.CommunicationToolList) },
                { Name: 'ColProduct', Value: renderProductColumn(detail, scope.ProductList, scope.SubProductList) },
                { Name: 'ColStation', Value: renderExtensionColumn(detail, scope.ExtensionList) },
                { Name: 'ColRole', Value: detail.Role },
                { Name: 'ColContact', Value: detail.Contact, CssClass: 'word-wrap-contactinfo' },
                { Name: 'ColCountry', Value: detail.CountryName },
                { Name: 'ColMainCategory', Value: renderCategoryColumn(detail, scope.mainCategory) },
                { Name: 'ColDescription', Value: detail.DescriptionWithoutSpecialHtmlTag, CssClass: 'word-wrap-remark' },
                { Name: 'ColSolution', Value: detail.SolutionWithoutSpecialHtmlTag, CssClass: 'word-wrap-remark' },
                { Name: 'ColStatus', Value: detail.Status },
                { Name: 'ColResponse', Value: detail.Response },
                { Name: 'ColCreator', Value: detail.CreatorName },
                { Name: 'ColCreationTime', Value: detail.CreateDateString }
            ];

                bodyData.push({ Columns: currentColumn });
            }

            return bodyData;
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

        function renderCasinoDailyDetail(scope, element) {
            var report = scope.dailyDetail;

            var casinoDetails = report.CasinoDetails;
            var bodyData = casinoDetails ? getGridBody(casinoDetails, scope) : [];
            gridOptions = getGridOptions(scope, report, bodyData);

            tableDailyDetail = element.find('#table-daily-detail');
            tableDailyDetail.empty().removeClass('hide').fanexGrid(gridOptions);
            element.find('#button-back').click(function () {
                scope.showSummary = true;
                scope.$apply();
            });
        }

        return {
            restrict: 'A',
            link: function (scope, element) {
                scope.$watch('dailyDetail', function (newValue, oldValue) {
                    /* jshint unused: false */
                    if (!newValue) {
                        return;
                    }

                    renderCasinoDailyDetail(scope, element);
                });
            }
        };
    });
})();