"use strict";
/* global directiveManager: true,
crmConstants */
/* jshint unused: vars */
directiveManager = directiveManager || {};
directiveManager.registerMultiSelectContainer = function (angularApplication, registerParams) {
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
        var cachedData = {
            DataList: [],
            Options: null
        };

        function getOptions(dataList, prefixId, currentOptionLevel, options) {
            var dropdownOptions = [];
            if (dataList === undefined ||
                dataList === null ||
                dataList.length === 0) {
                return dropdownOptions;
            }

            var dataListLength = dataList.length;
            for (var i = 0; i < dataListLength; i++) {
                var info = dataList[i];
                var subOptions = options.NumberOfSelectedLevel &&
                    options.NumberOfSelectedLevel > currentOptionLevel ?
                        getOptions(info[options.SubOptionsName], prefixId, currentOptionLevel + 1, options) :
                        [];
                dropdownOptions.push({
                    id: prefixId + "-" + info[registerParams.DataPropertyName.Id],
                    value: info[registerParams.DataPropertyName.Id],
                    text: info[registerParams.DataPropertyName.Name],
                    checked: true,
                    enable: true,
                    cssClass: "",
                    items: subOptions
                });
            }

            return dropdownOptions;
        }

        function renderDropdownLanguage(scope, element, attrs) {
            var options = scope[registerParams.OptionName];
            if (options === null ||
                options.DataList === null) {
                return;
            }

            cachedData.Options = options;
            var prefixId = registerParams.PickerId;
            var dropdown = element.find("#" + prefixId);
            var dropdownOptions = [];
            var dataList = options.DataList;
            cachedData.DataList = dataList;
            dropdownOptions = getOptions(dataList, prefixId, 1, options);
            dropdown.empty();
            dropdown.fanexMultiSelect(
            {
                minWidth: options.MinWidth,
                maxHeight: options.MaxHeight,
                shadow: true,
                showHeader: true,
                isCheckAll: true,
                showFooter: false,
                showTooltip: false,
                checkAllText: options.AllText,
                isUsedCustomCheckAllText: true,
                customCheckAllText: options.AllText,
                noItemText: options.NoItemText,
                noneSelectedText: options.NoneSelectedText,
                selectedList: options.NumberOfItemShowShortText,
                listData: dropdownOptions,
                autoCheckAll: options.AutoCheckAll
            });

            elementChange(element);
            element.on('change', function () {
                elementChange(element);
            });

            $("#" + prefixId + " input.fanex-multiselect-state-default").change(function () {
                elementChange(element);
            });
        }

        function elementChange(element) {
            if (cachedData.Options.OnChangeEvent &&
                typeof (cachedData.Options.OnChangeEvent) === "function") {
                var selectedOptions = element.find("#" + registerParams.PickerId).fanexMultiSelect("getItems", true);
                switch (cachedData.Options.MultiSelectType) {
                    case crmConstants.Enums.MultiSelectType.GetIdList:
                        cachedData.Options.OnChangeEvent(getSelectedIdList(selectedOptions));
                        break;
                    case crmConstants.Enums.MultiSelectType.GetFullInfoList:
                        cachedData.Options.OnChangeEvent(getSelectedItems(selectedOptions, cachedData.DataList, 1));
                        break;
                    default:
                        cachedData.Options.OnChangeEvent(getSelectedIdList(selectedOptions));
                        break;
                }
            }
        }

        function getSelectedIdList(selectOptions) {
            return selectOptions.join(",");
        }

        function getSelectedItems(selectOptions, dataList, currentOptionLevel) {
            var dataListLength = cachedData.DataList.length;
            var selectedItems = [];
            if (dataList === undefined ||
                dataList === null ||
                dataList.length === 0) {
                return selectedItems;
            }

            for (var i = 0; i < dataListLength; i++) {
                var info = dataList[i];
                if (cachedData.Options.NumberOfSelectedLevel &&
                    currentOptionLevel < cachedData.Options.NumberOfSelectedLevel) {
                    selectedItems = selectedItems.concat(getSelectedItems(selectOptions, info[cachedData.Options.SubOptionsName], currentOptionLevel + 1));
                } else if (selectOptions.indexOf(info[registerParams.DataPropertyName.Id].toString()) !== -1) {
                    selectedItems.push(info);
                }
            }

            return selectedItems;
        }

        return {
            restrict: registerParams.RestrictType,
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (oldval, newval) {
                    renderDropdownLanguage(scope, element, attrs);
                });
            }
        };
    });
};