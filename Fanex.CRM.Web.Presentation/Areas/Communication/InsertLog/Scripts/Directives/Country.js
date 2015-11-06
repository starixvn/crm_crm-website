(function () {
    "use strict";
    /* global crmConstants, viewResources */
    angular.module('communicationlog-insert-application').directive('dropdownCountry', function () {
        function renderDropdown(scope, element) {
            /* jshint unused: vars */
            if (scope.countryList) {
                var dropdown = element.find("#country");
                dropdown.empty();
                /* jshint camelcase: false */
                dropdown.append(
                '<option value="' + crmConstants.Enums.Country.NotApply + '">' + viewResources.Symbol_NotApply +
                '</option>');
                var countryListLength = scope.countryList.length;
                var countryId = scope.defaultSettings.CountryId;
                for (var i = 0; i < countryListLength; i++) {
                    var country = scope.countryList[i];
                    dropdown.append(
                    '<option value="' + country.Id + '"' +
                         (countryId === country.Id ? 'selected="selected"' : "") +
                     '>' +
                        country.Name +
                     '</option>');
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
                    //jshint unused:false
                    renderDropdown(scope, element);
                });
            }
        };
    });
})();