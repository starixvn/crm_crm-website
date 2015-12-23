/* global sharedScriptManager,
crmConstants,
viewResources */
/* jshint camelcase: false */
(function () {
    "use strict";
    angular.module('deleted-feedback-statistics-application')
    .directive('tableDeletedFeedbackStatisticsReportContainer', function () {
        var selfScope;
        var tableHeaders;

        function getGripOtions(report) {
            if (!tableHeaders) {
                tableHeaders = getTableHeader();
            }
            var bodyData = getDataRow(report);
            var footerData = [{
                Columns: [{
                    Name: "Date",
                    Value: sharedScriptManager.stringFormat("{0} ({1})",
                            [viewResources.Grand_Total, report.GrandTotal]),
                    MergeColNumber: 7
                }]
            }];

            var gridOptions = {
                columns: tableHeaders,
                showTitle: true,
                titleInfo: {
                    text: report ? report.Name : "",
                    showExcelButton: true,
                    exportExcelFunction: function () {
                        selfScope.exportReport();
                    }
                },
                bodyRows: bodyData,
                numberOfFrozenColumn: 0,
                fullWidth: true,
                fullHeight: true,
                showDistinctRow: false,
                showHighlightRow: false,
                headerColumnHeight: 29,
                bodyRowHeight: 29,
                showTree: true,
                treeParentPaddingLeft: 5,

                showFooter: true,
                footerRows: footerData
            };

            return gridOptions;
        }

        function getTableHeader() {
            return [
                { HeaderText: viewResources.Date, Width: 100, Name: "Date", HeaderAlign: "center", CellAlign: "left", ShowCollapseExpand: true },
                { HeaderText: viewResources.Handler, Width: 100, Name: "Handler", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.DeletedBy, Width: 100, Name: "DeletedBy", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Feedback, Width: 350, Name: "Feedback", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Category, Width: 250, Name: "Category", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Customer, Width: 100, Name: "Customer", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.RelatedCustomer, Width: 100, Name: "RelatedCustomer", HeaderAlign: "center", CellAlign: "left" }
            ];
        }

        function getDataRow(report) {
            var bodyData = [];
            var deletedFeedbackStatisticsList = report.DeletedFeedbackByHandlers;
            var totalItems = deletedFeedbackStatisticsList.length;

            var i, j;
            for (i = 0; i < totalItems; i++) {
                var groupedHandler = deletedFeedbackStatisticsList[i];
                var currentRow = {
                    Columns: [{
                        Name: "Date",
                        Value: sharedScriptManager.stringFormat("{0} ({1})",
                            [groupedHandler.Handler, groupedHandler.GrandTotal]),
                        MergeColNumber: 7
                    }],
                    SubRows: []
                };

                var deletedFeedbackList = groupedHandler.DeletedFeedbacks;
                var totalDeletedFeedbacks = deletedFeedbackList.length;

                for (j = 0; j < totalDeletedFeedbacks; j++) {
                    var deletedFeedback = deletedFeedbackList[j];
                    var subRow = {
                        Columns: [
                            {
                                Name: "Date",
                                Value: deletedFeedback.CreateDateString
                            },
                            {
                                Name: "Handler",
                                Value: deletedFeedback.Handler
                            },
                            {
                                Name: "DeletedBy",
                                Value: deletedFeedback.Deletor
                            },
                            {
                                Name: "Feedback",
                                Value: getFeedbackCellContent(deletedFeedback.FeedbackId, deletedFeedback.Subject)
                            },
                            {
                                Name: "Category",
                                Value: getCategoryCellContent(deletedFeedback.MainCategory, deletedFeedback.SubCategory)
                            },
                            {
                                Name: "Customer",
                                Value: deletedFeedback.Customer
                            },
                            {
                                Name: "RelatedCustomer",
                                Value: deletedFeedback.RelatedCustomer
                            }
                        ],
                        CssClass: 'lnkViewFeedbackDetail-row'
                    };

                    currentRow.SubRows.push(subRow);
                }

                bodyData.push(currentRow);
            }

            return bodyData;
        }

        function getFeedbackCellContent(feedbackId, subject) {
            return '<a href="javascript:;" class="lnkFeedback" data-id="' + feedbackId + '">' +
                    '<span class="b">' + subject + '</span></a>';
        }

        function getCategoryCellContent(mainCategory, subCategory) {
            return '<div class="main-category">' + mainCategory + '</div>' +
                    '<span class="sub-category">' + subCategory + '</span>';
        }

        function renderReport(scope, element) {
            if (scope.reportOptions.DataList === null) {
                return;
            }

            selfScope = scope;
            var tableReport = element.find("#deleted-feedback-statistics-report");
            var reportData = scope.reportOptions.DataList;
            var gridOptions = getGripOtions(reportData);
            tableReport.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
            bindEvents(scope, element);
        }

        function bindEvents(scope, element) {
            element.find(".lnkFeedback").click(function () {
                var feedbackId = $(this).attr('data-id');
                scope.openDetailFeedbackPopup(feedbackId);
            });

            var tableReport = element.find("#deleted-feedback-statistics-report");
            var viewLogRows = tableReport.find(".lnkViewFeedbackDetail-row");
            viewLogRows.dblclick(function () {
                var feedbackId = $(this).find('.lnkFeedback').attr('data-id');
                scope.openDetailFeedbackPopup(feedbackId);
            });
        }

        return {
            restrict: 'C',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (oldval, newval) {
                    /* jshint unused: false */
                    if (!scope.reportOptions.IsUpdateGrid) {
                        return;
                    }

                    renderReport(scope, element);

                    scope.reportOptions.IsUpdateGrid = false;
                });
            }
        };
    });
})();