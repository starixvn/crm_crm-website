(function () {
    "use strict";
    angular.module('communicationlog-insert-application').directive('dropdownBetType', function () {
        function renderDropdown(scope, element) {
            /* jshint unused: vars */
            if (scope.betTypeList) {
                var dropdown = element.find("#betType");
                dropdown.empty();
                /* jshint camelcase: false */
                var betTypeListLength = scope.betTypeList.length;
                for (var i = 0; i < betTypeListLength; i++) {
                    var betType = scope.betTypeList[i];
                    dropdown.append(
                    '<option value="' + betType.Id + '">' + betType.Name + '</option>');
                }

                element.change(function () {
                    scope.updateSelectedBetType(dropdown.find('option:selected').val());
                });
            }
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    //jshint unused:false
                    renderDropdown(scope, element);
                });
            }
        };
    });
})();