(function () {
    "use strict";
    /* global latestFeedbackResources,
    sharedScriptManager */
    angular.module('feedback-latest-application').directive('tableLatestFeedbackContainer', function () {
        var gridOptions, tableLatestFeedback;
        function getGridOptions() {
            return {
                columns: [
                    { HeaderText: latestFeedbackResources.Feedback, Width: 450, Name: 'ColFeedback', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: latestFeedbackResources.Category, Width: 270, Name: 'ColCategory', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: latestFeedbackResources.Status, Width: 50, Name: 'ColStatus', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: latestFeedbackResources.RelatedCustomer, Width: 160, Name: 'ColRelatedCustomer', HeaderAlign: 'center', CellAlign: 'left'}],
                showTitle: true,
                titleInfo: {
                    showExcelButton: true
                },
                showFooter: false,
                fullWidth: true,
                fullHeight: true,
                gridExpandHeight: 90,
                showDistinctRow: false,
                showHighlightRow: false,
                headerColumnHeight: 29
            };
        }

        function getGridBody(feedbacks, mainCategories) {
            var fbLength = feedbacks.length;
            if (fbLength === 0) { return []; }

            var bodyData = [];
            var fb, currentRow, status, cssClass;
            for (var i = 0; i < fbLength; i++) {
                fb = feedbacks[i];

                currentRow = [
                { Name: 'ColFeedback', Value: renderFeedbackColumn(fb, i) },
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

        function renderLatestFeedback(report, scope, element) {
            if (!report) { return; }

            var feedbacks = report.Feedbacks;
            var bodyData = getGridBody(feedbacks, report.Settings.MainCategory);

            gridOptions = gridOptions || getGridOptions();
            gridOptions.bodyRows = bodyData;
            gridOptions.titleInfo.text = report.Title;
            gridOptions.titleInfo.exportExcelFunction = function () {
                scope.exportFeedbackExcel();
            };

            tableLatestFeedback = element.find('#table-latest-feedback');
            tableLatestFeedback
            .empty()
            .removeClass('hide')
            .fanexGrid(gridOptions);

            if (bodyData.length > 0) {
                var tooltips = $('.desTooltip');
                tooltips.unbind('click')
                .bind('click', function () {
                    var $this = $(this);
                    $(".tipsy").remove();
                    scope.openEditFeedbackPopup($this.data('id'));
                })
                .tipsy({ fade: true, gravity: 'nw', html: true, title: function () { return feedbacks[$(this).data('index')].DescriptionTooltip; } });
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

        function renderFeedbackColumn(feedback, number) {
            var html = '<div style="margin:2px 0 0 2px;"> \
            <div class="lnksubject">\
                <a class="desTooltip" href="javascript:;" data-id="' + feedback.FeedbackId + '" data-index="' + number + '">\
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
                    renderLatestFeedback(newValues, scope, element);
                });
            }
        };
    });
})();