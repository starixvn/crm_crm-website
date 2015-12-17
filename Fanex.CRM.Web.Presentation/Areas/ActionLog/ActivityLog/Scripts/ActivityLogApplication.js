/* global actionLogDirectiveManager */
(function () {
    "use strict";
    angular.module('activitylog-application', []);

    actionLogDirectiveManager.registerFromDateDirective(angular.module('activitylog-application'));
    actionLogDirectiveManager.registerToDateDirective(angular.module('activitylog-application'));
})();
