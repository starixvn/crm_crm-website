"use strict";
/* global insertLogApplication */
insertLogApplication.directive('dropdownRole', function () {
    function renderDropdown(scope, element, attrs) {
        /* jshint unused: vars */
        if (scope.roleList) {
            var dropdown = element.find("#role");
            dropdown.empty();
            /* jshint camelcase: false */
            var roleListLength = scope.roleList.length;
            var roleId = scope.customerInfo.Role.Id;
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
                /* jshint unused: vars */
                renderDropdown(scope, element, attrs);
            });
        }
    };
});