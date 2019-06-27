(function () {
    "use strict";
    /* global crmConstants, viewResources */
    angular.module('edit-multiple-logs-application').directive('dropdownProvince', function () {
        function renderDropdown(scope, element) {
            if (scope.provinceList) {
                var dropdown = element.find("#province");
                dropdown.empty();
                /* jshint camelcase: false */               
                var provinceListLength = scope.provinceList.length;
                for (var i = 0; i < provinceListLength; i++) {
                    var province = scope.provinceList[i];
                    dropdown.append(
                    '<option value="' + province.Id + '"' +
                        (province.Id === scope.multipleLogs.Province.Id ?
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