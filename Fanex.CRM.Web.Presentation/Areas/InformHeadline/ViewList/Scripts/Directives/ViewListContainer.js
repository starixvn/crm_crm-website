"use strict";
/* global crmConstants,
viewResources */
/* jshint camelcase: false,
unused: vars */
angular.module('informHeadlineApplication').directive('informHeadlineListContainer', ['$document', function ($document) {
    var selfScope;
    var listData;

    function renderListData(scope, element) {
        selfScope = scope;
        listData = scope.listData;

        if (listData === null) {
            return;
        }

        var canContact = scope.contactPolicy !== "None";
        var $table = element.find("#inform-headline-list");
        var bodyData = [];
        $table.empty();
        var rowsCount = listData.length;
        if (rowsCount > 0) {
            for (var i = 0; i < rowsCount; i++) {
                var item = listData[i];

                bodyData.push({
                    Columns: [
                            {
                                Name: "Edit", Value: '<button type="button" class="actionButton">&nbsp;</button>'
                            },
                            { Name: "Title", Value: canContact ? '<a class="title" href="javascript:void(0)">' + item.Title + '</a>' : item.Title },
                            { Name: "CreatedDate", Value: item.ClientCreatedDate },
                            { Name: "NoDirectAccounts", Value: item.Total },
                            { Name: "Status", Value: item.Status },
                            { Name: "Done", Value: item.Done },
                            { Name: "RequestToCallBack", Value: item.RequestToCallBack },
                            { Name: "NoNeedToCall", Value: item.NoNeedToCall },
                            { Name: "NoReply", Value: item.NoReply },
                            { Name: "Uncontactable", Value: item.Uncontactable },
                            { Name: "InternalAccount", Value: item.InternalAccount },
                            { Name: "NeverTouch", Value: item.NeverTouch },
                            { Name: "KeptForReference", Value: item.KeptForReference }
                        ],
                    CssClass: 'status-' + item.Status.toLowerCase(),
                    CustomAttr: 'data-title-id=' + item.TitleId
                });
            }
        }

        var gridOptions = {
            columns: [
                    { HeaderText: viewResources.Edit, Width: 50, Name: "Edit", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.Title, Width: 250, Name: "Title", HeaderAlign: "center", CellAlign: "left", ShowCollapseExpand: true },
                    { HeaderText: viewResources.CreatedDate, Width: 100, Name: "CreatedDate", HeaderAlign: "center", FormatDateTime: "MMM DD, YYYY" },
                    { HeaderText: viewResources.NoDirectAccounts, Width: 100, Name: "NoDirectAccounts", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.Status, Width: 100, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.Done, Width: 100, Name: "Done", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.RequestToCallBack, Width: 100, Name: "RequestToCallBack", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.NoNeedToCall, Width: 100, Name: "NoNeedToCall", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.NoReply, Width: 100, Name: "NoReply", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.Uncontactable, Width: 100, Name: "Uncontactable", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.InternalAccount, Width: 100, Name: "InternalAccount", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.NeverTouch, Width: 100, Name: "NeverTouch", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: viewResources.KeptForReference, Width: 100, Name: "KeptForReference", HeaderAlign: "center", CellAlign: "center" }
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
            columnsNeedHide: rowsCount === 0 || scope.searchPolicy === "Host" ? null : [0]
        };

        $table.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);            
    }

    return {
        restrict: 'C',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (oldval, newval) {
                /* jshint unused: false */
                renderListData(scope, element);
            });

            var $table = element.find("#inform-headline-list");
            var $contextMenu = angular.element("#contextMenu");
            $document.click(function () {
                $contextMenu.hide();
            });
            angular.element(".grid-container").scroll(function () {
                $contextMenu.hide();
            });
            $contextMenu.on("click", "a", function (e) {
                var $element = angular.element(e.target);
                var titleId = $contextMenu.data("title-id");
                if ($element.hasClass("edit-menu-item")) {
                    scope.showUpdateCaseDialog(titleId);
                }
                else {

                    var status = $element.data("status");
                    scope.updateInformStatus(titleId, status);
                }
            });
            $table.on("click", ".actionButton", function (e) {
                var $target = angular.element(e.target);
                var titleId = $target.closest("tr").data("title-id");
                $contextMenu.data("title-id", titleId);
                $contextMenu.find("li").hide();
                if ($target.closest("tr").hasClass("status-closed")) {
                    $contextMenu.find(".status-closed").show();
                } else {
                    $contextMenu.find(":not(.status-closed)").show();
                }

                var offset = $target.offset();
                $contextMenu.show().css({
                    display: "block",
                    left: offset.left,
                    top: offset.top + $target.height()
                });
                return false;
            });

            $table.on("click", ".title", function (e) {
                var $target = angular.element(e.target);
                var titleId = $target.closest("tr").data("title-id");
                var isClosed = $target.closest("tr").hasClass("status-closed");
                scope.showContactDialog(titleId, isClosed);
            });
        }
    };
} ]);