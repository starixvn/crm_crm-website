(function (angular) {
    'use strict';

    angular.module('informHeadlineModule').directive('logsGrid', logsGrid);
    logsGrid.$inject = [];

    function logsGrid() {
        function generateBodyRows(scope) {
            let rows = [];
            const totalItems = scope.dataSources.length;

            for (let i = 0; i < totalItems; i++) {
                const item = scope.dataSources[i];
                rows.push({
                    Columns: [
                        { Name: 'Date', Value: item.ClientLogDate },
                        { Name: 'EditedBy', Value: item.Username },
                        { Name: 'Totals', Value: '<a class="log-details" href="javascript:;">' + item.Totals + '</a>' }
                    ],
                    CustomAttr: 'data-log-id=' + item.LogId
                });
            }

            return rows;
        }

        function renderGrid(scope, element) {
            if (scope.dataSources === null) {
                return;
            }

            $(element).empty();

            const gridOptions = {
                columns: [
                    {
                        HeaderText: viewResources.Date, Width: 100, MaxResizeWidth: 200, Name: 'Date',
                        HeaderAlign: 'center', CellAlign: 'center', CanResize: true
                    },
                    {
                        HeaderText: viewResources.EditedBy, Width: 100, MaxResizeWidth: 200, Name: 'EditedBy',
                        HeaderAlign: 'center', CellAlign: 'center', CanResize: true
                    },
                    {
                        HeaderText: viewResources.NumberOfRemovedEditedAccounts, Width: 150, Name: 'Totals',
                        HeaderAlign: 'center', CellAlign: 'center'
                    }
                ],
                bodyRows: generateBodyRows(scope),
                bodyRowHeight: 20,
                gridExpandHeight: 250,
                autoResizeColumn: true,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: scope.pagerOption.itemsPerPage,
                    itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                    currentPage: scope.pagerOption.pageIndex,
                    totalItem: scope.totalItems,
                    pagerSelectCallBack: function (pageIndex, pageSize) {
                        scope.onPageChange(pageIndex, pageSize);
                    }
                }
            };

            $(element).removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);

            $('a.log-details').off('click').on('click', function () {
                const logid = $(this).closest('tr').data('log-id');
                scope.onViewDetails(logid);
            });
        }

        return {
            restrict: 'A',
            require: '^ngModel',
            scope: {
                dataSources: '=ngModel',
                totalItems: '=',
                pagerOption: '=',
                onPageChange: '=',
                onViewDetails: '='
            },
            link: function (scope, element) {
                scope.$watch('dataSources', function (oldval, newval) {
                    renderGrid(scope, element);
                });
            }
        };
    }
})(angular);