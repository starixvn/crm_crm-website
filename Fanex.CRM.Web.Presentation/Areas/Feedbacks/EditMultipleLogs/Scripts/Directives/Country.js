(function () {
    "use strict";
    /* global crmConstants, viewResources */
    angular.module('edit-multiple-logs-application').directive('dropdownCountry', function () {
        function renderDropdown(scope, element) {
            if (scope.countryList) {
                var dropdown = element.find("#country");
                dropdown.empty();
                /* jshint camelcase: false */
                dropdown.append(
                '<option value="' + crmConstants.Enums.Country.NotApply + '"' +
                     (crmConstants.Enums.Country.NotApply === scope.multipleLogs.Country.Id ?
                        ' selected="selected"' :
                        '') +
                     '>' + viewResources.Symbol_NotApply +
                '</option>');
                var countryListLength = scope.countryList.length;
                for (var i = 0; i < countryListLength; i++) {
                    var country = scope.countryList[i];
                    dropdown.append(
                    '<option value="' + country.Id + '"' +
                        (country.Id === scope.multipleLogs.Country.Id ?
                        ' selected="selected"' :
                        '') +
                    '>' + country.Name + '</option>');
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
                    /* jshint unused: false */
                    renderDropdown(scope, element);
                });
            }
        };
    });
})();