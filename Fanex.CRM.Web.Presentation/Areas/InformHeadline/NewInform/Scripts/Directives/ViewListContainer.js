"use strict";
/* global viewListApplication,
crmConstants,
viewResources */
/* jshint camelcase: false,
unused: vars */
viewListApplication.directive('informHeadlineListContainer', function ($document) {
    var selfScope;
    var listData;

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
            for (var i = 0; i < rowsCount; i++) {
                var item = listData[i];

                bodyData.push({
                    Columns: [                            
                            { Name: "DirectAccount", Value: item.UserName },
                            { Name: "Status", Value: item.InformHeadlineStatusID },
                        ],
                    CssClass: 'status-' + item.Status.toLowerCase(),
                    CustomAttr: 'data-title-id=' + item.TitleId
                });
            }
        }

        var gridOptions = {
            columns: [
                    { HeaderText: viewResources.DirectAccount, Width: 100, Name: "DirectAccount", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.Status, Width: 100, Name: "Status", HeaderAlign: "center", CellAlign: "center" }                    
                ],
            showTitle: true,
            titleInfo: {
                text: viewResources.InformHeadlinesCases,
                showExcelButton: true,
                exportExcelFunction: function () {
                    scope.exportInformHeadlines();
                }
            },
            bodyRows: bodyData,
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 0,
            showDistinctRow: false,
            showHighlightRow: true,
            showTree: true,
            treeParentPaddingLeft: 5,
            headerColumnHeight: 29,
            bodyRowHeight: 25,
            showColumnsShowHide: false,
            columnsNeedHide: scope.permissionType === "Search" ? null : [0]
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
});