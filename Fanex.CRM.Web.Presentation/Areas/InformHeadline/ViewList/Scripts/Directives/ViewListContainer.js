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

        var tableSetting = element.find("#inform-headline-list");
        var bodyData = [];
        tableSetting.empty();
        var rowsCount = scope.listData.length;
        if (rowsCount > 0) {
            for (var i = 0; i < rowsCount; i++) {
                var item = listData[i];

                bodyData.push({
                    Columns: [
                            {
                                Name: "Edit", Value: '<button type="button" class="actionButton" href="#">&nbsp;</button>'
                            },
                            { Name: "Title", Value: '<a href="javascript:void(0)">' + item.Title + '</a>' },
                            { Name: "CreatedDate", Value: item.CreatedDate },
                            { Name: "NoDirectAccounts", Value: item.Total },
                            { Name: "Status", Value: item.Status },
                            { Name: "Done", Value: item.Done },
                            { Name: "RequestToCallBack", Value: item.RequestToCallBack },
                            { Name: "NoNeedToCall", Value: item.NoNeedToCall },
                            { Name: "NoReply", Value: item.NoReply },
                            { Name: "Uncontactable", Value: item.Uncontactable },
                            { Name: "InternalAccount", Value: item.InternalAccount },
                            { Name: "NeverTouch", Value: item.NeverTouch }
                        ],
                    CssClass: "status-" + item.Status.toLowerCase()
                });
            }
        }

        var gridOptions = {
            columns: [
                    { HeaderText: viewResources.Edit, Width: 50, Name: "Edit", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.Title, Width: 200, Name: "Title", HeaderAlign: "center", CellAlign: "left", ShowCollapseExpand: true },
                    { HeaderText: viewResources.CreatedDate, Width: 100, Name: "CreatedDate", HeaderAlign: "center", FormatDateTime: "MMM DD, YYYY" },
                    { HeaderText: viewResources.NoDirectAccounts, Width: 100, Name: "NoDirectAccounts", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.Status, Width: 100, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.Done, Width: 100, Name: "Done", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.RequestToCallBack, Width: 100, Name: "RequestToCallBack", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.NoNeedToCall, Width: 100, Name: "NoNeedToCall", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.NoReply, Width: 100, Name: "NoReply", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.Uncontactable, Width: 100, Name: "Uncontactable", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.InternalAccount, Width: 100, Name: "InternalAccount", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.NeverTouch, Width: 100, Name: "NeverTouch", HeaderAlign: "center", CellAlign: "center" }
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

        tableSetting.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
        var $contextMenu = angular.element("#contextMenu");
        $document.click(function () {
            $contextMenu.hide();
        });
        angular.element(".grid-container").scroll(function () {
            $contextMenu.hide();
        });
        tableSetting.on("click", ".actionButton", function (e) {
            var $element = angular.element(e.target);
            
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