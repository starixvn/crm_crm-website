"use strict";
/* global newInformApplication,
crmConstants,
viewResources */
/* jshint camelcase: false,
unused: vars */
newInformApplication.directive('informHeadlineListContainer', ['$document', function ($document) {
    var MIN_PAGE_SIZE = 100;
    var MAX_PAGE_SIZE = 500;
    var selfScope;

    function generateDropdown(data, bindingName, valueProperty, textProperty, selectedValue) {
        var html = '<select data-binding-name="' + bindingName + '" style="width: 100px">';

        for (var i = 0; i < data.length; i++) {
            var item = data[i];
            var value = item[valueProperty];
            html += '<option value="' + item[valueProperty] + '" ' +
                    (value && selectedValue && value.toString() === selectedValue.toString() ? 'selected' : '') +
                     ' >' + item[textProperty] + '</option>';
        }
        html += '</select>';

        return html;
    }
    function htmlEncode(value) {
        return String(value)
            .replace(/&/g, '&amp;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
    }
    function htmlDecode(value) {
        return $('<div/>').html(value).text();
    }

    function renderStatusCell(columnModel) {
        return generateDropdown(selfScope.$parent.informStatuses,
            "InformHeadlineStatusId", "SettingId", "SettingValue", columnModel.Value.InformHeadlineStatusId);
    }

    function renderRemarkCell(columnModel) {
        return '<textarea data-binding-name="Remark">' + htmlDecode(columnModel.Value.Remark) + '</textarea>';
    }

    function renderExtensionCell(columnModel) {
        return generateDropdown(selfScope.$parent.stations, "Extension", "StationId", "Name", columnModel.Value.Extension);
    }

    function renderProductStatusCell(columnModel) {
        return generateDropdown(selfScope.$parent.productStatuses, "ProductStatus", "StatusId", "Name", columnModel.Value.ProductStatus);
    }

    function renderListData(scope, element, attrs) {
        selfScope = scope;

        if (scope.dataSource === null) {
            return;
        }

        var $table = element.find("#inform-headline-list");
        $table.empty();
        var bodyData = [];
        var rowsCount = scope.dataSource.length;
        if (rowsCount > 0) {
            for (var i = 0; i < rowsCount; i++) {
                var item = scope.dataSource[i];
                bodyData.push({
                    Columns: [
                            { Name: "DirectAccount", Value: item.UserName },
                            { Name: "Status", Value: item, CustomCellRender: renderStatusCell },
                            { Name: "UpdatedTime", Value: item.UpdatedTime },
                            { Name: "Remark", Value: item, CustomCellRender: renderRemarkCell },
                            { Name: "Handler", Value: '' },
                            { Name: "Extension", Value: item, CustomCellRender: renderExtensionCell },
                            { Name: "ProductStatus", Value: item, CustomCellRender: renderProductStatusCell },
                        ],
                    CssClass: item.GroupId > 0 ? 'headline' : 'direct',
                    CustomAttr: 'data-headline-id=' + item.HeadlineId + ' data-rowindex=' + (item.Id - 1)
                });
            }
        }

        var gridOptions = {
            columns: [
                    { HeaderText: viewResources.DirectAccount, Width: 120, Name: "DirectAccount", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: viewResources.Status, Width: 130, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.UpdatedTime, Width: 100, Name: "UpdatedTime", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: viewResources.Remark, Width: 190, Name: "Remark", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.Handler, Width: 100, Name: "Handler", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: viewResources.Extension, Width: 130, Name: "Extension", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.ProductStatus, Width: 130, Name: "ProductStatus", HeaderAlign: "center", CellAlign: "center" }
                ],
            bodyRows: bodyData,
            numberOfFrozenColumn: 0,
            fullWidth: true,
            showDistinctRow: false,
            showHighlightRow: true,
            showTree: true,
            treeParentPaddingLeft: 5,
            headerColumnHeight: 29,
            bodyRowHeight: 20,
            gridExpandHeight: 250,
            showPager: true,
            pagerOption: {
                itemsPerPage: MIN_PAGE_SIZE,
                maxItemsPerPage: MAX_PAGE_SIZE,
                currentPage: 0,
                totalItem: scope.dataSource.length,
                startPerPage: MIN_PAGE_SIZE,
                pageStep: MIN_PAGE_SIZE
            }
        };

        function updateDataSource($target, useHtmlEncode) {
            var $tr = $target.closest("tr");
            var headlineId = $tr.data("headline-id");
            var rowIndex = parseInt($tr.data("rowindex"));
            var bindingName = $target.data("binding-name");

            var dataItem = scope.dataSource[rowIndex];
            dataItem[bindingName] = useHtmlEncode ? htmlEncode($target.val()) : $target.val();
            scope.updatedItems[headlineId] = dataItem;
        }

        $table.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);

        $table.on("change", "select", function (e) {
            updateDataSource(angular.element(e.target));
        });

        $table.on("change", "textarea", function (e) {
            updateDataSource(angular.element(e.target), true);
        });
    }

    return {
        restrict: 'A',
        require: '^ngModel',
        scope: {
            dataSource: '=ngModel',
            updatedItems: '='
        },
        link: function (scope, element, attrs) {
            scope.updatedItems = scope.updatedItems || {};
            scope.$watch("dataSource", function (oldval, newval) {
                scope.updatedItems = {};
                renderListData(scope, element, attrs);
            });
        }
    };
} ]);