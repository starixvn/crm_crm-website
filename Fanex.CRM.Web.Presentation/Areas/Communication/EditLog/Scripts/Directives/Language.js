(function () {
    "use strict";
    /* global viewResources,
    crmConstants */
    /* jshint camelcase: false */
    angular.module('communicationlog-edit-application').directive('dropdownLanguage', function () {
        function renderDropdown(scope, element) {
            if (scope.languageList) {
                var dropdown = element.find("#language");
                dropdown.empty();
                var optionHtml = '<option value="' + crmConstants.Enums.Language.NotApply + '"' +
                    (crmConstants.Enums.Language.NotApply === scope.communicationLog.LanguageId ?
                    ' selected="selected"' :
                    '') +
                '>' + viewResources.Symbol_NotApply +
                '</option>';
                var languageListLength = scope.languageList.length;
                for (var i = 0; i < languageListLength; i++) {
                    var language = scope.languageList[i];
                    optionHtml += '<option value="' + language.Id + '"' +
                      (language.Id === scope.communicationLog.LanguageId ?
                        ' selected="selected"' :
                        '') + '>' + language.Name + '</option>';
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
                    /* jshint unused: false */
                    renderDropdown(scope, element);
                });
            }
        };
    });
})();