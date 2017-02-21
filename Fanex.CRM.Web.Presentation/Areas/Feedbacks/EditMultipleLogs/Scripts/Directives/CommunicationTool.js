(function () {
    "use strict";
    angular.module('edit-multiple-logs-application').directive('dropdownCommunicationTool', function () {
        function renderDropdown(scope, element) {
            if (scope.communicationToolList) {
                var dropdown = element.find("#communicationTool");
                dropdown.empty();
                var optionHtml = '';
                var communicationToolListLength = scope.communicationToolList.length;
                for (var i = 0; i < communicationToolListLength; i++) {
                    var communicationTool = scope.communicationToolList[i];
                    optionHtml += '<option value="' + communicationTool.Id + '"' +
                        (communicationTool.Id === scope.multipleLogs.CommunicationTool.Id ?
                        ' selected="selected"' :
                        '') +
                '>' + communicationTool.Name + '</option>';
                }

                dropdown.append(optionHtml);
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