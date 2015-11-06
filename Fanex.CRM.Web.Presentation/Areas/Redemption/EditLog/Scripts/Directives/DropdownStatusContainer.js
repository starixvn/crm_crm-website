(function () {
    "use strict";
    angular.module('redemption-editlog-application').directive('dropdownStatusContainer', function () {
        function renderDropdown(scope, element) {
            /* jshint unused: vars */
            if (scope.statusList) {
                var dropdown = element.find("#status");
                dropdown.empty();
                var optionHtml = '';

                var statusLength = scope.statusList.length;
                for (var i = 0; i < statusLength; i++) {
                    var status = scope.statusList[i];
                    optionHtml += '<option value="' + status.Id + '"' +
                        (scope.redemptionLog.Status.Id === status.Id ?
                        ' selected="selected"' :
                        '') + '>' + status.Name + '</option>';
                }

                dropdown.append(optionHtml);
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