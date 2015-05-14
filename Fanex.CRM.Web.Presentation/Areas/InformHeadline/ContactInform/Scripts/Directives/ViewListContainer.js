"use strict";
/* global contactInformApplication,
crmConstants,
viewResources */
/* jshint camelcase: false,
unused: vars */
contactInformApplication.directive('informHeadlineListContainer', ['$document', function ($document) {
    var KEPT_FOR_REFERENCE_STATUS_ID = 45007;
    var MIN_PAGE_SIZE = 100;
    var MAX_PAGE_SIZE = 500;
    var selfScope;

    function generateDropdown(data, bindingName, valueProperty, textProperty, selectedValue, disabled) {
        var html = '<select ' + (disabled ? 'disabled' : '') + ' data-binding-name="' + bindingName + '" style="width: 100px">';

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

    function renderDropdownCell(columnModel, data, bindingName, valueProperty, textProperty, selectedValue) {
        if (columnModel.Value.InformHeadlineStatusId === KEPT_FOR_REFERENCE_STATUS_ID) {
            return '<span class="disabled-box">' + viewResources.KeptForReference + '</span>';
        }

        return generateDropdown(data, bindingName, valueProperty, textProperty, selectedValue, selfScope.$parent.hasHostPermission === false);
    }

    function renderStatusDropdownCell(columnModel) {
        return renderDropdownCell(columnModel, selfScope.$parent.informStatuses,
            "InformHeadlineStatusId", "SettingId", "SettingValue", columnModel.Value.InformHeadlineStatusId);
    }

    function renderExtensionDropdownCell(columnModel) {
        return renderDropdownCell(columnModel, selfScope.$parent.stations,
            "Extension", "StationId", "Name", columnModel.Value.Extension);
    }

    function renderProductStatusDropdownCell(columnModel) {
        return renderDropdownCell(columnModel, selfScope.$parent.productStatuses,
            "ProductStatus", "StatusId", "Name", columnModel.Value.ProductStatus);
    }

    function renderRemarkCell(columnModel) {
        return '<textarea ' + (selfScope.$parent.hasHostPermission === true ? '' : 'disabled') + 
            ' data-binding-name="Remark">' + htmlDecode(columnModel.Value.Remark) + '</textarea>';
    }

    function renderListData(scope, element, attrs) {
        selfScope = scope;

        if (scope.dataSource === null) {
            return;
        }

        var $tableSetting = element.find("#inform-headline-list");
        $tableSetting.empty();
        var bodyData = [];
        var rowsCount = scope.dataSource.length;
        if (rowsCount > 0) {
            for (var i = 0; i < rowsCount; i++) {
                var item = scope.dataSource[i];

                bodyData.push({
                    Columns: [
                            { Name: "DirectAccount", Value: item.UserName },
                            { Name: "Status", Value: item, CustomCellRender: renderStatusDropdownCell },
                            { Name: "UpdatedTime", Value: item.ClientUpdatedTime },
                            { Name: "Remark", Value: item, CustomCellRender: renderRemarkCell },
                            { Name: "Handler", Value: item.Handler },
                            { Name: "Extension", Value: item, CustomCellRender: renderExtensionDropdownCell },
                            { Name: "ProductStatus", Value: item, CustomCellRender: renderProductStatusDropdownCell },
                        ],
                    CssClass: item.GroupId > 0 ? 'headline' : 'direct',
                    CustomAttr: 'data-headline-id=' + item.HeadlineId + ' data-rowindex=' + i
                });
            }
        }

        var gridOptions = {
            columns: [
                    { HeaderText: viewResources.DirectAccount, Width: 120, Name: "DirectAccount", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: viewResources.Status, Width: 130, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.UpdatedTime, Width: 100, Name: "UpdatedTime", HeaderAlign: "center", CellAlign: "left", 
                        FormatDateTime: "MMM DD, YYYY" },
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
            gridExpandHeight: 200,
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

        $tableSetting.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);

        $tableSetting.on("change", "select", function (e) {
            updateDataSource(angular.element(e.target));
        });

        $tableSetting.on("change", "textarea", function (e) {
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