(function () {
    "use strict";
    angular.module('create-multiple-logs-application').directive('dropdownRole', function () {
        function renderDropdown(scope, element) {
            if (scope.roleList) {
                var dropdown = element.find("#role");
                dropdown.empty();
                var roleListLength = scope.roleList.length;
                for (var i = 0; i < roleListLength; i++) {
                    var role = scope.roleList[i];
                    dropdown.append(
                    '<option value="' + role.Id + '">' +
                        role.Value +
                     '</option>');
                }

                element.change(function () {
                    scope.updateSelectedRole(dropdown.find('option:selected').val());
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