"use strict";
/* global editLogApplication */
editLogApplication.directive('dropdownLanguage', function () {
    function renderDropdown(scope, element, attrs) {
        /* jshint unused: vars */
        if (scope.languageList) {
            var dropdown = element.find("#language");
            dropdown.empty();
            /* jshint camelcase: false */
            var languageListLength = scope.languageList.length;
            for (var i = 0; i < languageListLength; i++) {
                var language = scope.languageList[i];
                dropdown.append(
                    '<option value="' + language.Id +
                      (language.Id === scope.communicationLog.LanguageId ?
                        ' selected="selected"' :
                        '') + '">' + language.Name + '</option>');
            }

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