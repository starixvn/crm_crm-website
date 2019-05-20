(function () {
    "use strict";
    /* global crmConstants, viewResources */
    angular.module('edit-multiple-logs-application').directive('dropdownProvince', function () {
        function renderDropdown(scope, element) {
            if (scope.provinceList) {
                var dropdown = element.find("#province");
                dropdown.empty();
                /* jshint camelcase: false */
                dropdown.append(
                '<option value="' + crmConstants.Enums.Country.NotApply + '"' +
                     (crmConstants.Enums.Country.NotApply === scope.multipleLogs.Province.Id ?
                        ' selected="selected"' :
                        '') +
                     '>' + viewResources.Symbol_NotApply +
                '</option>');
                var provinceListLength = scope.provinceList.length;
                for (var i = 0; i < provinceListLength; i++) {
                    var province = scope.provinceList[i];
                    dropdown.append(
                    '<option value="' + province.Id + '"' +
                        (province.Id === scope.multipleLogs.Country.Id ?
                        ' selected="selected"' :
                        '') +
                    '>' + province.Name + '</option>');
                }

                element.change(function () {
                    scope.updateSelectedProvince(dropdown.find('option:selected').val());
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