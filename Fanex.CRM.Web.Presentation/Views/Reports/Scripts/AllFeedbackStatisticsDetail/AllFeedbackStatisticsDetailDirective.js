"use strict";
/* global allFeedbackStatisticsDetailApplication,
sharedScriptManager,
crmConstants*/

allFeedbackStatisticsDetailApplication.directive('tableFeedbacksContainer', function () {

    function durationTimeFormat(time) {
        var html = '';
        if (time) {
            html = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Solving Duration: " + time;
        }
        return html;
    }

    function reviewerFormat(reviewer, reviewDate) {
        var html = '';
        if (reviewer !== "") {
            html = '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Reviewed&nbsp;by&nbsp;<b>' + reviewer + '</b>&nbsp;on&nbsp;' + reviewDate + '<br>';
        }
        return html;
    }

    function renderFeedbacks(scope, element) {
        /* jshint unused: vars */
        if (scope.feedbacks === null) {
            return;
        }

        var gridOptions;
        /* jshint unused: vars */
        function ReRenderAfterPaged(page_index, itemsPerPage, jq) {            
            gridOptions.pagerOption.currentPage = page_index;
            gridOptions.pagerOption.itemsPerPage = itemsPerPage;
            tableFeedbacks.fanexGrid(gridOptions);

            scope.pagingFeedbackStatisticsDetail(page_index, itemsPerPage);
            return false;
        }

        var tableFeedbacks = element.find("#table-feedbackstatistics");
        var feedbacks = scope.feedbacks.rows;
        var bodyData = [];
        var footerData = [];

        tableFeedbacks.empty();
        if (feedbacks && feedbacks.length > 0) {
            var totalFeedbacks = feedbacks.length;
            for (var i = 0; i < totalFeedbacks; i++) {
                var info = feedbacks[i];
                bodyData.push({
                    Columns: [
                        {
                            Name: "ColFeedback",
                            Value: '<div style="width:380px">\
                                        <div class="lnksubject">\
                                            <a class="desTooltip" href="JavaScript:void(0);" data-logindex="' + i + '">\
                                                <b>' + info.Subject + '</b>\
                                            </a>\
                                        </div>\
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Feedback&nbsp;date&nbsp;<b>' + info.FeedbackDate + '</b>\
                                        <br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;By&nbsp;<b>' + info.Creator + '</b> on ' + info.CreateDate +
                                        '<br>' + durationTimeFormat(info.DurationTime) +
                                        '<br>' + reviewerFormat(info.Reviewer, info.ReviewDate) +
                                '</div>',
                            TdClass: 'editlog-link-td'
                        },
                        {
                            Name: "ColCustomer",
                            /* jshint camelcase: false */
                            Value: info.Username
                        },
                        {
                            Name: "ColMainCategory",
                            Value: info.MainCategory
                        },
                        {
                            Name: "ColSubcategory",
                            Value: info.SubCategory
                        },
                        {
                            Name: "ColStatus",
                            Value: info.Status
                        },
                        {
                            Name: "CoRelatedCustomer",
                            Value: info.RelatedUsername
                        }],
                    CssClass: 'editlog-link-row row-' + info.Status.toLowerCase()
                });
            }
        }

        gridOptions = {
            columns: [
                { HeaderText: 'Feedback', Width: 380, Name: "ColFeedback", HeaderAlign: "center", CellAlign: "left", Sort: false },
                { HeaderText: 'Customer', Width: 130, Name: "ColCustomer", HeaderAlign: "center", CellAlign: "center", Sort: false },
                { HeaderText: 'Main Category', Width: 200, Name: "ColMainCategory", HeaderAlign: "center", CellAlign: "left", Sort: false },
                { HeaderText: 'Subcategory', Width: 200, Name: "ColSubcategory", HeaderAlign: "center", CellAlign: "left", Sort: false },
                { HeaderText: 'Status', Width: 60, Name: "ColStatus", HeaderAlign: "center", CellAlign: "center", Sort: false },
                { HeaderText: 'Related Customer', Width: 180, Name: "CoRelatedCustomer", HeaderAlign: "center", CellAlign: "center", Sort: false}],
            showTitle: true,
            titleInfo: {
                text: 'Feedback Statistic Detail Report from ' + scope.requestParams.param.FromDate + ' to ' + scope.requestParams.param.ToDate,
                showExcelButton: true,
                exportExcelFunction: scope.exportDataToExcelFile
            },
            bodyRows: bodyData,
            footerRows: footerData,
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 0,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 34,
            bodyRowHeight: 51,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: sharedScriptManager.cachedData.defaultItemsPerPage,
                currentPage: scope.requestParams.param.CurrentPage,
                itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                totalItem: scope.feedbacks.total,
                pagerSelectCallBack: ReRenderAfterPaged
            }
        };

        tableFeedbacks.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
    }

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (oldval, newval) {
                /* jshint unused: vars */
                renderFeedbacks(scope, element);
            });
        }
    };
});