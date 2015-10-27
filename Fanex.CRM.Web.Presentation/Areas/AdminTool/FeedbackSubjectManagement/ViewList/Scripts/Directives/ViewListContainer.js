(function () {
    "use strict";
    /* global crmConstants,
    sharedScriptManager,
    viewResources */
    /* jshint camelcase: false,
    unused: vars */
    angular.module('addmintool-feedbacksubjectmanagement-application').directive('feedbackTemplateListContainer', function () {
        var selfScope;
        var feedbackSubjectTemplateView;

        function renderFeedbackSubjectTemplates(scope, element, attrs) {
            selfScope = scope;
            feedbackSubjectTemplateView = scope.feedbackSubjectTemplateView;
            if (scope.feedbackSubjectTemplateView === null) {
                return;
            }

            var tableTemplates = element.find("#feedback-template-list");
            var templates = scope.feedbackSubjectTemplateView.Templates;
            var bodyData = [];

            tableTemplates.empty();
            if (templates && templates.length > 0) {
                var templatesLength = templates.length;
                for (var i = 0; i < templatesLength; i++) {
                    var info = templates[i];
                    bodyData.push({
                        Columns: [
                        { Name: "ColTemplateId", Value: info.Id },
                        { Name: "ColTemplateContent", Value: info.TemplateContent },
                        {
                            Name: "ColEdit",
                            Value: '<a class="c hand editSetting" href="JavaScript:void(0);" data-row-index="' + i + '"></a>'
                        },
                        {
                            Name: "ColDelete",
                            Value: '<a class="c hand deleteSetting" href="JavaScript:void(0);" data-row-index="' + i + '"></a>'
                        }],
                        CssClass: 'editlog-link-row row-index-' + i
                    });
                }
            }

            var gridOptions = {
                columns: [{ HeaderText: viewResources.Template_Id, Width: 80, Name: "ColTemplateId", HeaderAlign: "center", CellAlign: "center" },
                {
                    HeaderText: viewResources.Feedback_Subject_Template_Content,
                    Width: 470,
                    Name: "ColTemplateContent",
                    HeaderAlign: "center",
                    CellAlign: "left"
                },
                { HeaderText: viewResources.Edit, Width: 60, Name: "ColEdit", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Delete, Width: 60, Name: "ColDelete", HeaderAlign: "center", CellAlign: "center"}],
                showTitle: false,
                bodyRows: bodyData,
                numberOfFrozenColumn: 2,
                fullWidth: true,
                fullHeight: true,

                gridHeight: 0,
                showDistinctRow: false,
                showHighlightRow: true,
                headerColumnHeight: 29,
                bodyRowHeight: 25,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: scope.lastSelectedParams.PageSize,
                    currentPage: scope.lastSelectedParams.PageIndex,
                    itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                    totalItem: scope.feedbackSubjectTemplateView.TotalTemplates,
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };

            tableTemplates.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
            var editIcons = tableTemplates.find(".editSetting");
            editIcons.unbind("click");
            editIcons.click(function () {
                var rowIndex = parseInt($(this).attr("data-row-index"));
                scope.editTemplate(feedbackSubjectTemplateView.Templates[rowIndex]);
            });

            var deleteIcons = tableTemplates.find(".deleteSetting");
            deleteIcons.unbind("click");
            deleteIcons.click(function () {
                var rowIndex = parseInt($(this).attr("data-row-index"));
                scope.deleteTemplate(feedbackSubjectTemplateView.Templates[rowIndex]);
            });
        }

        function ReRenderAfterPaged(page_index, itemsPerPage, jq) {
            selfScope.pagingFeedbackSubjectTemplateView(page_index, itemsPerPage);
            return false;
        }

        return {
            restrict: 'C',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (oldval, newval) {
                    renderFeedbackSubjectTemplates(scope, element, attrs);
                });
            }
        };
    });
})();