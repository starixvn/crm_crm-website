"use strict";
/* global directiveManager */
/* jshint unused: vars */
directiveManager = directiveManager || {};
directiveManager.registerDropdownContainer = function (angularApplication, registerParams) {
    angularApplication.directive(registerParams.PlugInName, function () {
        //        var defaultRegisterParams = {
        //            PlugInName: null,
        //            OptionName: null,
        //            PickerId: null
        //        };

        //        var defaultOption = {
        //            DataList: null,
        //            OnChangeEvent: null
        //        };

        var dataList = [];
        function renderDropdown(scope, element, attrs) {
            var options = scope[registerParams.OptionName];
            if (options === null ||
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
            selectedId = parseInt(selectedId);
            var dataListLength = dataList.length;
            var selectedItem = null;
            for (var i = 0; i < dataListLength; i++) {
                var data = dataList[i];
                if (data.Id === selectedId) {
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
                    /* jshint unused: vars */
                    renderDropdown(scope, element, attrs);
                });
            }
        };
    });
};

