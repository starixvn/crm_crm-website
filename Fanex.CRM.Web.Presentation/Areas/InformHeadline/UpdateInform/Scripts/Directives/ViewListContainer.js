"use strict";
/* global updateInformApplication,
crmConstants,
viewResources */
/* jshint camelcase: false,
unused: vars */
updateInformApplication.directive('informHeadlineListContainer', ['$document', function ($document) {
    var selfScope;

    function generateDropdown(data, bindingName, valueProperty, textProperty, selectedValue) {
        var html = '<select data-binding-name="' + bindingName + '" style="width: 100px">';

        for (var i = 0; i < data.length; i++) {
            var item = data[i];

            html += '<option value="' + item[valueProperty] + '" ' +
                    (item[valueProperty] == selectedValue ? 'selected' : '') +
                     ' >' + item[textProperty] + '</option>';
        }
        html += '</select>';

        return html;
    }
    function htmlEncode(value) {
        return $('<div/>').text(value).html();
    }
    function htmlDecode(value) {
        return $('<div/>').html(value).text();
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
                            { Name: "Status", Value: item, CustomCellRender: function (columnModel) {
                                return generateDropdown(scope.$parent.informStatuses, "InformHeadlineStatusId", "SettingId", "SettingValue", columnModel.Value.InformHeadlineStatusId);
                            }
                            },
                            { Name: "UpdatedTime", Value: item.UpdatedTime },
                            { Name: "Remark", Value: item, CustomCellRender: function (columnModel) {
                                return '<textarea data-binding-name="Remark">' + htmlDecode(columnModel.Value.Remark) + '</textarea>';
                            }
                            },
                            { Name: "Handler", Value: item.Handler },
                            { Name: "Extension", Value: item, CustomCellRender: function (columnModel) {
                                return generateDropdown(scope.$parent.stations, "Extension", "StationId", "Name", columnModel.Value.Extension);
                            }
                            },
                            { Name: "ProductStatus", Value: item, CustomCellRender: function (columnModel) {
                                return generateDropdown(scope.$parent.productStatuses, "ProductStatus", "StatusId", "Name", columnModel.Value.ProductStatus);
                            }
                            },
                        ],
                    CssClass: item.GroupId > 0 ? 'headline' : 'direct',
                    CustomAttr: 'data-headline-id=' + item.HeadlineId + ' data-rowindex=' + i
                });
            }
        }

        var gridOptions = {
            columns: [
                    { HeaderText: viewResources.DirectAccount, Width: 120, Name: "DirectAccount", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: viewResources.Status, Width: 120, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.UpdatedTime, Width: 100, Name: "UpdatedTime", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MMM DD, YYYY" },
                    { HeaderText: viewResources.Remark, Width: 200, Name: "Remark", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.Handler, Width: 100, Name: "Handler", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.Extension, Width: 120, Name: "Extension", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.ProductStatus, Width: 120, Name: "ProductStatus", HeaderAlign: "center", CellAlign: "center" }
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
            gridExpandHeight: 230,
            showPager: true,
            pagerOption: {
                itemsPerPage: 10,
                maxItemsPerPage: 100,
                currentPage: 0,
                totalItem: scope.dataSource.length,
                startPerPage: 10,
                pageStep: 10
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
                renderListData(scope, element, attrs);
            });
        }
    };
} ]);