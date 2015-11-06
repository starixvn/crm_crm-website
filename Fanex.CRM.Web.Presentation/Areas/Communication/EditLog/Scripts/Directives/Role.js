(function () {
    "use strict";
    angular.module('communicationlog-edit-application').directive('dropdownRole', function () {
        function renderDropdown(scope, element) {
            /* jshint unused: vars */
            if (scope.roleList) {
                var dropdown = element.find("#role");
                dropdown.empty();
                /* jshint camelcase: false */
                var roleListLength = scope.roleList.length;
                for (var i = 0; i < roleListLength; i++) {
                    var role = scope.roleList[i];
                    dropdown.append(
                    '<option value="' + role.Id + '"' +
                        (role.Id === scope.communicationLog.RoleInfo.Id ?
                        ' selected="selected"' :
                        '') +
                    '>' + role.Value + '</option>');
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