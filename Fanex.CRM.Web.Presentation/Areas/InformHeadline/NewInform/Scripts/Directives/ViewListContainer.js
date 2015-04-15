"use strict";
/* global viewListApplication,
crmConstants,
viewResources */
/* jshint camelcase: false,
unused: vars */
viewListApplication.directive('informHeadlineListContainer', ['$document', function ($document) {
    var selfScope;
    var listData;

    function generateDropdown(data, valueProperty, textProperty, selectedValue) {
        var html = '<select style="width: 100px">';

        for (var i = 0; i < data.length; i++) {
            var item = data[i];
            html += '<option value="' + item[valueProperty] + '" ' +
                    (item[valueProperty] == selectedValue ? 'selected' : '') +
                     ' >' + item[textProperty] + '</option>';
        }
        html += '</select>';

        return html;
    }



    function renderListData(scope, element, attrs) {
        selfScope = scope;
        listData = scope.listData;

        if (scope.listData === null) {
            return;
        }

        var $tableSetting = element.find("#inform-headline-list");
        var bodyData = [];
        $tableSetting.empty();
        var rowsCount = scope.listData.length;
        if (rowsCount > 0) {

            var stationDropdownHtml = generateDropdown(scope.stations, "StationId", "Name");
            var productStatusDropdownHtml = generateDropdown(scope.productStatuses, "StatusId", "Name");

            for (var i = 0; i < rowsCount; i++) {
                var item = listData[i];
                bodyData.push({
                    Columns: [
                            { Name: "DirectAccount", Value: item.UserName },
                            { Name: "Status", Value: generateDropdown(scope.informStatuses, "SettingId", "SettingValue", item.InformHeadlineStatusId) },
                            { Name: "UpdatedTime", Value: item.UpdatedTime },
                            { Name: "Remark", Value: "<textarea></textarea>" },
                            { Name: "Handler", Value: item.UpdatedTime },
                            { Name: "Extension", Value: stationDropdownHtml },
                            { Name: "ProductStatus", Value: productStatusDropdownHtml }
                        ],
                    CssClass: item.GroupId > 0 ? 'headline' : 'direct'
                });
            }
        }

        var gridOptions = {
            columns: [
                    { HeaderText: viewResources.DirectAccount, Width: 120, Name: "DirectAccount", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.Status, Width: 120, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.UpdatedTime, Width: 100, Name: "UpdatedTime", HeaderAlign: "center", CellAlign: "center" },
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
                totalItem: scope.listData.length,
                startPerPage: 10,
                pageStep: 10
            }
        };

        $tableSetting.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
        var $contextMenu = angular.element("#contextMenu");
        $document.click(function () {
            $contextMenu.hide();
        });
        angular.element(".grid-container").scroll(function () {
            $contextMenu.hide();
        });
        $contextMenu.on("click", "a", function (e) {
            var $element = angular.element(e.target);
            if ($element.hasClass("edit-menu-item")) {
                // not implemented.
            }
            else {
                var titleId = $contextMenu.data("title-id");
                var status = $element.data("status");
                scope.updateInformStatus(titleId, status);
            }
        });
        $tableSetting.on("click", ".actionButton", function (e) {
            var $element = angular.element(e.target);
            var titleId = $element.closest("tr").data("title-id");
            $contextMenu.data("title-id", titleId);
            $contextMenu.find("li").hide();
            if ($element.closest("tr").hasClass("status-closed")) {
                $contextMenu.find(".status-closed").show();
            } else {
                $contextMenu.find(":not(.status-closed)").show();
            }

            var offset = $element.offset();
            $contextMenu.show().css({
                display: "block",
                left: offset.left,
                top: offset.top + $element.height()
            });
            return false;
        });
    }

    return {
        restrict: 'C',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (oldval, newval) {
                renderListData(scope, element, attrs);
            });
        }
    };
} ]);