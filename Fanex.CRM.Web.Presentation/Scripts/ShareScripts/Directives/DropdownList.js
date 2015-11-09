"use strict";
/* global directiveManager: true */
/* jshint unused: false */
directiveManager = directiveManager || {};
directiveManager.registerDropdownContainer = function (angularApplication, registerParams) {
    angularApplication.directive(registerParams.PlugInName, function () {
        var dataList = [];
        function renderDropdown(scope, element, attrs) {
            var options = scope[registerParams.OptionName];
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
            for (var i = 0; i < dataListLength; i++) {
                var data = options.DataList[i];
                optionHtml +=
                    '<option value="' + data[registerParams.DataPropertyName.Id] + '"' +
                        (data[registerParams.DataPropertyName.Id] === options.SelectedId ?
                        ' selected="selected"' :
                        '') +
                   '>' +
                   data[registerParams.DataPropertyName.Name] + '</option>';
            }

            dropdown.append(optionHtml);
            element.change(function () {
                if (options.OnChangeEvent &&
                    typeof (options.OnChangeEvent) === "function") {
                    var selectedItem = getSelectedItem(dropdown.find('option:selected').val());
                    options.OnChangeEvent(selectedItem);
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

            return selectedItem;
        }

        return {
            restrict: registerParams.RestrictType,
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    renderDropdown(scope, element, attrs);
                });
            }
        };
    });
};

