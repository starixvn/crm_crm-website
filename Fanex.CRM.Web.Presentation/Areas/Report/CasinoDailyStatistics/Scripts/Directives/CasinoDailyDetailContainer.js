(function () {
    "use strict";
    /* global casinoDailyResources,
    sharedScriptManager */
    /* jshint camelcase: false */
    /* jshint unused: vars */
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
            { HeaderText: casinoDailyResources.Category, Width: 200, Name: 'ColMainCategory', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: casinoDailyResources.SubCategory, Width: 200, Name: 'ColSubCategory', HeaderAlign: 'center', CellAlign: 'left' },
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

        function getGridBody(casinoDetails) {
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
                { Name: 'ColCommunication', Value: detail.Communication },
                { Name: 'ColProduct', Value: detail.Product },
                { Name: 'ColStation', Value: detail.Station },
                { Name: 'ColRole', Value: detail.Role },
                { Name: 'ColContact', Value: detail.Contact, CssClass: 'word-wrap-contactinfo' },
                { Name: 'ColCountry', Value: detail.CountryName },
                { Name: 'ColMainCategory', Value: detail.MainCategory },
                { Name: 'ColSubCategory', Value: detail.SubCategory },
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

        function renderCasinoDailyDetail(scope, element) {
            var report = scope.dailyDetail;

            var casinoDetails = report.CasinoDetails;
            var bodyData = casinoDetails ? getGridBody(casinoDetails) : [];
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