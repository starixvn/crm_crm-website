(function () {
    "use strict";
    /* global crmConstants,
    sharedScriptManager,
    viewResources */
    /* jshint camelcase: false,
    unused: vars */
    angular.module('addmintool-settingmanagement-application').directive('settingManagementListContainer', function () {
        var selfScope;
        var settingManagementView;

        function renderSettingManagement(scope, element, attrs) {
            selfScope = scope;
            settingManagementView = scope.settingManagementView;
            if (scope.settingManagementView === null) {
                return;
            }

            var tableSetting = element.find("#settingmanagement-list");
            var bodyData = [];
            tableSetting.empty();
            var settingLength = scope.settingManagementView.length;
            if (settingLength > 0) {
                for (var i = 0; i < settingLength; i++) {
                    var setting = settingManagementView[i];
                    bodyData.push({
                        Columns: [
                                { Name: "ColSettingName", Value: setting.Value },
                                { Name: "ColSettingId", Value: setting.Id },
                                { Name: "ColOrder", Value: setting.DisplayOrder },
                                {
                                    Name: "ColEdit",
                                    Value: '<a class="c hand editSetting" href="JavaScript:void(0);" data-row-index="' + i + '"></a>'
                                },
                                {
                                    Name: "ColDelete",
                                    Value: '<a class="c hand deleteSetting" href="JavaScript:void(0);" data-row-index="' + i + '"></a>'
                                }],
                        CssClass: 'editlog-link-row row-index-' + i
                    });
                }
            }

            var gridOptions = {
                columns: [
                {
                    HeaderText: viewResources.Setting,
                    Width: 400,
                    Name: "ColSettingName",
                    HeaderAlign: "center",
                    CellAlign: "left",
                    ShowCollapseExpand: true
                },
                { HeaderText: viewResources.Setting_Id, Width: 80, Name: "ColSettingId", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: viewResources.Order, Width: 60, Name: "ColOrder", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: viewResources.Edit, Width: 60, Name: "ColEdit", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Delete, Width: 60, Name: "ColDelete", HeaderAlign: "center", CellAlign: "center"}],
                showTitle: true,
                titleInfo: {
                    text: sharedScriptManager.stringFormat(viewResources.ViewListTitle, [scope.getSettingRelationsParams.MainSetting.SettingType])
                },
                bodyRows: bodyData,
                numberOfFrozenColumn: 1,
                fullWidth: true,
                fullHeight: true,
                gridHeight: 0,
                showDistinctRow: false,
                showHighlightRow: true,
                showTree: true,
                treeParentPaddingLeft: 5,
                headerColumnHeight: 29,
                bodyRowHeight: 25
            };

            tableSetting.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);

            var deleteIcons = tableSetting.find(".deleteSetting");
            deleteIcons.unbind("click");
            deleteIcons.click(function () {
                var rowIndex = parseInt($(this).attr("data-row-index"));
                selfScope.deleteSetting(settingManagementView[rowIndex]);
            });

            var editIcons = tableSetting.find(".editSetting");
            editIcons.unbind("click");
            editIcons.click(function () {
                var rowIndex = parseInt($(this).attr("data-row-index"));
                selfScope.editSetting(settingManagementView[rowIndex]);
            });
        }

        return {
            restrict: 'C',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (oldval, newval) {
                    renderSettingManagement(scope, element, attrs);
                });
            }
        };
    });
})();