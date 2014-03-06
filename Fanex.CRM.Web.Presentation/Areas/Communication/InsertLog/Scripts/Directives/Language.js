"use strict";
/* global insertLogApplication */
/* jshint camelcase: false */
insertLogApplication.directive('dropdownLanguage', function () {
    function renderDropdown(scope, element, attrs) {
        /* jshint unused: vars */
        if (scope.languageList) {
            var dropdown = element.find("#language");
            dropdown.empty();
            var optionHtml = '<option value="' + crmConstants.Enums.Language.NotApply + '">' + viewResources.Symbol_NotApply +
                '</option>';
            var languageListLength = scope.languageList.length;
            for (var i = 0; i < languageListLength; i++) {
                var language = scope.languageList[i];
                optionHtml += '<option value="' + language.Id + '">' + language.Name + '</option>';
            }

            dropdown.append(optionHtml);
            element.change(function () {
                scope.updateSelectedLanguage(dropdown.find('option:selected').val());
            });
        }
    }

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (newval, oldval) {
                /* jshint unused: vars */
                renderDropdown(scope, element, attrs);
            });
        }
    };
});