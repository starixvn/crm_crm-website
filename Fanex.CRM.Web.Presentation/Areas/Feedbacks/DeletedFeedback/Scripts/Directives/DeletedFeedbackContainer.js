(function () {
    "use strict";
    /* global deletedFeedbackResources,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('deleted-feedback-application').directive('tableDeletedFeedbackContainer', function () {
        var gridOptions, tableDeletedFeedback;
        function getGridOptions(scope, bodyData, totalItems) {
            function ReRenderAfterPaged(page_index, itemsPerPage) {
                gridOptions.pagerOption.currentPage = page_index;
                gridOptions.pagerOption.itemsPerPage = itemsPerPage;
                tableDeletedFeedback.fanexGrid(gridOptions);

                scope.searchCriteria.PageIndex = page_index;
                scope.searchCriteria.ItemsPerPage = itemsPerPage;
                scope.searchManageFeedback();
                return false;
            }

            return {
                columns: [
                    { HeaderText: deletedFeedbackResources.Feedback, Width: 380, Name: 'ColFeedback', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: deletedFeedbackResources.Customer, Width: 130, Name: 'ColCustomer', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: deletedFeedbackResources.Category, Width: 270, Name: 'ColCategory', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: deletedFeedbackResources.Status, Width: 50, Name: 'ColStatus', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: deletedFeedbackResources.RelatedCustomer, Width: 130, Name: 'ColRelatedCustomer', HeaderAlign: 'center', CellAlign: 'left'}],
                showTitle: true,
                titleInfo: {
                    text: deletedFeedbackResources.DeletedFeedbackTitle,
                    showExcelButton: true,
                    exportExcelFunction: function () {
                        scope.exportFeedbackExcel();
                    }
                },
                bodyRows: bodyData,
                showFooter: false,
                numberOfFrozenColumn: 1,
                autoFixFrozenRowHeight: true,
                fullWidth: true,
                fullHeight: true,
                gridExpandHeight: 180,
                showDistinctRow: false,
                showHighlightRow: false,
                headerColumnHeight: 29,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: scope.searchCriteria.ItemsPerPage,
                    currentPage: scope.searchCriteria.PageIndex,
                    itemsPerPageArray: sharedScriptManager.cachedData.largePagingOptions,
                    totalItem: totalItems,
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };
        }

        function getGridBody(feedbacks, mainCategories) {
            var fbLength = feedbacks.length;
            if (fbLength === 0) { return []; }

            var bodyData = [];
            var fb;
            for (var i = 0; i < fbLength; i++) {
                fb = feedbacks[i];
                bodyData.push({ Columns: [
                { Name: 'ColFeedback', Value: renderFeedbackColumn(fb) },
                { Name: 'ColCustomer', Value: fb.Username, CssClass: 'word-wrap' },
                { Name: 'ColCategory', Value: renderCategoryColumn(fb, mainCategories) },
                { Name: 'ColStatus', Value: fb.Status ? fb.Status : 'N/A' },
                { Name: 'ColRelatedCustomer', Value: fb.IsRelated ? fb.Username : fb.RelatedCustomer, CssClass: 'word-wrap'}]
                });
            }

            return bodyData;
        }

        function renderDeletedFeedback(report, scope, element) {
            if (!report) { return; }

            var totalItems = report.TotalItems;
            var feedbacks = totalItems > 0 ? report.Feedbacks : [];
            var bodyData = getGridBody(feedbacks, report.Settings.MainCategory);
            gridOptions = getGridOptions(scope, bodyData, totalItems);

            tableDeletedFeedback = element.find('#table-deleted-feedback');
            tableDeletedFeedback
            .empty()
            .removeClass('hide')
            .fanexGrid(gridOptions);

            if (totalItems > 0) {
                $('.desTooltip')
                .unbind('click')
                .bind('click', function () {
                    scope.openEditFeedbackPopup($(this).data('id'));
                });
            }
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

        function renderFeedbackColumn(feedback) {
            var html = '<div style="margin:2px 0 0 2px;"> \
            <div class="lnksubject">\
                <a class="desTooltip" href="javascript:;" data-id="' + feedback.FeedbackId + '">\
                    <span class="b">' + sharedScriptManager.encodeHtml(feedback.Subject) + '</span>\
                </a>\
            </div>\
            <div class="description">\
                <div>Feedback date <span class="b">' + feedback.FeedbackDateString + '</span></div>\
                <div>By <span class="b">' + feedback.CreatorName + '</span> on ' + feedback.CreateDateString + '</div>';

            var reviewer = (feedback.ReviewerName && feedback.ReviewerName.length > 0) ?
        '<div>Reviewed by <span class="b">' + feedback.ReviewerName + '</span> on ' + feedback.SolveDateString + '</div>' :
        '';
            return html + reviewer + '</div></div>';
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newValues, oldValues) {
                    /* jshint unused: false */
                    renderDeletedFeedback(newValues, scope, element);
                });
            }
        };
    });
})();