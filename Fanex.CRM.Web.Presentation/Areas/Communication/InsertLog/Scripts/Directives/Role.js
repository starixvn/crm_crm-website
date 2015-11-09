(function () {
    "use strict";
    angular.module('communicationlog-insert-application').directive('dropdownRole', function () {
        function renderDropdown(scope, element) {
            if (scope.roleList) {
                var dropdown = element.find("#role");
                dropdown.empty();
                var roleListLength = scope.roleList.length;
                var roleId = scope.defaultSettings.RoleId;
                for (var i = 0; i < roleListLength; i++) {
                    var role = scope.roleList[i];
                    dropdown.append(
                    '<option value="' + role.Id + '"' +
                         (roleId === role.Id ? 'selected="selected"' : "") +
                     '>' +
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