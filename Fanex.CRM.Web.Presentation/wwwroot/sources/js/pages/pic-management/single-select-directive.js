(function () {
    "use strict";
    /* global directiveManager: true,
    crmConstants,
    viewResources */
    /* jshint camelcase: false,
    unused: vars */
    directiveManager = directiveManager || {};
    directiveManager.registerSingleSelectDirectiveContainer = function (angularApplication, registerParams) {
        angularApplication.directive(registerParams.PlugInName, function () {
            var dataList = [];
            function renderDropdown(scope, element) {
                var options = scope.selectOptions;
                var target = scope.target;
                var selectedValue = scope.selectedValue;
                
                if (options === null ||
                options === undefined ||
                options.DataList === null) {
                    return;
                }

                dataList = options.DataList;
                var dropdown = element.find("#" + registerParams.PickerId);
                dropdown.empty();
                var optionHtml = '';
                var dataListLength = options.DataList.length;
                optionHtml +=
                    '<option value="' + crmConstants.Enums.Setting.NotApply + '"' +
                (crmConstants.Enums.Setting.NotApply === selectedValue ?
                        ' selected="selected"' :
                        '') +
                   '>' +
                  viewResources.Selected + '</option>';
                for (var i = 0; i < dataListLength; i++) {
                    var data = options.DataList[i];
                    optionHtml +=
                    '<option value="' + data[registerParams.DataPropertyName.Id] + '"' +
                    (data[registerParams.DataPropertyName.Id] === selectedValue ?
                        ' selected="selected"' :
                        '') +
                   '>' +
                   data[registerParams.DataPropertyName.Value] + '</option>';
                }

                dropdown.append(optionHtml);
                element.change(function () {
                    if (scope.onChangeEvent &&
                        typeof (scope.onChangeEvent) === "function") {
                        var selectedItem = getSelectedItem(dropdown.find('option:selected').val());
                        scope.onChangeEvent({target: target, selectedItem: selectedItem});
                    }
                });
            }

            function getSelectedItem(selectedId) {
                var dataListLength = dataList.length;
                var selectedItem = null;
                for (var i = 0; i < dataListLength; i++) {
                    var data = dataList[i];
                    if (data[registerParams.DataPropertyName.Id] === selectedId ||
                    data[registerParams.DataPropertyName.Id] === parseInt(selectedId)) {
                        selectedItem = data;
                        break;
                    }
                }

                if (selectedItem === null) {
                    return {
                        Id: crmConstants.Enums.Setting.NotApply,
                        SettingType: viewResources.Symbol_NotApply,
                        Value: viewResources.Symbol_NotApply
                    };
                }

                return selectedItem;
            }

            return {
                restrict: 'C',
                scope: {
                    selectOptions: '=',
                    target: '=',
                    selectedValue: '=',
                    onChangeEvent: '&'
                },
                link: function (scope, element, attrs) {
                    scope.$watch('selectOptions.DataList', function (newval, oldval) {
                        /* jshint unused: false */
                        renderDropdown(scope, element);
                    });
                }
            };
        });
    };
})();

