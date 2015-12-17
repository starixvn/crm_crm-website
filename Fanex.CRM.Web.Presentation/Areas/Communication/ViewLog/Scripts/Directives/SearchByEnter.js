(function () {
    "use strict";
    /* global keyboardManager */
    angular.module('communication-application').directive('searchByEnter', function () {
        return {
            restrict: 'C',
            scope: true,
            link: function (scope, element) {
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
})();