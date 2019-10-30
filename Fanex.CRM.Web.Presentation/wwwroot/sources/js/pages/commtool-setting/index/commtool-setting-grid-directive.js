(function (angular) {
    'use strict';

    angular.module('commToolSettingModule').directive('commToolSettingGrid', commToolSettingGrid);
    commToolSettingGrid.$inject = [];

    function commToolSettingGrid() {
        function generateColumns() {
            let columns = [
                { HeaderText: communicationSettingResources.NumberColumn, Width: 50, Name: 'RowNum', HeaderAlign: 'center', CellAlign: 'center' },
                {
                    HeaderText: communicationSettingResources.Customer, Width: 100, MaxResizeWidth: 200, Name: 'Customer',
                    HeaderAlign: 'center', CellAlign: 'left', CanResize: true
                },
                { HeaderText: communicationSettingResources.Position, Width: 100, Name: 'Position', HeaderAlign: 'center', CellAlign: 'center' },
                {
                    HeaderText: communicationSettingResources.Category, Width: 100, MaxResizeWidth: 300, Name: 'Category',
                    HeaderAlign: 'center', CellAlign: 'left', CanResize: true
                },
                {
                    HeaderText: communicationSettingResources.SubCategory, Width: 100, MaxResizeWidth: 300, Name: 'SubCategory',
                    HeaderAlign: 'center', CellAlign: 'left', CanResize: true
                },
                {
                    HeaderText: communicationSettingResources.CommunicationTool, Width: 100, MaxResizeWidth: 200, Name: 'CommunicationTool',
                    HeaderAlign: 'center', CellAlign: 'left', CanResize: true
                }
            ];

            if (isHostOnCommunicationSetting) {
                columns.push({ HeaderText: communicationSettingResources.Edit, Width: 50, Name: 'Edit', HeaderAlign: 'center', CellAlign: 'center', MergeHeaderColNumber: 2 });
                columns.push({ HeaderText: communicationSettingResources.Edit, Width: 50, Name: 'Delete', HeaderAlign: 'center', CellAlign: 'center' });
            }

            return columns;
        }

        function generateBodyRows(scope) {
            let rows = [];
            const totalItems = scope.dataSources.length;

            for (let i = 0; i < totalItems; i++) {
                const item = scope.dataSources[i];
                let columns = [
                    { Name: 'RowNum', Value: scope.pageIndex * scope.pageSize + i + 1 },
                    { Name: 'Customer', Value: item.Username },
                    { Name: 'Position', Value: item.Role },
                    { Name: 'Category', Value: item.Category },
                    { Name: 'SubCategory', Value: item.SubCategory },
                    { Name: 'CommunicationTool', Value: item.CommTool }
                ];
                if (isHostOnCommunicationSetting) {
                    columns.push({ Name: 'Edit', Value: '<a class="edit-setting" href="javascript:;" data-default-settingid="' + item.DefaultCommId + '"><i class="fa fa-pencil"></i></a>' });
                    columns.push({ Name: 'Delete', Value: '<a class="delete-setting" href="javascript:;" data-default-settingid="' + item.DefaultCommId + '"><i class="fa fa-times"></i></a>' });
                }

                rows.push({ Columns: columns });
            }

            return rows;
        }

        function renderGrid(scope, element) {
            if (scope.dataSources === null) {
                return;
            }

            $(element).empty();

            const gridOptions = {
                columns: generateColumns(),
                showTitle: true,
                titleInfo: {
                    text: communicationSettingResources.DefaultCommunicationSetting
                },
                bodyRows: generateBodyRows(scope),
                gridExpandHeight: 200,
                autoResizeColumn: true,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: scope.pageSize,
                    itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                    currentPage: scope.pageIndex,
                    totalItem: scope.totalItems,
                    pagerSelectCallBack: function (pageIndex, pageSize) {
                        scope.onPageChange(pageIndex, pageSize);
                    }
                }
            };

            $(element).removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);

            $('a.edit-setting', $(element)).off('click').on('click', function () {
                const settingid = parseInt($(this).data('default-settingid'), 10);
                scope.onEdit(settingid);
            });

            $('a.delete-setting', $(element)).off('click').on('click', function () {
                const settingid = parseInt($(this).data('default-settingid'), 10);
                scope.onDelete(settingid);
            });
        }

        return {
            restrict: 'A',
            require: 'ngModel',
            scope: {
                dataSources: '=ngModel',
                totalItems: '=',
                pageIndex: '=',
                pageSize: '=',
                onPageChange: '=',
                onEdit: '=',
                onDelete: '='
            },
            link: function (scope, element) {
                scope.$watch('dataSources', function (oldval, newval) {
                    renderGrid(scope, element);
                });
            }
        };
    }
})(angular);