(function () {
    "use strict";
    /* global crmConstants,
    viewResources,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('picofheadline-application').directive('picAssignmentDataGridDirective', function () {
        var cachedData = {
            scope: null,
            reportTable: null,
            gridData: null,
            gridOptions: null
        };

        function getGridHeight() {
            return crmConstants.MarginBottomPage + (cachedData.scope.isShowFilter ? 230 : 50);
        }

        function getGripOtions(gridData) {
            var bodyData = [];
            var footerData = [];
            var reportLength = gridData ? gridData.Items.length : 0;
            var mergeHeadline = 0;
            var mergePIC = 0;
            if (reportLength > 0) {
                for (var i = 0; i < reportLength; i++) {
                    var assignment = gridData.Items[i];
                    var columns = [{ Name: "ColNo", Value: assignment.RowNum }];

                    if (mergeHeadline === 0) {
                        mergeHeadline = gridData.Items.filter(item => item.HeadlineID === assignment.HeadlineID).length;
                        mergePIC = gridData.Items.filter(item => item.HeadlineID === assignment.HeadlineID && item.PIC === assignment.PIC).length;
                        columns.push(
                            { Name: "ColGroupHeadline", Value: assignment.HeadlineName, MergeRowNumber: mergeHeadline },
                            { Name: "ColHeadlineType", Value: assignment.HeadlineType, MergeRowNumber: mergeHeadline },
                            { Name: "ColPIC", Value: getPICCell(assignment.PIC), MergeRowNumber: mergePIC }
                        );

                    }

                    if (mergePIC === 0 && mergeHeadline !== 0) {
                        mergePIC = gridData.Items.filter(item => item.HeadlineID === assignment.HeadlineID && item.PIC === assignment.PIC).length;
                        columns.push({ Name: "ColPIC", Value: getPICCell(assignment.PIC), MergeRowNumber: mergePIC });
                    }

                    columns.push(
                        { Name: "ColDirectAccount", Value: assignment.DirectAccount },
                        { Name: "ColStatus", Value: assignment.Status },
                        { Name: "ColSite", Value: assignment.Site },
                        { Name: "ColCurrency", Value: assignment.Currency },
                        { Name: "ColPosition", Value: assignment.Position },
                        { Name: "ColAction", Value: `<input class='item-${assignment.}' type='checkbox'/>` }
                    );

                    bodyData.push({
                        Columns: columns
                    });
                    mergeHeadline--;
                    mergePIC--;
                }
            }

            var headerColumns = [
                { HeaderText: viewResources.No, Width: 70, Name: "ColNo", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.GroupHeadline, Width: 150, Name: "ColGroupHeadline", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.HeadlineType, Width: 100, Name: "ColHeadlineType", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.PIC, Width: 150, Name: "ColPIC", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.DirectAccount, Width: 152, Name: "ColDirectAccount", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Status, Width: 100, Name: "ColStatus", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Site, Width: 150, Name: "ColSite", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Currency, Width: 100, Name: "ColCurrency", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Position, Width: 100, Name: "ColPosition", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: `<input type='button' value='Remove'/><input type='checkbox'/>`, Width: 70, Name: "ColAction", HeaderAlign: "center", CellAlign: "center" }
            ];

            var gridOptions = {
                columns: headerColumns,
                showTitle: true,
                titleInfo: {
                    text: viewResources.CustomerReport_Title,
                    showExcelButton: true,
                    exportExcelFunction: function () {
                        cachedData.scope.exportReport();
                    }
                },
                bodyRows: bodyData,
                footerRows: footerData,
                numberOfFrozenColumn: 0,
                fullWidth: true,
                fullHeight: true,
                gridExpandHeight: getGridHeight(),
                showDistinctRow: false,
                showHighlightRow: true,
                headerColumnHeight: 29,
                bodyRowHeight: 29,
                showPager: true,
                showTree: false,
                customPager: true,
                allowTruncate: true,
                pagerOption: {
                    itemsPerPage: cachedData.scope.searchCriteria.PageSize,
                    currentPage: cachedData.scope.searchCriteria.PageIndex - 1,
                    itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                    totalItem: gridData.Total,
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };

            return gridOptions;
        }

        function ReRenderAfterPaged(page_index, itemsPerPage) {
            cachedData.scope.pagingCustomerReport(page_index + 1, itemsPerPage);
            return false;
        }

        function renderPicOfHeadlineGrid(scope, element) {
            if (scope.picOfHeadlineOptions.DataList === null) {
                return;
            }

            cachedData.scope = scope;
            var tablePicOfHeadline = element.find("#table-picOfHeadline");
            cachedData.reportTable = tablePicOfHeadline;
            var gridData = scope.picOfHeadlineOptions.DataList;
            cachedData.gridData = gridData;
            var gridOptions = getGripOtions(gridData);
            cachedData.gridOptions = gridOptions;
            tablePicOfHeadline.empty().removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
        }

        function getPICCell(picValue) {
            if (picValue == null) {
                return `<span>+</span>`;
            }
            else {
                return picValue;
            }
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch('picOfHeadlineOptions', function (oldval, newval) {
                    if (!scope.picOfHeadlineOptions.IsUpdateGrid) {
                        return;
                    }

                    if (scope.picOfHeadlineOptions.IsResize && cachedData.reportTable) {
                        cachedData.reportTable.fanexGrid("resizeHeight", getGridHeight());
                        scope.picOfHeadlineOptions.IsResize = false;
                    } else {
                        renderPicOfHeadlineGrid(scope, element);
                    }

                    scope.picOfHeadlineOptions.IsUpdateGrid = false;
                });
            }
        };
    });
})();