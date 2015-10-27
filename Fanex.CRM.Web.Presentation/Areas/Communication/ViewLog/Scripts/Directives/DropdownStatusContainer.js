(function () {
    "use strict";
    /* global viewResources,
    crmConstants */
    angular.module('communication-application').directive('dropdownStatusContainer', function () {
        function renderDropdown(scope, element, attrs) {
            /* jshint unused: vars */
            if (scope.statusList) {
                var dropdown = element.find("#status");
                dropdown.empty();
                var optionHtml = '';
                optionHtml += '<option value="' + crmConstants.Enums.Status.All + '">' + viewResources.All + '</option>';
                var statusLength = scope.statusList.length;
                for (var i = 0; i < statusLength; i++) {
                    var status = scope.statusList[i];
                    optionHtml += '<option value="' + status.Id + '">' + status.Name + '</option>';
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
                    /* jshint unused: vars */
                    renderDropdown(scope, element, attrs);
                });
            }
        };
    });
})();