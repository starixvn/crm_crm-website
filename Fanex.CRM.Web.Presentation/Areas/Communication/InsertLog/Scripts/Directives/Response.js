(function () {
    "use strict";
    angular.module('communicationlog-insert-application').directive('dropdownResponse', function () {
        function renderDropdown(scope, element) {
            if (scope.responseList) {
                var dropdown = element.find("#response");
                dropdown.empty();
                var responseListLength = scope.responseList.length;
                for (var i = 0; i < responseListLength; i++) {
                    var response = scope.responseList[i];
                    dropdown.append(
                    '<option value="' + response.Id + '">' + response.Value + '</option>');
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
                    /* jshint unused: false */
                    renderDropdown(scope, element);
                });
            }
        };
    });
})();