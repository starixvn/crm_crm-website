"use strict";
/* global updateInformApplication,
crmConstants,
viewResources */
/* jshint camelcase: false,
unused: vars */
updateInformApplication.directive('informHeadlineListContainer', ['$document', function ($document) {
    var KEPT_FOR_REFERENCE_HEADLINE_ID = 45007;
    var MIN_PAGE_SIZE = 100;
    var MAX_PAGE_SIZE = 500;
    var selfScope;

    function generateDropdown(data, bindingName, valueProperty, textProperty, selectedValue, disabled) {
        var html = '<select ' + (disabled ? 'disabled' : '') + '  data-binding-name="' + bindingName + '" style="width: 100px">';

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
    function hasKeepThisAccountOption(caseId) {
        return caseId == 1 || caseId == 5 || caseId == 12;
    }

    function renderDropdownCell(columnModel, data, bindingName, valueProperty, textProperty, selectedValue) {
        if (columnModel.Value.InformHeadlineStatusId == KEPT_FOR_REFERENCE_HEADLINE_ID) {
            return '<span class="disabled-box">' + viewResources.KeptForReference + '</span>';
        }

        return generateDropdown(data, bindingName, valueProperty, textProperty, selectedValue, columnModel.Value.Case > 0);
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

    function generateBodyRows(scope, pageIndex, pageSize) {
        var bodyData = [];
        var rowsCount = scope.dataSource.length;
        var firstRowIndex = pageIndex * pageSize;
        if (rowsCount >= firstRowIndex) {

            var lastRowIndex = firstRowIndex + pageSize;
            if (lastRowIndex > rowsCount) {
                lastRowIndex = rowsCount;
            }

            for (var i = firstRowIndex; i < lastRowIndex; i++) {
                var item = scope.dataSource[i];

                var isUseOld = item.Case == 1 || item.Case == 5;
                var userName = isUseOld ? item.OldUserName : item.NewUserName;
                var headlineId = isUseOld ? item.OldCustId : item.NewCustId;

                bodyData.push({
                    Columns: [
                            { Name: "DirectAccount", Value: item.Case == 0 ? userName : '<span class="description-tooltip" data-case=' + item.Case + '>' + userName + '</span>' },
                            { Name: "Status", Value: item, CustomCellRender: function (columnModel) {
                                var html = "";
                                if (columnModel.Value.InformHeadlineStatusId == KEPT_FOR_REFERENCE_HEADLINE_ID) {
                                    return '<span class="disabled-box">' + viewResources.KeptForReference + '</span>';
                                }
                                else if (hasKeepThisAccountOption(columnModel.Value.Case)) {
                                    html = '<div class="checkbox display-inline-block"><label><input class="iskeep" value="true" type="checkbox" data-binding-name="IsKeep" />' +
                                            viewResources.KeepThisAccount + '</label></div>';
                                }

                                return html + generateDropdown(scope.$parent.informStatuses,
                                    "InformHeadlineStatusId", "SettingId", "SettingValue",
                                    columnModel.Value.InformHeadlineStatusId, columnModel.Value.Case > 0);
                            }
                            },
                            { Name: "UpdatedTime", Value: item.UpdatedTime },
                            { Name: "Remark", Value: item, CustomCellRender: function (columnModel) {
                                return '<textarea data-binding-name="Remark">' + htmlDecode(columnModel.Value.Remark) + '</textarea>';
                            }
                            },
                            { Name: "Handler", Value: item.Handler },
                            { Name: "Extension", Value: item, CustomCellRender: renderExtensionDropdownCell },
                            { Name: "ProductStatus", Value: item, CustomCellRender: renderProductStatusDropdownCell },
                        ],
                    CssClass: item.Case > 0 ? 'updated' : (item.GroupId > 0 ? 'headline' : 'direct'),
                    CustomAttr: 'data-headline-id=' + headlineId + ' data-rowindex=' + i
                });
            }
        }

        return bodyData;
    }
    function renderListData(scope, element, attrs) {
        selfScope = scope;
        if (scope.dataSource === null) {
            return;
        }

        var $tableSetting = element.find("#inform-headline-list");
        $tableSetting.empty();

        var gridOptions = {
            columns: [
                    { HeaderText: viewResources.DirectAccount, Width: 120, Name: "DirectAccount", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: viewResources.Status, Width: 150, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.UpdatedTime, Width: 100, Name: "UpdatedTime", HeaderAlign: "center", FormatDateTime: "MMM DD, YYYY" },
                    { HeaderText: viewResources.Remark, Width: 200, Name: "Remark", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.Handler, Width: 100, Name: "Handler", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: viewResources.Extension, Width: 120, Name: "Extension", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.ProductStatus, Width: 120, Name: "ProductStatus", HeaderAlign: "center", CellAlign: "center" }
                ],
            bodyRows: generateBodyRows(scope, 0, MIN_PAGE_SIZE),
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
            customPager: true,
            pagerOption: {
                itemsPerPage: MIN_PAGE_SIZE,
                maxItemsPerPage: MAX_PAGE_SIZE,
                currentPage: 0,
                totalItem: scope.dataSource.length,
                startPerPage: MIN_PAGE_SIZE,
                pageStep: MIN_PAGE_SIZE,
                pagerSelectCallBack: function (pageIndex, pageSize) {
                    gridOptions.pagerOption.pageSize = pageSize;
                    gridOptions.pagerOption.currentPage = pageIndex;
                    gridOptions.bodyRows = generateBodyRows(scope, pageIndex, pageSize);
                    $tableSetting.fanexGrid(gridOptions);

                    $tableSetting.find(".description-tooltip").hover(addTooltip);
                }
            }
        };

        function getDescriptionCase(dataItem, caseId) {
            var message = viewResources['ChangedHeadlineCase' + caseId];

            switch (parseInt(caseId)) {
                case 1:
                case 5:
                    {
                        message = message.replace('(A)', dataItem.OldUserName);
                        break;
                    }
                case 2:
                case 4:
                    {
                        message = message.replace('(A)', dataItem.NewUserName);
                        break;
                    }
                case 3:
                    {
                        message = message.replace('(A)', dataItem.OldUserName);
                        message = message.replace('(B)', dataItem.NewUserName);
                        break;
                    }
                default:
                    break;
            }

            return message;
        }

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

        $tableSetting.on("change", "select, input[type=checkbox]", function (e) {
            updateDataSource(angular.element(e.target));
        });

        $tableSetting.on("change", "textarea", function (e) {
            updateDataSource(angular.element(e.target), true);
        });

        function addTooltip(e) {
            var $target = angular.element(e.target);
            var rowIndex = parseInt($target.closest("tr").data("rowindex"));
            var caseId = $target.data("case");
            var message = getDescriptionCase(scope.dataSource[rowIndex], caseId);

            $target.attr("data-original-title", message).tooltip({ placement: "right" }).tooltip("show");
            $target.unbind("hover", addTooltip);
        }

        $tableSetting.find(".description-tooltip").hover(addTooltip);
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