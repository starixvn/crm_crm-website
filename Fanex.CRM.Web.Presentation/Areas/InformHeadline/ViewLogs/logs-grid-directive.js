(function (angular) {
    'use strict';

    angular.module('informHeadlineApplication').directive('logsGrid', logsGrid);
    logsGrid.$inject = [];

    function logsGrid() {
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

                for (var i = firstRowIndex; i < lastRowIndex; i++) {
                    var item = scope.dataSources[i];
                    bodyData.push({
                        Columns: [
                            { Name: 'Date', Value: item.ClientDate },
                            { Name: 'EditedBy', Value: item.EditedBy },
                            { Name: 'Totals', Value: '<a class="log-details" href="javascript:;">' + item.Totals + '</a>' }
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
                        HeaderText: viewResources.Date, Width: 100, MaxResizeWidth: 200, Name: 'Date',
                        HeaderAlign: 'center', CellAlign: 'center', CanResize: true
                    },
                    {
                        HeaderText: viewResources.EditedBy, Width: 100, Name: 'EditedBy',
                        HeaderAlign: 'center', CellAlign: 'center'
                    },
                    {
                        HeaderText: viewResources.NumberOfRemovedEditedAccounts, Width: 150, Name: 'Totals',
                        HeaderAlign: 'center', CellAlign: 'center'
                    }
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

            $('a.log-details').off('click').on('click', function () {
                scope.onViewDetails();
            });
        }

        return {
            restrict: 'A',
            require: '^ngModel',
            scope: {
                dataSources: '=ngModel',
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