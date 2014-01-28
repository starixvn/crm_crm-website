editLogApplication.directive('dropdownCountry', function () {
    return {
        restrict: 'A',
        template: '<option value="' + crmConstants.Enums.Country.NotApply +
                        ' data-ng-click="updateSelectedCountry(country);">' +
                        viewResources.Symbol_NotApply +
                  '</option>\
                   <option data-ng-repeat="country in countryList track by $index"' +
                        ' data-ng-click="updateSelectedCountry(country);"' +
                        ' value="{{ country.Id }}">{{ country.Name }}</option>',
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