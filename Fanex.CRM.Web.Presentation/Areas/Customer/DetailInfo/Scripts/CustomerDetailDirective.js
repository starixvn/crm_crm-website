(function () {
    "use strict";
    /* global
    crmConstants,
    viewResources */
    angular.module('customerdetail-application').directive('dropdownLanguage', function () {
        function renderDropdownLanguage(scope, element) {
            var dropdown = element.find("#customerinfodetail-languageoptions");
            dropdown.empty();
            var dropdownLanguage = [];
            var prefixId = 'customerinfo-languageoptions-';
            var isCheckAll = true;

            var languageList = scope.languageList;
            if (languageList && languageList.length > 0) {
                var optionLength = languageList.length;
                for (var i = 0; i < optionLength; i++) {
                    var info = languageList[i];
                    var isChecked = scope.isCustomerKnowLanguage(info.Id);
                    if (!isChecked) {
                        isCheckAll = false;
                    }

                    dropdownLanguage.push({
                        id: prefixId + info.Id,
                        value: info.Id,
                        text: info.Name,
                        checked: isChecked,
                        enable: true,
                        cssClass: "",
                        items: []
                    });
                }
            }

            /* jshint camelcase: false */
            dropdown.fanexMultiSelect({
                minWidth: 177,
                maxHeight: 450,
                shadow: true,
                showHeader: true,
                isCheckAll: isCheckAll,
                showFooter: false,
                showTooltip: false,
                checkAllText: viewResources.All_Languages,
                isUsedCustomCheckAllText: true,
                customCheckAllText: viewResources.All_Languages,
                noItemText: viewResources.No_languages_found,
                noneSelectedText: viewResources.Symbol_NotApply,
                selectedList: 3,
                listData: dropdownLanguage
            });

            element.find("#customerinfodetail-languageoptions .fanex-multiselect-state-default").attr("tabindex", 7);
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                renderDropdownLanguage(scope, element);
                scope.$watch(attrs.ngModel, function (oldval, newval) {
                    /* jshint unused: false */
                    renderDropdownLanguage(scope, element);
                    scope.updateSelectedLanguage(element.find("#customerinfodetail-languageoptions").fanexMultiSelect("getItems", true).join(","));
                    scope.updateOldSelectedLanguage(element.find("#customerinfodetail-languageoptions").fanexMultiSelect("getItems", true).join(","));
                });

                element.on('change', function () {
                    scope.updateSelectedLanguage(element.find("#customerinfodetail-languageoptions").fanexMultiSelect("getItems", true).join(","));
                    if (!scope.$$phase) {
                        scope.$apply();
                    }
                });
            }
        };
    });
})();