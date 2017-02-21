(function () {
    "use strict";
    angular.module('edit-multiple-logs-application').directive('dropdownStatus', function () {
        function renderDropdown(scope, element) {
            if (scope.statusList) {
                var dropdown = element.find("#status");
                dropdown.empty();
                var statusListLength = scope.statusList.length;
                for (var i = 0; i < statusListLength; i++) {
                    var status = scope.statusList[i];
                    dropdown.append(
                    '<option value="' + status.Id + '"' +
                        (status.Id === scope.multipleLogs.Status.Id ?
                        ' selected="selected"' :
                        '') +
                     '>' + status.Name + '</option>');
                }

                element.change(function () {
                    scope.updateSelectedStatus(dropdown.find('option:selected').val());
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