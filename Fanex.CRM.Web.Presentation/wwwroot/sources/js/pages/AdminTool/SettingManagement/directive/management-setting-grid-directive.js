(function (angular) {
    'use strict';

    angular.module('addmintool-settingmanagement-application').directive('managementSettingGrid', managementSettingGrid);
    managementSettingGrid.$inject = [];

    function managementSettingGrid() {
        var selfScope;
        var settingManagementView;
        var currentSettingType;

        function renderSettingManagement(scope, element) {
            selfScope = scope;
            settingManagementView = scope.settingManagementView;
            if (scope.settingManagementView === null) {
                return;
            }

            currentSettingType = scope.getSettingRelationsParams.MainSetting.SettingType;
            var tableSetting = element.find("#settingmanagement-list");
            var bodyData = [];
            tableSetting.empty();
            var settingLength = scope.settingManagementView.length;
            if (settingLength > 0) {
                for (var i = 0; i < settingLength; i++) {
                    var setting = settingManagementView[i];
                    var checked = setting.IsActive === true ? "checked" : "";
                    var collumns = [];
                    collumns.push({ Name: "ColSettingName", Value: setting.Value });

                    if (hasSourceMappingCollumn(currentSettingType)) {
                        collumns.push({ Name: "SourceMapping", Value: renderSourceMappingColumn(setting.SettingSource) });
                    }

                    collumns.push(
                        { Name: "ColSettingId", Value: setting.Id },
                        { Name: "ColOrder", Value: setting.DisplayOrder },
                        {
                            Name: "ColEdit",
                            Value: '<a class="c hand editSetting" href="JavaScript:void(0);" data-row-index="' + i + '"><i class="fa fa-pencil"></i></a>'
                        },
                        {
                            Name: "ColActive",
                            Value: '<input type="checkbox" class="activeSetting"' + checked + ' data-row-index="' + i + '" />'
                        });

                    bodyData.push({
                        Columns: collumns,
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
                    }],
                showTitle: true,
                titleInfo: {
                    text: sharedScriptManager.stringFormat(viewResources.ViewListTitle, [scope.getSettingRelationsParams.MainSetting.SettingType])
                },
                bodyRows: bodyData,
                headerColumnHeight: 29,
                bodyRowHeight: 25
            };

            if (hasSourceMappingCollumn(currentSettingType)) {
                gridOptions.columns.push({
                    HeaderText: viewResources.SourceMapping,
                    Width: 400,
                    Name: "SourceMapping",
                    HeaderAlign: "center",
                    CellAlign: "left",
                    ShowCollapseExpand: true
                });
            }

            gridOptions.columns.push(
                { HeaderText: viewResources.Setting_Id, Width: 80, Name: "ColSettingId", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: viewResources.Order, Width: 60, Name: "ColOrder", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: viewResources.Edit, Width: 60, Name: "ColEdit", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Active, Width: 60, Name: "ColActive", HeaderAlign: "center", CellAlign: "center" }
            );

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

        function hasSourceMappingCollumn(settingType) {
            return (settingType === 'Product'
                || settingType === 'SubProduct'
                || settingType === 'BetType')
        }

        function renderSourceMappingColumn(sourceMapping) {
            let html = sourceMapping;
            const maxItems = 20,
                sourceMappings = sourceMapping ? sourceMapping.split(',') : [];
            if (sourceMappings.length > maxItems) {
                html = '<span class="tooltip-tipsy" title="' + sourceMapping + '">' + sourceMappings.splice(0, maxItems).join(',') + ' ...</span>';
            }

            return html;
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
    }
})(angular);