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
                var setting = listData[i];

                bodyData.push({
                    Columns: [
                                {
                                    Name: "ColRelation",
                                    Value: '<button type="button" class="actionButton" href="#">&nbsp;</button>'
                                },
                                { Name: "ColSettingName", Value: setting.Value },
                                { Name: "ColSettingId", Value: setting.Id },
                                { Name: "ColOrder", Value: setting.DisplayOrder}],
                    CssClass: 'editlog-link-row row-index-' + i
                });
            }
        }

        var gridOptions = {
            columns: [
                { HeaderText: viewResources.Relation, Width: 50, Name: "ColRelation", HeaderAlign: "center", CellAlign: "center" },
                {
                    HeaderText: viewResources.Setting,
                    Width: 200,
                    Name: "ColSettingName",
                    HeaderAlign: "center",
                    CellAlign: "left",
                    ShowCollapseExpand: true
                },
                { HeaderText: viewResources.Setting_Id, Width: 100, Name: "ColSettingId", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: viewResources.Order, Width: 100, Name: "ColOrder", HeaderAlign: "center", CellAlign: "right"}],
            showTitle: true,
            titleInfo: {
                text: "sharedScriptManager.stringFormat(viewResources.ViewListTitle, [scope.getSettingRelationsParams.MainSetting.SettingType])"
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
            columnsNeedHide: scope.hasSearchPermission === true ? null : [0] 
        };

        tableSetting.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
        var $contextMenu = $("#contextMenu");
        $document.click(function () {
            $contextMenu.hide();
        });
        angular.element(".grid-container").scroll(function () {
            $contextMenu.hide();
        });
        tableSetting.on("click", ".actionButton", function (e) {
            var $element = angular.element(e.target);

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