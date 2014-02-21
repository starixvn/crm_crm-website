"use strict";
/* global editLogApplication, crmConstants, viewResources */
editLogApplication.directive('dropdownCountry', function () {
    function renderDropdown(scope, element, attrs) {
        /* jshint unused: vars */
        if (scope.countryList) {
            var dropdown = element.find("#country");
            dropdown.empty();
            /* jshint camelcase: false */
            dropdown.append(
                '<option value="' + crmConstants.Enums.Country.NotApply + '">' + viewResources.Symbol_NotApply +
                '</option>');
            var countryListLength = scope.countryList.length;
            for (var i = 0; i < countryListLength; i++) {
                var country = scope.countryList[i];
                dropdown.append(
                    '<option value="' + country.Id + '">' + country.Name + '</option>');
            }

            element.change(function () {
                scope.updateSelectedCountry(dropdown.find('option:selected').val());
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