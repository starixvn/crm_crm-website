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

        function renderSettingManagement(scope, element) {
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
                    var checked = setting.IsActive === true ? "checked" : "";
                    bodyData.push({
                        Columns: [
                                { Name: "ColSettingNameEnglish", Value: setting.Value },
                                { Name: "ColSettingNameSimplChinese", Value: setting.Value },
                                { Name: "ColSettingNameTradChinese", Value: setting.Value },
                                { Name: "ColSettingId", Value: setting.Id },
                                { Name: "ColOrder", Value: setting.DisplayOrder },
                                {
                                    Name: "ColEdit",
                                    Value: '<a class="c hand editSetting" href="JavaScript:void(0);" data-row-index="' + i + '"></a>'
                                },
                                {
                                    Name: "ColActive",
                                    Value: '<input type="checkbox" class="activeSetting"' + checked + ' data-row-index="' + i + '" />'
                                }],
                        CssClass: 'editlog-link-row row-index-' + i
                    });
                }
            }

            var gridOptions = {
                customizeHeader: true,
                groupHeaderRows: [
                    {
                        Columns: [
                            {
                                HeaderText: viewResources.Setting,                               
                                Name: "ColSettingNameEnglish",
                                MergeColNumber: 3
                            },
                            { HeaderText: viewResources.Setting_Id, Name: "ColSettingId", MergeRowNumber: 2 },
                            { HeaderText: viewResources.Order, Name: "ColOrder", MergeRowNumber: 2 },
                            { HeaderText: viewResources.Edit, Name: "ColEdit", MergeRowNumber: 2 },
                            { HeaderText: viewResources.Active, Name: "ColActive", MergeRowNumber: 2 }
                        ]
                    }
                ],
                columns: [                
                { HeaderText: viewResources.English, Width: 150, Name: "ColSettingNameEnglish", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: viewResources.SimplifiedChinese, Width: 150, Name: "ColSettingNameSimplChinese", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: viewResources.TraditionalChinese, Width: 150, Name: "ColSettingNameTradChinese", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: viewResources.Setting_Id, Width: 80, Name: "ColSettingId", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: viewResources.Order, Width: 60, Name: "ColOrder", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: viewResources.Edit, Width: 60, Name: "ColEdit", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Active, Width: 60, Name: "ColActive", HeaderAlign: "center", CellAlign: "center"}],
                showTitle: true,
                titleInfo: {
                    text: sharedScriptManager.stringFormat(viewResources.ViewListTitle, [scope.getSettingRelationsParams.MainSetting.SettingType])
                },
                bodyRows: bodyData,
                //numberOfFrozenColumn: 1,
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

            var activeSettings = tableSetting.find(".activeSetting");
            activeSettings.unbind("click");
            activeSettings.click(function () {
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
                    /* jshint unused: false */
                    renderSettingManagement(scope, element, attrs);
                });
            }
        };
    });
})();