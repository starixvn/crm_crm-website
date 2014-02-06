"use strict";
/* global editLogApplication, crmConstants, viewResources */
editLogApplication.directive('dropdownCountry', function () {
    return {
        /* jshint camelcase: false */
        restrict: 'A',
        template: '<option value="' + crmConstants.Enums.Country.NotApply +
                        ' data-ng-click="updateSelectedCountry(country);">' +
                        viewResources.Symbol_NotApply +
                  '</option>\
                   <option data-ng-repeat="country in countryList track by $index"' +
                        ' data-ng-click="updateSelectedCountry(country);"' +
                        ' data-ng-selected="country.Id === communicationLog.Country.Id"' +
                        ' value="{{ country.Id }}">{{ country.Name }}</option>',
        replace: false,
        priority: 0,
        transclude: false,
        scope: true,
        terminal: false,
        require: false,
        compile: function compile(tElement, tAttrs, transclude) {
            /* jshint unused: vars */
        },
        link: function (scope, elem, attrs) {
            /* jshint unused: vars */
        }
    };
});