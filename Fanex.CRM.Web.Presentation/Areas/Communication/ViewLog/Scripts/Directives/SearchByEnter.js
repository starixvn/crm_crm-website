"use strict";
/* global communicationApplication,
keyboardManager */
/* jshint unused: vars */
communicationApplication.directive('searchByEnter', function () {
    return {
        restrict: 'C',
        scope: true,
        link: function (scope, element, attrs) {
            element.keyup(function (e) {
                e = (e) ? e : window.event;
                var key = (e.keyCode) ? e.keyCode : e.which;
                if (key === keyboardManager.Enums.KeyCode.Enter) {
                    scope.searchCommunicationLogs();
                }

                return;
            });
        }
    };
});