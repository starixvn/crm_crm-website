(function () {
    "use strict";
    /* global crmConstants,
    sharedScriptManager,
    viewResources */
    /* jshint camelcase: false,
    unused: vars */
    angular.module('addmintool-settingrelation-application').directive('settingRelationListContainer', function () {
        var selfScope;
        var settingRelationView;

        function renderSettingRelations(scope, element) {
            selfScope = scope;
            settingRelationView = scope.settingRelationView;
            if (scope.settingRelationView === null) {
                return;
            }

            var tableSetting = element.find("#settingrelation-list");
            var bodyData = [];
            tableSetting.empty();
            var settingLength = scope.settingRelationView.length;
            if (settingLength > 0) {
                for (var i = 0; i < settingLength; i++) {
                    var setting = settingRelationView[i];
                    var subRows = [];
                    var subSettingLength = setting.ChildSettings ?
                    setting.ChildSettings.length :
                    0;
                    for (var j = 0; j < subSettingLength; j++) {
                        var subSetting = setting.ChildSettings[j];
                        subRows.push({
                            Columns: [
                                { Name: "ColSettingName", Value: subSetting.Value },
                                { Name: "ColRelation", Value: '' },
                                { Name: "ColSettingId", Value: subSetting.Id },
                                { Name: "ColOrder", Value: subSetting.DisplayOrder}],
                            CssClass: j % 2 === 0 ? ' freegrid-even' : ''
                        });
                    }

                    bodyData.push({
                        Columns: [
                                { Name: "ColSettingName", Value: setting.Value },
                                {
                                    Name: "ColRelation",
                                    Value: '<a class="c hand addSetting" href="JavaScript:void(0);" data-row-index="' + i + '"></a>'
                                },
                                { Name: "ColSettingId", Value: setting.Id },
                                { Name: "ColOrder", Value: setting.DisplayOrder}],
                        SubRows: subRows,
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
                { HeaderText: viewResources.Relation, Width: 60, Name: "ColRelation", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Setting_Id, Width: 80, Name: "ColSettingId", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: viewResources.Order, Width: 60, Name: "ColOrder", HeaderAlign: "center", CellAlign: "right"}],
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

            var addIcons = tableSetting.find(".addSetting");
            addIcons.unbind("click");
            addIcons.click(function () {
                var rowIndex = parseInt($(this).attr("data-row-index"));
                selfScope.addRelation(settingRelationView[rowIndex]);
            });
        }

        return {
            restrict: 'C',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (oldval, newval) {
                    /* jshint unused: false */
                    renderSettingRelations(scope, element);
                });
            }
        };
    });
})();