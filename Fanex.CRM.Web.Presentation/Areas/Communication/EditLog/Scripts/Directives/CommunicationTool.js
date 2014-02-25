"use strict";
/* global editLogApplication,
crmConstants */
editLogApplication.directive('dropdownCommunicationTool', function () {
    function renderDropdown(scope, element, attrs) {
        /* jshint unused: vars */
        if (scope.communicationToolList) {
            var dropdown = element.find("#communicationTool");
            dropdown.empty();
            /* jshint camelcase: false */
            var communicationToolListLength = scope.communicationToolList.length;
            for (var i = 0; i < communicationToolListLength; i++) {
                var communicationTool = scope.communicationToolList[i];
                dropdown.append(
                    '<option value="' + communicationTool.Id + '"' +
                        (communicationTool.Id === scope.communicationLog.CommunicationTool.Id ?
                        ' selected="selected"' :
                        '') +
                        (communicationTool.Id === crmConstants.Enums.CommunicationTool.NotApply ?
                        ' disabled="disabled"' :
                        '') +
                    '>' + communicationTool.Name + '</option>');
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
                /* jshint unused: vars */
                renderDropdown(scope, element, attrs);
            });
        }
    };
});