editLogApplication.directive('dropdownLanguage', function () {
    return {
        restrict: 'A',
        template: '<option data-ng-repeat="language in languageList track by $index"' +
                        ' data-ng-click="updateSelectedLanguage(language);"' +
                        ' value="{{ language.Id }}"' +
                        ' data-ng-selected="language.Id === communicationLog.LanguageId">{{ language.Value }}</option>',
        replace: false,
        priority: 0,
        transclude: false,
        scope: true,
        terminal: false,
        require: false,
        compile: function compile(tElement, tAttrs, transclude) {
        },
        link: function (scope, elem, attrs) {
        }
    }
});