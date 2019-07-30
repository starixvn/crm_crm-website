(function (angular) {
    'use strict';

    angular.module('informHeadlineApplication').directive('logDetailsGrid', logDetailsGrid);
    logDetailsGrid.$inject = [];

    function logDetailsGrid() {
        var MIN_PAGE_SIZE = 100;
        var MAX_PAGE_SIZE = 300;

        function generateBodyRows(scope, pageIndex, pageSize) {
            var bodyData = [];
            var rowsCount = scope.dataSources.length;
            var firstRowIndex = pageIndex * pageSize;
            if (rowsCount >= firstRowIndex) {
                var lastRowIndex = firstRowIndex + pageSize;
                if (lastRowIndex > rowsCount) {
                    lastRowIndex = rowsCount;
                }

                var remark = 'remark remark remark remark remark \n remark remark remark remark remark \n remark remark remark remark remark';

                for (var i = firstRowIndex; i < lastRowIndex; i++) {
                    var item = scope.dataSources[i];
                    bodyData.push({
                        Columns: [
                            { Name: 'DirectAccount', Value: item.EditedBy },
                            { Name: 'Status', Value: 'Open' },
                            { Name: 'UpdatedTime', Value: item.Date },
                            { Name: 'Remark', Value: remark },
                            { Name: 'Handler', Value: item.EditedBy },
                            { Name: 'Extension', Value: '111' },
                            { Name: 'ProductStatus', Value: 'Enabled' }
                        ],
                        CustomAttr: 'data-rowindex=' + i
                    });
                }
            }

            return bodyData;
        }

        function renderGrid(scope, element) {
            if (scope.dataSources === null) {
                return;
            }

            $(element).empty();

            var gridOptions = {
                columns: [
                    {
                        HeaderText: viewResources.DirectAccount, Width: 100, MaxResizeWidth: 200, Name: 'DirectAccount',
                        HeaderAlign: 'center', CellAlign: 'center', CanResize: true
                    },
                    {
                        HeaderText: viewResources.Status, Width: 80, MaxResizeWidth: 120, Name: 'Status',
                        HeaderAlign: 'center', CellAlign: 'center', CanResize: true
                    },
                    {
                        HeaderText: viewResources.UpdatedTime, Width: 100, Name: 'UpdatedTime',
                        HeaderAlign: 'center', CellAlign: 'center', FormatDateTime: 'MMM DD, YYYY hh:mm:ss A'
                    },
                    {
                        HeaderText: viewResources.Remark, Width: 200, MaxResizeWidth: 300, Name: 'Remark',
                        HeaderAlign: 'center', CellAlign: 'left', CanResize: true
                    },
                    { HeaderText: viewResources.Handler, Width: 100, Name: 'Handler', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: viewResources.Extension, Width: 80, Name: 'Extension', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: viewResources.ProductStatus, Width: 100, Name: 'ProductStatus', HeaderAlign: 'center', CellAlign: 'center' }
                ],
                bodyRows: generateBodyRows(scope, 0, MIN_PAGE_SIZE),
                bodyRowHeight: 20,
                gridExpandHeight: 250,
                autoResizeColumn: true,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: MIN_PAGE_SIZE,
                    maxItemsPerPage: MAX_PAGE_SIZE,
                    currentPage: 0,
                    totalItem: scope.dataSources.length,
                    startPerPage: MIN_PAGE_SIZE,
                    pageStep: MIN_PAGE_SIZE,
                    pagerSelectCallBack: function (pageIndex, pageSize) {
                        gridOptions.pagerOption.itemsPerPage = pageSize;
                        gridOptions.pagerOption.pageSize = pageSize;
                        gridOptions.pagerOption.currentPage = pageIndex;
                        gridOptions.bodyRows = generateBodyRows(scope, pageIndex, pageSize);
                        $(element).fanexGrid(gridOptions);
                    }
                }
            };

            $(element).removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
        }

        return {
            restrict: 'A',
            require: '^ngModel',
            scope: {
                dataSources: '=ngModel'
            },
            link: function (scope, element) {
                scope.$watch('dataSources', function (oldval, newval) {
                    renderGrid(scope, element);
                });
            }
        };
    }
})(angular);