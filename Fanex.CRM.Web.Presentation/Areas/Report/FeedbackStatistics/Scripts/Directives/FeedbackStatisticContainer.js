'use strict';
/* global fbStatisticApplication,
feedbackStatisticResources,
crmConstants,
sharedScriptManager */
/* jshint camelcase: false */
/* jshint unused: vars */
fbStatisticApplication.directive('tableFeedbackStatisticContainer', function () {
    var gridOptions, tableFeedbackStatistic;
    function getUsernameColumn(name) {
        return {
            HeaderText: name,
            Width: 150,
            Name: 'Col' + name,
            HeaderAlign: 'center',
            CellAlign: 'center',
            FormatNumber: crmConstants.Enums.FormatConstant.IntNumber
        };
    }

    function getGridOptions(scope, reportTitle, bodyData, footerData, usernames) {
        function ReRenderAfterPaged(page_index, itemsPerPage, jq) {
            gridOptions.pagerOption.currentPage = page_index;
            gridOptions.pagerOption.itemsPerPage = itemsPerPage;
            tableFeedbackStatistic.fanexGrid(gridOptions);

            scope.searchCriteria.PageSize = itemsPerPage;
            scope.searchCriteria.PageIndex = page_index;
            scope.getFeedbackStatistic(page_index, itemsPerPage);
            return false;
        }

        var columns = [{
            HeaderText: feedbackStatisticResources.SubCategory,
            Width: 400,
            Name: 'ColSubCategory',
            HeaderAlign: 'center',
            CellAlign: 'left',
            ShowCollapseExpand: true
        }];

        for (var i = 0, leng = usernames.length; i < leng; i++) {
            columns.push(getUsernameColumn(usernames[i].Name));
        }

        return {
            columns: columns,
            showTitle: true,
            titleInfo: {
                text: reportTitle,
                showExcelButton: true,
                showCollapseExpandButton: true,
                exportExcelFunction: function () {
                    scope.exportFeedbackStatistic();
                },
                customButtons: '<input id="button-viewchart" class="btnCharting" type="button" title="' + feedbackStatisticResources.ViewChart + '" />\
                    <input id="button-viewdetail" class="btnViewDetail" type="button" title="' + feedbackStatisticResources.ViewDetail + '" />'
            },
            bodyRows: bodyData,
            showFooter: true,
            showTree: true,
            footerRows: footerData,
            numberOfFrozenColumn: 1,
            fullWidth: true,
            fullHeight: true,
            gridExpandHeight: 180,
            showDistinctRow: false,
            showHighlightRow: false,
            headerColumnHeight: 29,
            autoFixFrozenRowHeight: true,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: scope.searchCriteria.PageSize,
                currentPage: scope.searchCriteria.PageIndex,
                itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                totalItem: scope.feedbackStatistic.TotalItems,
                pagerSelectCallBack: ReRenderAfterPaged
            }
        };
    }

    function getGridBody(scope, feedbackCategories, usernames) {
        var bodyData = [];
        if (feedbackCategories) {
            var subLen;
            var fbCategory, fbSubCategory, currentRow, currentSubRow;
            var usernamesLen = usernames.length;
            // view chart.
            var categories = [];
            for (var i = 0, fbCategoryLen = feedbackCategories.length; i < fbCategoryLen; i++) {
                fbCategory = feedbackCategories[i];
                subLen = fbCategory.FeedbackSubcategories ? fbCategory.FeedbackSubcategories.length : 0;
                currentRow = {
                    Columns: [
                        { Name: 'ColSubCategory', Value: fbCategory.Name + ' - ' + subLen + ' ' + feedbackStatisticResources.Items },
                        { Name: 'Col' + usernames[0].Name, Value: '', MergeColNumber: usernamesLen }
                    ],
                    SubRows: []
                };

                for (var j = 0; j < subLen; j++) {
                    fbSubCategory = fbCategory.FeedbackSubcategories[j];

                    currentSubRow = { Columns: [{ Name: 'ColSubCategory', Value: fbSubCategory.Name + ' (' + fbSubCategory.Total + ')'}] };
                    for (var k = 0; k < usernamesLen; k++) {
                        currentSubRow.Columns.push({
                            Name: 'Col' + usernames[k].Name,
                            Value: buildResolveLink(fbSubCategory.Resolves[k], fbCategory.Id, fbSubCategory.Id, usernames[k].Id)
                        });
                    }

                    currentRow.SubRows.push(currentSubRow);
                }

                // Sub-total
                currentSubRow = {
                    Columns: [{ Name: 'ColSubCategory', Value: feedbackStatisticResources.SubTotal + ' (' + fbCategory.SubTotal.Total + ')'}],
                    CssClass: ' bold'
                };
                for (var l = 0; l < usernamesLen; l++) {
                    currentSubRow.Columns.push({
                        Name: 'Col' + usernames[l].Name,
                        Value: buildResolveLink(fbCategory.SubTotal.Resolves[l], fbCategory.Id, 0, usernames[l].Id)
                    });
                }

                currentRow.SubRows.push(currentSubRow);
                // ---------- End of Sub-total ----------

                bodyData.push(currentRow);
                categories.push(fbCategory.Name);
            }

            // view chart.
            if (usernamesLen > 0) {
                scope.feedbackStatisticChart = { labels: categories, datasets: buildChartData(feedbackCategories, usernames) };
            }
        }

        return bodyData;
    }

    function buildResolveLink(resolve, categoryId, subCategoryId, valueId) {
        var customData = 'data-category="' + categoryId + '" data-subcategory="' + subCategoryId + '" data-value="' + valueId + '"';
        return resolve === 0 ? resolve : '<div class="link" ' + customData + '">' + resolve + '</div>';
    }

    function getGridFooter(grandTotals, usernames) {
        var footerData = [];
        var cell = { Columns: [{ Name: 'ColSubCategory', Value: feedbackStatisticResources.GrandTotal}] };

        for (var i = 0, usernamesLen = usernames.length; i < usernamesLen; i++) {
            cell.Columns.push({ Name: 'Col' + usernames[i].Name, Value: buildResolveLink(grandTotals[i], 0, 0, usernames[i].Id) });
        }

        footerData.push(cell);
        return footerData;
    }

    function buildChartData(feedbackCategories, usernames) {
        var datasets = [];
        var bgColor, bgColorIndex = 0;
        var bgColorFunction = parent.crmChart.getBackGroundColor;
        var resolves;

        var fbCategory;
        var usernamesLen = usernames.length;
        for (var i = 0; i < usernamesLen; i++) {
            resolves = [];
            for (var j = 0, fbCategoryLen = feedbackCategories.length; j < fbCategoryLen; j++) {
                fbCategory = feedbackCategories[j];
                resolves.push(fbCategory.SubTotal.Resolves[i]);
            }

            bgColor = bgColorFunction(bgColorIndex++);
            datasets.push({ fillColor: bgColor, strokeColor: bgColor, title: usernames[i].Name, data: resolves });
        }

        return datasets;
    }

    function renderFeedbackStatistic(scope, element, attrs) {
        var report = scope.feedbackStatistic;
        if (!report || report.TotalItems === 0) {
            element.addClass('hide');
            return;
        }

        element.removeClass('hide');
        var reportTitle = report.Title;
        var feedbackCategories = report.FeedbackCategories;
        var grandTotals = report.GrandTotals;
        var usernames = report.Usernames;

        var bodyData = getGridBody(scope, feedbackCategories, usernames);
        var footerData = getGridFooter(grandTotals, usernames);

        gridOptions = getGridOptions(scope, reportTitle, bodyData, footerData, usernames);
        tableFeedbackStatistic = element.find('#table-feedback-statistic');
        tableFeedbackStatistic.empty().removeClass('hide').fanexGrid(gridOptions);
        // view chart event.
        element.find('#button-viewchart').click(function () {
            scope.viewFeedbackStatisticChart();
        });
        element.find('#button-viewdetail').click(function () {
            scope.openFeedbackStatisticDetail(0, 0, 0);
        });
        element.find('.link').click(function () {
            var $this = $(this);
            scope.openFeedbackStatisticDetail($this.data('category'), $this.data('subcategory'), $this.data('value'));
        });
    }

    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            scope.$watch('feedbackStatistic', function (newValue, oldValue) {
                if (!newValue) {
                    return;
                }

                renderFeedbackStatistic(scope, element, attrs);
            });
        }
    };
});