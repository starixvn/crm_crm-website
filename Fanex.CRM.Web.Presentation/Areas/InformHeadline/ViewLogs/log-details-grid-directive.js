(function (angular) {
    'use strict';

    angular.module('informHeadlineApplication').directive('logDetailsGrid', logDetailsGrid);
    logDetailsGrid.$inject = [];

    function logDetailsGrid() {
        function generateBodyRows(scope) {
            var rows = [];
            var totalItems = scope.dataSources.length;

            for (var i = 0; i < totalItems; i++) {
                var item = scope.dataSources[i];
                rows.push({
                    Columns: [
                        { Name: 'DirectAccount', Value: item.DirectAccount },
                        { Name: 'Status', Value: item.Status },
                        { Name: 'UpdatedTime', Value: item.ClientUpdatedTime },
                        { Name: 'Remark', Value: item.Remark },
                        { Name: 'Handler', Value: item.Handler },
                        { Name: 'Extension', Value: item.Extension },
                        { Name: 'ProductStatus', Value: item.ProductStatus }
                    ]
                });
            }

            return rows;
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
                bodyRows: generateBodyRows(scope, 0),
                bodyRowHeight: 20,
                gridExpandHeight: 250,
                autoResizeColumn: true,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: scope.pagerOption.itemsPerPage,
                    itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                    currentPage: scope.pagerOption.pageIndex,
                    totalItem: scope.dataSources.length,
                    pagerSelectCallBack: function (pageIndex, pageSize) {
                        scope.onPageChange(pageIndex, pageSize);
                    }
                }
            };

            $(element).removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
        }

        return {
            restrict: 'A',
            require: '^ngModel',
            scope: {
                dataSources: '=ngModel',
                pagerOption: '=',
                onPageChange: '='
            },
            link: function (scope, element) {
                scope.$watch('dataSources', function (oldval, newval) {
                    renderGrid(scope, element);
                });
            }
        };
    }
})(angular);