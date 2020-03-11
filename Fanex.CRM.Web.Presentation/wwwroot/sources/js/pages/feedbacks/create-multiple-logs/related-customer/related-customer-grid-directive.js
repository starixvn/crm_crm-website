(function (angular) {
    'use strict';

    angular.module('createMultipleLogsModule').directive('relatedCustomerGrid', relatedCustomerGrid);
    relatedCustomerGrid.$inject = [];

    function relatedCustomerGrid() {
        function generateColumns() {
            let columns = [
                { HeaderText: 'Group Headline', Width: 200, Name: 'Headline', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: 'Username', Width: 800, Name: 'Username', HeaderAlign: 'center', CellAlign: 'left' }
            ];

            return columns;
        }

        function generateBodyRows(scope) {
            let rows = [];
            const totalItems = scope.dataSources.length;

            for (let i = 0; i < totalItems; i++) {
                const item = scope.dataSources[i];
                let columns = [
                    { Name: 'Headline', Value: item.Headline },
                    { Name: 'Username', Value: item.RelatedCustomers.map(x => x.Username).join(', ') }
                ];

                rows.push({ Columns: columns });
            }

            return rows;
        }

        function renderGrid(scope, element) {
            if (scope.dataSources === null) {
                return;
            }

            $(element).empty();

            const totalItems = scope.dataSources.length;
            const gridOptions = {
                columns: generateColumns(),
                showTitle: true,
                titleInfo: {
                    text: 'List of customers under headlines'
                },
                bodyRows: generateBodyRows(scope),
                showPager: true,
                pagerOption: {
                    itemsPerPage: 10,
                    itemsPerPageArray: [10, 20, 50],
                    currentPage: 0,
                    totalItem: totalItems
                }
            };

            $(element).removeClass('display-none').fanexGrid(gridOptions);
        }

        return {
            restrict: 'A',
            require: 'ngModel',
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