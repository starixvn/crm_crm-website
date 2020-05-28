angular.module('fanexMultiSelectModule', []).directive('fanexMultiSelect', function () {
    function getOptions(dataList, prefixId, currentOptionLevel, options, selectedList) {
        var dropdownOptions = [];
        if (dataList === undefined ||
            dataList === null ||
            dataList.length === 0) {
            return dropdownOptions;
        }

        var dataListLength = dataList.length;
        for (var i = 0; i < dataListLength; i++) {
            var info = dataList[i];
            var subOptions = options && options.NumberOfSelectedLevel &&
                options.NumberOfSelectedLevel > currentOptionLevel ?
                getOptions(info[options.SubOptionsName], prefixId, currentOptionLevel + 1, options) :
                [];
            dropdownOptions.push({
                id: prefixId + "-" + info["Name"],
                value: info["Name"],
                text: info["Name"],
                checked: selectedList.includes(info["Name"]),
                enable: true,
                cssClass: "",
                items: subOptions
            });
        }

        return dropdownOptions;
    }

    function renderDropdown(scope, element, attrs) {
        if (scope.dataList === null) {
            return;
        }

        var dataList = scope.dataList;
        var options = scope.options;
        var selectedList = scope.selectedList;
        var dropdown = element.find("#language");
        var dropdownOptions = getOptions(dataList, 'language', 1, options, selectedList);
        console.log(dropdownOptions, dataList)
        dropdown.empty();
        dropdown.fanexMultiSelect(
            {
                minWidth: 150,
                maxHeight: 400,
                shadow: true,
                showHeader: true,
                isCheckAll: selectedList.split(',').length === dataList.length,
                showFooter: false,
                showTooltip: false,
                checkAllText: "All",
                isUsedCustomCheckAllText: true,
                customCheckAllText: "All",
                noItemText: "No item",
                noneSelectedText: "Select",
                selectedList: 1,
                listData: dropdownOptions,
                autoCheckAll: false
            });

        elementChange(element, scope);
        element.on('change', function () {
            elementChange(element, scope);
        });

        $(element).find("input.fanex-multiselect-state-default").change(function () {
            console.log(element, 'merlin')
            elementChange(element, scope);
        });
    }

    function elementChange(element, scope) {
        if (scope.onChange && typeof (scope.onChange) === "function") {
            var selectedOptions = element.fanexMultiSelect("getItems", true);
            console.log(scope.multiSelectType, crmConstants.Enums.MultiSelectType)
            switch (scope.multiSelectType) {
                case crmConstants.Enums.MultiSelectType.GetIdList:
                    scope.onChange(getSelectedIdList(selectedOptions));
                    break;
                case crmConstants.Enums.MultiSelectType.GetFullInfoList:
                    scope.onChange(getSelectedItems(selectedOptions, scope.dataList, 1, scope.options));
                    break;
                default:
                    scope.onChange(getSelectedIdList(selectedOptions));
                    break;
            }
        }
    }

    function getSelectedIdList(selectOptions) {
        return selectOptions.join(",");
    }

    function getSelectedItems(selectOptions, dataList, currentOptionLevel, options) {
        var dataListLength = dataList.length;
        var selectedItems = [];
        if (dataList === undefined ||
            dataList === null ||
            dataList.length === 0) {
            return selectedItems;
        }

        for (var i = 0; i < dataListLength; i++) {
            var info = dataList[i];
            if (options && options.NumberOfSelectedLevel &&
                currentOptionLevel < scope.options.NumberOfSelectedLevel) {
                selectedItems = selectedItems.concat(getSelectedItems(selectOptions, info[options.SubOptionsName], currentOptionLevel + 1));
            } else if (selectOptions.indexOf(info["Name"].toString()) !== -1) {
                selectedItems.push(info);
            }
        }

        return selectedItems;
    }

    return {
        restrict: 'A',
        require: 'ngModel',
        scope: {
            dataList: '=ngModel',
            options: '=',
            selectedList: '=',
            onChange: '=',
            multiSelectType: '='

        },
        link: function (scope, element, attrs) {
            scope.$watch('dataList', function (newval, oldval) {
                if (scope.dataList.length > 0) {
                    renderDropdown(scope, element, attrs);
                }
            });
        }
    };
});