(function () {
    "use strict";
    /* global crmConstants,
    viewResources,
    sharedScriptManager */
    /* jshint camelcase: false,
    unused: vars */
    angular.module('addmintool-settingrelation-addrelation-application').directive('mainSettingRelationListContainer', function () {
        var selfScope;
        var settingRelationView;

        function renderSettingRelations(scope, element) {
            selfScope = scope;
            settingRelationView = scope.mainSettingRelationView;
            if (scope.mainSettingRelationView === null) {
                return;
            }

            var tableSetting = element.find("#main-settingrelation-list");
            var bodyData = [];
            tableSetting.empty();
            var settingLength = scope.mainSettingRelationView.length;
            if (settingLength > 0) {
                for (var i = 0; i < settingLength; i++) {
                    var setting = settingRelationView[i];
                    bodyData.push({
                        Columns: [
                                { Name: "ColSettingNameEnglish", Value: setting.SettingValue },
                                { Name: "ColSettingNameSimplChinese", Value: setting.SettingValueCS },
                                { Name: "ColSettingNameTradChinese", Value: setting.SettingValueCT },
                                {
                                    Name: "ColIsSub",
                                    Value: '<input type="checkbox" id="chk' + setting.SettingID + '"' +
                                         (setting.IsSub ? ' checked="checked" ' : '') +
                                         ' data-row-index="' + i + '"' +
                                         ' class="hand checkbox-issub" name="chk" value="' + setting.SettingID + '"/>'
                                },
                                {
                                    Name: "ColOrder",
                                    Value: '<input class="c textbox-displayorder" type="text"' +
                                        ' id="txt' + setting.SettingID + '"' +
                                        ' data-row-index="' + i + '"' +
                                        ' value="' + setting.DisplayOrder + '" />'
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
                            { HeaderText: viewResources.Is_Sub, Name: "ColIsSub", MergeRowNumber: 2 },
                            { HeaderText: viewResources.Order, Name: "ColOrder", MergeRowNumber: 2 }                            
                        ]
                    }
                ],
                columns: [
                {
                    HeaderText: viewResources.English,
                    Width: 260,
                    Name: "ColSettingNameEnglish",
                    HeaderAlign: "center",
                    CellAlign: "left"
                },
                {
                    HeaderText: viewResources.SimplifiedChinese,
                    Width: 260,
                    Name: "ColSettingNameSimplChinese",
                    HeaderAlign: "center",
                    CellAlign: "left"
                },
                {
                    HeaderText: viewResources.TraditionalChinese,
                    Width: 260,
                    Name: "ColSettingNameTradChinese",
                    HeaderAlign: "center",
                    CellAlign: "left"
                },
                { HeaderText: viewResources.Is_Sub, Width: 60, Name: "ColIsSub", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Order, Width: 60, Name: "ColOrder", HeaderAlign: "center", CellAlign: "center"}],
                showTitle: false,
                bodyRows: bodyData,
                //numberOfFrozenColumn: 1,
                fullWidth: true,
                fullHeight: false,
                gridHeight: 400,
                showDistinctRow: false,
                showHighlightRow: true,
                headerColumnHeight: 29,
                bodyRowHeight: 25
            };

            tableSetting.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);

            var textboxDisplayOrder = tableSetting.find(".textbox-displayorder");
            textboxDisplayOrder.unbind("keypress");
            textboxDisplayOrder.keypress(function (e) {
                return sharedScriptManager.forceNumberInput(e);
            });

            textboxDisplayOrder.unbind("keyup");
            textboxDisplayOrder.keyup(function () {
                var rowIndex = parseInt($(this).attr("data-row-index"));
                selfScope.updateOrder(rowIndex, $(this).val());
            });

            var checkboxIsSub = tableSetting.find(".checkbox-issub");
            checkboxIsSub.unbind("click");
            checkboxIsSub.click(function () {
                var isCheck = $(this).attr("checked") ? true : false;
                var rowIndex = parseInt($(this).attr("data-row-index"));
                selfScope.updateIsSub(rowIndex, isCheck);
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