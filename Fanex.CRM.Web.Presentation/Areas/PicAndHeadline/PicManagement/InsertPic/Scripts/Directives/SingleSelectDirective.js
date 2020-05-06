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
                var options = scope[registerParams.OptionName];
                var target = scope.$parent.x;
                console.log("scope");
                console.log(scope.$parent.x);
                
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
                        (crmConstants.Enums.Setting.NotApply === options.SelectedId ?
                        ' selected="selected"' :
                        '') +
                   '>' +
                  viewResources.Select + '</option>';
                for (var i = 0; i < dataListLength; i++) {
                    var data = options.DataList[i];
                    optionHtml +=
                    '<option value="' + data[registerParams.DataPropertyName.Id] + '"' +
                        (data[registerParams.DataPropertyName.Id] === options.SelectedId ?
                        ' selected="selected"' :
                        '') +
                   '>' +
                   data[registerParams.DataPropertyName.Value] + '</option>';
                }

                dropdown.append(optionHtml);
                element.change(function () {
                    if (options.OnChangeEvent &&
                    typeof (options.OnChangeEvent) === "function") {
                        var selectedItem = getSelectedItem(dropdown.find('option:selected').val());
                        options.OnChangeEvent(selectedItem, target);
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
                restrict: registerParams.RestrictType,
                scope: true,
                link: function (scope, element, attrs) {
                    scope.$watch(attrs.ngModel, function (newval, oldval) {
                        /* jshint unused: false */
                        renderDropdown(scope, element);
                    });
                }
            };
        });
    };
})();

