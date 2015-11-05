(function () {
    "use strict";
    angular.module('communicationlog-edit-application').directive('dropdownResponse', function () {
        function renderDropdown(scope, element) {
            /* jshint unused: vars */
            if (scope.responseList) {
                var dropdown = element.find("#response");
                dropdown.empty();
                /* jshint camelcase: false */
                var responseListLength = scope.responseList.length;
                for (var i = 0; i < responseListLength; i++) {
                    var response = scope.responseList[i];
                    dropdown.append(
                    '<option value="' + response.Id + '"' +
                        (response.Id === scope.communicationLog.Response.Id ?
                        ' selected="selected"' :
                        '') +
                    '>' + response.Value + '</option>');
                }

                element.change(function () {
                    scope.updateSelectedResponse(dropdown.find('option:selected').val());
                });
            }
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    /* jshint unused: vars */
                    renderDropdown(scope, element);
                });
            }
        };
    });
})();