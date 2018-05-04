(function () {
    "use strict";
    /* global manageFeedbackResources,
    crmConstants,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('feedback-manage-application').directive('tableManageFeedbackContainer', function () {
        var gridOptions, tableManageFeedback;
        function getGridOptions(scope, bodyData, totalItems) {
            function ReRenderAfterPaged(page_index, itemsPerPage) {
                gridOptions.pagerOption.currentPage = page_index;
                gridOptions.pagerOption.itemsPerPage = itemsPerPage;
                tableManageFeedback.fanexGrid(gridOptions);

                scope.searchCriteria.PageIndex = page_index;
                scope.searchCriteria.ItemsPerPage = itemsPerPage;
                scope.searchManageFeedback();
                return false;
            }

            return {
                columns: [
                    {
                        HeaderText: '<input type="checkbox" class="checkActionAll"/>',
                        Name: "Edit",
                        Width: 50,                        
                        HeaderAlign: 'center', CellAlign: 'center',
                    },
                    { HeaderText: manageFeedbackResources.Feedback, Width: 380, Name: 'ColFeedback', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: manageFeedbackResources.Customer, Width: 130, Name: 'ColCustomer', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: manageFeedbackResources.Category, Width: 270, Name: 'ColCategory', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: manageFeedbackResources.Status, Width: 50, Name: 'ColStatus', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: manageFeedbackResources.RelatedCustomer, Width: 130, Name: 'ColRelatedCustomer', HeaderAlign: 'center', CellAlign: 'left'}],
                showTitle: true,
                titleInfo: {
                    text: renderTitle(scope),
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
                gridExpandHeight: 210,
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
            var fb, currentRow, status, cssClass;
            for (var i = 0; i < fbLength; i++) {
                fb = feedbacks[i];
                var checkedClass = fb.IsChecked ? 'checked="checked"' : '';
                currentRow = [
                { Name: 'Edit', Value: '<input type="checkbox" class="checkActionItem" data-id="' + fb.FeedbackId + '" />' },
                { Name: 'ColFeedback', Value: renderFeedbackColumn(fb) },
                { Name: 'ColCustomer', Value: fb.Username, CssClass: 'word-wrap' },
                { Name: 'ColCategory', Value: renderCategoryColumn(fb, mainCategories) },
                { Name: 'ColStatus', Value: fb.Status ? fb.Status : 'N/A' },
                { Name: 'ColRelatedCustomer', Value: fb.IsRelated ? fb.Username : fb.RelatedCustomer, CssClass: 'word-wrap' }
            ];

                status = fb.Status ? fb.Status.toLowerCase() : '';
                cssClass = (status !== 'pending' && status !== 'closed') ? '' : status;
                bodyData.push({ Columns: currentRow, CssClass: cssClass });
            }

            return bodyData;
        }

        function renderManageFeedback(report, scope, element) {
            if (!report) { return; }

            var totalItems = report.TotalItems;
            var feedbacks = totalItems > 0 ? report.Feedbacks : [];
            var bodyData = getGridBody(feedbacks, report.MainCategory);
            gridOptions = getGridOptions(scope, bodyData, totalItems);

            tableManageFeedback = element.find('#table-manage-feedback');
            tableManageFeedback
            .empty()
            .removeClass(crmConstants.Enums.CssClass.DisplayNone)
            .fanexGrid(gridOptions);

            if (totalItems > 0) {
                $('.desTooltip')
                .unbind('click')
                .bind('click', function () {
                    scope.openEditFeedbackPopup($(this).data('id'));
                });

                $('.desTitle')
                .unbind('click')
                .bind('click', function () {
                    scope.openEditMultipleLogs();
                });

                $('.updateSiteLicensee')
                .unbind('click')
                .bind('click', function () {
                    scope.updateSiteLicenseeType();
                });

                $('.checkActionItem')
                .unbind('click')
                .bind('click', function () {
                    scope.checkActionItem($(this).data('id'));
                    $(".checkActionAll").prop('checked', scope.getCheckAllStatus());
                });

                $('.checkActionAll')
                .unbind('click')
                .bind('click', function () {
                    scope.checkActionAll();
                    $(".checkActionItem").prop('checked', scope.getCheckAllStatus());
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

        function getSubCategoryName(mainCategoryId, subCategoryId, mainCategories){
            var mainCategory = null;
            for (var i = 0; i < mainCategories.length; i++) {
                if (mainCategories[i].Id == mainCategoryId) {
                    mainCategory = mainCategories[i];
                    break;
                }
            }

            if(mainCategory){
                for(var j = 0; j < mainCategory.ChildSettings.length; j++){
                    if(mainCategory.ChildSettings[j].Id == subCategoryId){
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
                if (settings[i].SettingType == "Category" && mainCategoryIds.indexOf(settings[i].SettingIdLv1) < 0)
                {
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

        function renderTitle(scope) {
            var updateButtons = !scope.createUpdateFeedbackPermission ? '' :
                '<div class="update-site-licensee"><a class="updateSiteLicensee" href="javascript:;">' + manageFeedbackResources.UpdateSiteLicenseeType + '</a></div>' +
                '<div class="split-bar"> | </div>' +
                '<div class="lnksubject multiple-edit"><a class="desTitle" href="javascript:;">' + manageFeedbackResources.MultipleEdit + '</a></div>' +
                '<div class="split-bar"> | </div>';

            return '<div class="feed-back">' + manageFeedbackResources.Feedback + '</div>' + updateButtons;
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
                <div>By <span class="b">' + feedback.CreatorName + '</span> on ' + feedback.CreateDateString + '</div>\
                <div>Solving Duration: ' + feedback.DurationTime + '</div>';

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
                    renderManageFeedback(newValues, scope, element);
                });
            }
        };
    });
})();