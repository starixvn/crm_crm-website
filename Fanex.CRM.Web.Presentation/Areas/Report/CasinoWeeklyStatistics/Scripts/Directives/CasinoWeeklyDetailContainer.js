'use strict';
/* global casinoWeeklyApplication,
casinoWeeklyResources,
sharedScriptManager */
/* jshint camelcase: false */
/* jshint unused: vars */
casinoWeeklyApplication.directive('tableWeeklyDetailContainer', function () {
    function getGridOptions(scope, report, bodyData) {
        var columns = [
            { HeaderText: casinoWeeklyResources.Date, Width: 80, Name: 'ColDate', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: casinoWeeklyResources.StartTime, Width: 60, Name: 'ColStartTime', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: casinoWeeklyResources.EndTime, Width: 60, Name: 'ColEndTime', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: casinoWeeklyResources.Duration, Width: 60, Name: 'ColDuration', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: casinoWeeklyResources.Customer, Width: 140, Name: 'ColUserName', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: casinoWeeklyResources.Site, Width: 100, Name: 'ColSite', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: casinoWeeklyResources.TypeOfContact, Width: 100, Name: 'ColCommunication', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: casinoWeeklyResources.ProductName, Width: 100, Name: 'ColProduct', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: casinoWeeklyResources.Ext, Width: 60, Name: 'ColStation', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: casinoWeeklyResources.Contact, Width: 220, Name: 'ColContact', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: casinoWeeklyResources.Country, Width: 110, Name: 'ColCountry', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: casinoWeeklyResources.Category, Width: 200, Name: 'ColMainCategory', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: casinoWeeklyResources.SubCategory, Width: 200, Name: 'ColSubCategory', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: casinoWeeklyResources.Description, Width: 480, Name: 'ColDescription', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: casinoWeeklyResources.Solution, Width: 480, Name: 'ColSolution', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: casinoWeeklyResources.Status, Width: 60, Name: 'ColStatus', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: casinoWeeklyResources.Response, Width: 60, Name: 'ColResponse', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: casinoWeeklyResources.HandleBy, Width: 100, Name: 'ColCreator', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: casinoWeeklyResources.CreationTime, Width: 80, Name: 'ColCreationTime', HeaderAlign: 'center', CellAlign: 'center' }
        ];

        return {
            columns: columns,
            showTitle: true,
            titleInfo: {
                text: report.Title,
                customButtons: '<input id="button-back" class="btnGoBack" type="button" title="' + casinoWeeklyResources.GoBack + '"/>',
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
                currentPage: report.PageIndex,
                itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                totalItem: report.TotalItems,
                pagerSelectCallBack: function (page_index, itemsPerPage) {
                    scope.getCasinoWeeklyDetail(page_index, itemsPerPage);
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
                { Name: 'ColUserName', Value: detail.UserName },
                { Name: 'ColSite', Value: detail.SiteName },
                { Name: 'ColCommunication', Value: detail.Communication },
                { Name: 'ColProduct', Value: detail.Product },
                { Name: 'ColStation', Value: detail.Station },
                { Name: 'ColContact', Value: buildContact(detail.MSN, detail.Skype, detail.Email, detail.Phone, detail.MobilePhone) },
                { Name: 'ColCountry', Value: detail.CountryName },
                { Name: 'ColMainCategory', Value: detail.MainCategory },
                { Name: 'ColSubCategory', Value: detail.SubCategory },
                { Name: 'ColDescription', Value: detail.DescriptionWithoutSpecialHtmlTag },
                { Name: 'ColSolution', Value: detail.SolutionWithoutSpecialHtmlTag },
                { Name: 'ColStatus', Value: detail.Status },
                { Name: 'ColResponse', Value: detail.Response },
                { Name: 'ColCreator', Value: detail.CreatorName },
                { Name: 'ColCreationTime', Value: detail.CreateDateString }
            ];

            bodyData.push({ Columns: currentColumn });
        }

        return bodyData;
    }

    function buildContact(msn, skype, email, phone, mobile) {
        return casinoWeeklyResources.MSN + ': ' + (msn ? sharedScriptManager.encodeHtml(msn) : '-') +
            '<br/>' + casinoWeeklyResources.Skype + ': ' + (skype ? sharedScriptManager.encodeHtml(skype) : '-') +
            '<br/>' + casinoWeeklyResources.Email + ': ' + (email ? sharedScriptManager.encodeHtml(email) : '-') +
            '<br/>' + casinoWeeklyResources.Phone + ': ' + (phone ? sharedScriptManager.encodeHtml(phone) : '-') +
            '<br/>' + casinoWeeklyResources.Mobile + ': ' + (mobile ? sharedScriptManager.encodeHtml(mobile) : '-');
    }

    function renderCasinoWeeklyDetail(scope, element, attrs) {
        var report = scope.weeklyDetail;
        
        var casinoDetails = report.CasinoDetails;
        var bodyData = casinoDetails ? getGridBody(casinoDetails) : [];
        var gridOptions = getGridOptions(scope, report, bodyData);

        var grid = element.find('#table-weekly-detail');
        grid.empty().removeClass('hide').fanexGrid(gridOptions);
        element.find('#button-back').click(function () {
            scope.showSummary = true;
            scope.$apply();
        });
    }

    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            scope.$watch('weeklyDetail', function (newValue, oldValue) {
                if (!newValue) {
                    return;
                }

                renderCasinoWeeklyDetail(scope, element, attrs);
            });
        }
    };
});