(function () {
    "use strict";
    angular.module('communicationlog-insert-application').directive('dropdownCommunicationTool', function () {
        function renderDropdown(scope, element) {
            if (scope.communicationToolList) {
                var dropdown = element.find("#communicationTool");
                dropdown.empty();
                var communicationToolListLength = scope.communicationToolList.length;
                for (var i = 0; i < communicationToolListLength; i++) {
                    var communicationTool = scope.communicationToolList[i];
                    dropdown.append(
                    '<option value="' + communicationTool.Id + '">' + communicationTool.Name + '</option>');
                }

                element.change(function () {
                    scope.updateSelectedCommunicationTool(dropdown.find('option:selected').val());
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